using Duo1JFramework.Asset;
using Duo1JFramework.GamerInput;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Duo1JFramework.TimelineAPI
{
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
            AssetManager.Instance.LoadInsByType<GameObject>(timelinePath, (go) =>
            {
                TimelineData td = WrapTimelinePrefab(go);
                callback?.Invoke(td);
            }, loadType);
        }

        /// <summary>
        /// 同步加载Timeline
        /// </summary>
        public TimelineData LoadTimelineSync(string timelinePath, EAssetLoadType loadType = EAssetLoadType.Bundle)
        {
            GameObject go = AssetManager.Instance.LoadInsByTypeSync<GameObject>(timelinePath, loadType);
            return WrapTimelinePrefab(go);
        }

        /// <summary>
        /// 创建Timeline包装类
        /// </summary>
        private TimelineData WrapTimelinePrefab(GameObject go)
        {
            try
            {
                return new TimelineData(go);
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e);
                if (go != null)
                {
                    go.DestroyImmediate();
                }
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
            resumableList.Add(new ResumablePlayableWrap(playable, resumeMouse, resumeKey));
        }

        private void OnUpdate()
        {
            if (resumableList != null)
            {
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
                        resumableList.RemoveAt(i);
                    }
                    removeList.Clear();
                }
            }
        }

        protected override void OnDispose()
        {
            resumableList.Clear();
            resumableList = null;
        }

        protected override void OnInit()
        {
            resumableList = new List<ResumablePlayableWrap>();

            Reg.RegisterUpdate(OnUpdate);
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

            public bool Resume()
            {
                if (!CheckResumable())
                {
                    return false;
                }

                director.Resume();

                return true;
            }

            private void Pause()
            {
                director.Pause();
            }

            public bool CheckResumable()
            {
                return InputManager.GetMouseBtnDown(resumeMouse) || InputManager.GetKeyDown(resumeKey);
            }

            public ResumablePlayableWrap(Playable playable, int resumeMouse, KeyCode resumeKey)
            {
                this.playable = playable;
                this.resumeMouse = resumeMouse;
                this.resumeKey = resumeKey;

                director = TimelineUtil.GetDirectorByPlayble(playable);

                Pause();
            }
        }
    }
}
