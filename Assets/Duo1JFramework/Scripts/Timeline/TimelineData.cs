using Duo1JFramework.Actor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

using UObject = UnityEngine.Object;

namespace Duo1JFramework.Timeline
{
    /// <summary>
    /// Timeline包装数据
    /// </summary>
    public class TimelineData
    {
        private GameObject go;
        private PlayableDirector pd;
        private Dictionary<string, PlayableBinding> bindingDict;

        public GameObject Go => go;
        public Transform Tf => Go.transform;
        public PlayableDirector Pd => pd;

        public Action<TimelineData> OnPlayed;
        public Action<TimelineData> OnPaused;
        public Action<TimelineData> OnStopped;

        /// <summary>
        /// 动态绑定
        /// </summary>
        /// <param name="key">轨道Key值</param>
        /// <param name="tarObj">绑定目标</param>
        public TimelineData SetGenericBinding(string key, UObject tarObj)
        {
            if (bindingDict.TryGetValue(key, out PlayableBinding binding))
            {
                UObject preObj = pd.GetGenericBinding(binding.sourceObject);
                if (preObj != null)
                {
                    Log.LevelWarn(LogLevel.Timeline, $"{ToString()}上绑定{key}时，原绑定不为空");

                    if (preObj is Component comp)
                    {
                        comp.gameObject.SetActive(false);
                    }
                }

                pd.SetGenericBinding(binding.sourceObject, tarObj);
                Log.Level(LogLevel.Timeline, $"{ToString()} -> 绑定{tarObj.name}到{key}");
            }
            else
            {
                Log.LevelError(LogLevel.Timeline, $"{ToString()}上未找到Key: {key}，无法执行绑定");
            }
            return this;
        }

        /// <summary>
        /// 和目标同步旋转和位移
        /// </summary>
        public TimelineData SyncTransform(Transform target)
        {
            Tf.rotation = Quaternion.LookRotation(target.forward, Vector3.up);
            Tf.position = target.position;
            return this;
        }

        public TimelineData SyncTransform(BaseActor baseActor)
        {
            return SyncTransform(baseActor.ModelTf);
        }

        public TimelineData Play()
        {
            pd.Play();
            Log.Level(LogLevel.Timeline, $"{ToString()} -> Play()");
            return this;
        }

        public TimelineData Stop()
        {
            pd.Stop();
            Log.Level(LogLevel.Timeline, $"{ToString()} -> Stop()");
            return this;
        }

        public TimelineData Pause()
        {
            pd.Pause();
            Log.Level(LogLevel.Timeline, $"{ToString()} -> Pause()");
            return this;
        }

        public TimelineData Resume()
        {
            pd.Resume();
            Log.Level(LogLevel.Timeline, $"{ToString()} -> Resume()");
            return this;
        }

        public TimelineData Restart()
        {
            Pause();
            SetTime(0);
            Play();
            Log.Level(LogLevel.Timeline, $"{ToString()} -> Restart()");
            return this;
        }

        public TimelineData SetTime(float time)
        {
            pd.time = time;
            Log.Level(LogLevel.Timeline, $"{ToString()} -> SetTime({time})");
            return this;
        }

        public TimelineData SetInitialTime(float time)
        {
            pd.initialTime = time;
            Log.Level(LogLevel.Timeline, $"{ToString()} -> SetInitialTime({time})");
            return this;
        }

        public TimelineData SetWrapMode(DirectorWrapMode wrapMode)
        {
            pd.extrapolationMode = wrapMode;
            Log.Level(LogLevel.Timeline, $"{ToString()} -> SetWrapMode({wrapMode})");
            return this;
        }

        public TimelineData RebuildGraph()
        {
            pd.RebuildGraph();
            Log.Level(LogLevel.Timeline, $"{ToString()} -> RebuildGraph()");
            return this;
        }

        public TimelineData DestroyOnStop()
        {
            //todo hlj 停止回调注册销毁事件
            return this;
        }

        /// <summary>
        /// 初始化绑定映射字典
        /// </summary>
        /// <param name="force">是否强制刷新</param>
        public TimelineData InitBindingDict(bool force = false)
        {
            if (!force && bindingDict != null)
            {
                return this;
            }
            bindingDict = new Dictionary<string, PlayableBinding>();
            foreach (PlayableBinding binding in pd.playableAsset.outputs)
            {
                bindingDict.Add(binding.streamName, binding);
            }
            return this;
        }

        public override string ToString()
        {
            return $"<Timeline-{go.name}-{go.GetInstanceID()}>";
        }

        public TimelineData(GameObject go)
        {
            Assert.NotNull(go, "Timeline预制体Go为空，无法创建TimelineData");
            this.go = go;

            PlayableDirector pd = go.GetComponent<PlayableDirector>();
            Assert.NotNull(pd, $"无法从{ToString()}上获取PlayableDirector");
            this.pd = pd;

            this.go.SetParent(Root.Instance.TimelineRoot);
            this.pd.playOnAwake = false;

            InitBindingDict();
            RegisterCallback();
        }

        private void RegisterCallback()
        {
            if (pd == null)
            {
                return;
            }

            pd.played += (pd) =>
            {
                OnPlayed?.Invoke(this);
            };

            pd.paused += (pd) =>
            {
                OnPaused?.Invoke(this);
            };

            pd.stopped += (pd) =>
            {
                OnStopped?.Invoke(this);
            };
        }
    }
}