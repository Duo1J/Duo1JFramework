using Duo1JFramework.Asset;
using Duo1JFramework.GamerInput;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

using UObject = UnityEngine.Object;

namespace Duo1JFramework.TimelineAPI
{
    /// <summary>
    /// Timeline播放参数
    /// </summary>
    public class TimelinePlayOptions
    {
        public Transform SyncTarget;
        public Vector3? Position;
        public Quaternion? Rotation;
        public DirectorWrapMode? WrapMode;
        public Dictionary<string, UObject> Bindings;
        public bool DisablePreviousBinding;
        public bool DestroyOnStop;
        public Action<TimelineData> OnPlayed;
        public Action<TimelineData> OnPaused;
        public Action<TimelineData> OnStopped;
        public Action<TimelineData> OnDestroyed;
        public bool AutoPlay = true;
    }

    /// <summary>
    /// Timeline管理器
    /// </summary>
    public class TimelineManager : MonoSingleton<TimelineManager>
    {
        private List<ResumablePlayableWrap> resumableList;

        /// <summary>
        /// 异步加载Timeline
        /// </summary>
        public void LoadTimeline(string timelinePath, Action<TimelineData> callback = null, EAssetLoadType loadType = EAssetLoadType.Bundle)
        {
            AssetManager.Instance.LoadByType<GameObject>(timelinePath, (handle) =>
            {
                TimelineData td = WrapTimelinePrefab(handle);
                callback?.Invoke(td);
            }, loadType);
        }

        /// <summary>
        /// 同步加载Timeline
        /// </summary>
        public TimelineData LoadTimelineSync(string timelinePath, EAssetLoadType loadType = EAssetLoadType.Bundle)
        {
            IAssetHandle<GameObject> handle = AssetManager.Instance.LoadByTypeSync<GameObject>(timelinePath, loadType);
            return WrapTimelinePrefab(handle);
        }

        /// <summary>
        /// 异步播放Timeline
        /// </summary>
        public void PlayTimeline(string timelinePath, TimelinePlayOptions options = null, Action<TimelineData> callback = null, EAssetLoadType loadType = EAssetLoadType.Bundle)
        {
            LoadTimeline(timelinePath, (td) =>
            {
                ApplyPlayOptions(td, options);
                callback?.Invoke(td);
            }, loadType);
        }

        /// <summary>
        /// 同步播放Timeline
        /// </summary>
        public TimelineData PlayTimelineSync(string timelinePath, TimelinePlayOptions options = null, EAssetLoadType loadType = EAssetLoadType.Bundle)
        {
            TimelineData td = LoadTimelineSync(timelinePath, loadType);
            ApplyPlayOptions(td, options);
            return td;
        }

        /// <summary>
        /// 创建Timeline包装类
        /// </summary>
        private TimelineData WrapTimelinePrefab(IAssetHandle<GameObject> handle)
        {
            if (handle == null)
            {
                return null;
            }

            GameObject go = handle.Instantiate();
            try
            {
                return new TimelineData(go);
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e);
                go?.DestroySmart();
                return null;
            }
        }

        /// <summary>
        /// Playable设置可恢复暂停
        /// </summary>
        public void SetResumablePause(Playable playable, int resumeMouse = Def.Input.INPUT_MOUSE_NONE, KeyCode resumeKey = KeyCode.None)
        {
            if (resumableList == null)
            {
                resumableList = new List<ResumablePlayableWrap>();
            }

            PlayableDirector director = TimelineUtil.GetDirectorByPlayable(playable);
            if (director == null)
            {
                return;
            }

            foreach (ResumablePlayableWrap item in resumableList)
            {
                if (item.IsSameDirector(director))
                {
                    return;
                }
            }

            ResumablePlayableWrap wrap = new ResumablePlayableWrap(playable, director, resumeMouse, resumeKey);
            if (wrap.IsValid)
            {
                resumableList.Add(wrap);
            }
        }

        private void OnUpdate()
        {
            if (resumableList == null || resumableList.Count == 0)
            {
                return;
            }

            List<ResumablePlayableWrap> removeList = null;

            InputManager.IgnoreLimit(() =>
            {
                foreach (ResumablePlayableWrap item in resumableList)
                {
                    if (item.Resume())
                    {
                        if (removeList == null)
                        {
                            removeList = new List<ResumablePlayableWrap>();
                        }
                        removeList.Add(item);
                    }
                }
            });

            if (removeList != null)
            {
                for (int i = 0; i < removeList.Count; i++)
                {
                    resumableList.Remove(removeList[i]);
                }
                removeList.Clear();
            }
        }

        protected override void OnDispose()
        {
            resumableList?.Clear();
            resumableList = null;
        }

        protected override void OnInit()
        {
            resumableList = new List<ResumablePlayableWrap>();

            Reg.RegisterUpdate(OnUpdate);
        }

        private void ApplyPlayOptions(TimelineData td, TimelinePlayOptions options)
        {
            if (td == null)
            {
                return;
            }

            if (options == null)
            {
                td.Play();
                return;
            }

            if (options.SyncTarget != null)
            {
                td.SyncTransform(options.SyncTarget);
            }
            else
            {
                if (options.Position.HasValue)
                {
                    td.Tf.position = options.Position.Value;
                }

                if (options.Rotation.HasValue)
                {
                    td.Tf.rotation = options.Rotation.Value;
                }
            }

            if (options.WrapMode.HasValue)
            {
                td.SetWrapMode(options.WrapMode.Value);
            }

            if (options.Bindings != null)
            {
                td.SetGenericBindings(options.Bindings, options.DisablePreviousBinding);
            }

            if (options.OnPlayed != null)
            {
                td.OnPlayed += options.OnPlayed;
            }

            if (options.OnPaused != null)
            {
                td.OnPaused += options.OnPaused;
            }

            if (options.OnStopped != null)
            {
                td.OnStopped += options.OnStopped;
            }

            if (options.OnDestroyed != null)
            {
                td.SetDestroyCallback(options.OnDestroyed);
            }

            if (options.DestroyOnStop)
            {
                td.DestroyOnStop();
            }

            if (options.AutoPlay)
            {
                td.Play();
            }
        }

        /// <summary>
        /// 可恢复Playable包装
        /// </summary>
        private class ResumablePlayableWrap
        {
            private PlayableDirector director;
            public Playable playable;
            public int resumeMouse;
            public KeyCode resumeKey;

            public bool IsValid => director != null;

            public bool Resume()
            {
                if (!IsValid)
                {
                    return true;
                }

                if (!CheckResumable())
                {
                    return false;
                }

                director.Resume();

                return true;
            }

            private void Pause()
            {
                if (director != null)
                {
                    director.Pause();
                }
            }

            public bool CheckResumable()
            {
                return InputManager.GetMouseBtnDown(resumeMouse) || InputManager.GetKeyDown(resumeKey);
            }

            public bool IsSameDirector(PlayableDirector targetDirector)
            {
                return director == targetDirector;
            }

            public ResumablePlayableWrap(Playable playable, PlayableDirector director, int resumeMouse, KeyCode resumeKey)
            {
                this.playable = playable;
                this.director = director;
                this.resumeMouse = resumeMouse;
                this.resumeKey = resumeKey;

                Pause();
            }
        }
    }
}