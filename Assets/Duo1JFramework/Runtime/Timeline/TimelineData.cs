using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

using UObject = UnityEngine.Object;

namespace Duo1JFramework.TimelineAPI
{
    /// <summary>
    /// Timeline包装数据
    /// </summary>
    public class TimelineData
    {
        private GameObject go;
        private PlayableDirector pd;
        private Dictionary<string, PlayableBinding> bindingDict;
        private Action<PlayableDirector> playedHandler;
        private Action<PlayableDirector> pausedHandler;
        private Action<PlayableDirector> stoppedHandler;

        public GameObject Go => go;
        public Transform Tf => Go.transform;
        public PlayableDirector Pd => pd;

        public bool IsDestroyed { get; private set; }

        public event Action<TimelineData> OnPlayed;
        public event Action<TimelineData> OnPaused;
        public event Action<TimelineData> OnStopped;

        private Action<TimelineData> OnDestroyed;
        protected Action<TimelineData> InnerOnPlayed;
        protected Action<TimelineData> InnerOnPaused;
        protected Action<TimelineData> InnerOnStopped;

        /// <summary>
        /// 动态绑定
        /// </summary>
        /// <param name="key">轨道Key值</param>
        /// <param name="tarObj">绑定目标，传入null时表示解绑</param>
        /// <param name="disablePrevious">是否禁用旧绑定对象</param>
        public TimelineData SetGenericBinding(string key, UObject tarObj, bool disablePrevious = false)
        {
            if (!CheckAvailable("SetGenericBinding"))
            {
                return this;
            }

            if (string.IsNullOrEmpty(key))
            {
                Log.LevelError(ELogLevel.Timeline, $"{ToString()}绑定Key为空，无法执行绑定");
                return this;
            }

            if (bindingDict == null)
            {
                InitBindingDict();
            }

            if (bindingDict.TryGetValue(key, out PlayableBinding binding))
            {
                UObject preObj = pd.GetGenericBinding(binding.sourceObject);
                if (preObj != null && preObj != tarObj)
                {
                    Log.LevelWarn(ELogLevel.Timeline, $"{ToString()}绑定{key}时，原绑定不为空");

                    if (disablePrevious && preObj is Component comp)
                    {
                        comp.gameObject.SetActive(false);
                    }
                }

                pd.SetGenericBinding(binding.sourceObject, tarObj);
                string tarObjName = tarObj == null ? "null" : tarObj.name;
                Log.LevelInfo(ELogLevel.Timeline, $"{ToString()}绑定{tarObjName}到{key}");
            }
            else
            {
                Log.LevelError(ELogLevel.Timeline, $"{ToString()}未找到Key: {key}，无法执行绑定");
            }
            return this;
        }

        /// <summary>
        /// 批量动态绑定
        /// </summary>
        /// <param name="bindings">绑定映射</param>
        /// <param name="disablePrevious">是否禁用旧绑定对象</param>
        public TimelineData SetGenericBindings(Dictionary<string, UObject> bindings, bool disablePrevious = false)
        {
            if (bindings == null)
            {
                return this;
            }

            foreach (KeyValuePair<string, UObject> item in bindings)
            {
                SetGenericBinding(item.Key, item.Value, disablePrevious);
            }

            return this;
        }

        /// <summary>
        /// 和目标同步旋转和位移
        /// </summary>
        public TimelineData SyncTransform(Transform target)
        {
            if (!CheckAvailable("SyncTransform") || target == null)
            {
                return this;
            }

            Tf.rotation = Quaternion.LookRotation(target.forward, Vector3.up);
            Tf.position = target.position;
            return this;
        }

        public TimelineData Play()
        {
            if (!CheckAvailable("Play"))
            {
                return this;
            }

            pd.Play();
            Log.LevelInfo(ELogLevel.Timeline, $"{ToString()} -> Play()");
            return this;
        }

        public TimelineData Stop()
        {
            if (!CheckAvailable("Stop"))
            {
                return this;
            }

            pd.Stop();
            Log.LevelInfo(ELogLevel.Timeline, $"{ToString()} -> Stop()");
            return this;
        }

        public TimelineData Pause()
        {
            if (!CheckAvailable("Pause"))
            {
                return this;
            }

            pd.Pause();
            Log.LevelInfo(ELogLevel.Timeline, $"{ToString()} -> Pause()");
            return this;
        }

        public TimelineData Resume()
        {
            if (!CheckAvailable("Resume"))
            {
                return this;
            }

            pd.Resume();
            Log.LevelInfo(ELogLevel.Timeline, $"{ToString()} -> Resume()");
            return this;
        }

        public TimelineData Restart()
        {
            if (!CheckAvailable("Restart"))
            {
                return this;
            }

            Pause();
            SetTime(0);
            Play();
            Log.LevelInfo(ELogLevel.Timeline, $"{ToString()} -> Restart()");
            return this;
        }

        public TimelineData SetTime(float time)
        {
            if (!CheckAvailable("SetTime"))
            {
                return this;
            }

            pd.time = time;
            Log.LevelInfo(ELogLevel.Timeline, $"{ToString()} -> SetTime({time})");
            return this;
        }

        public TimelineData SetInitialTime(float time)
        {
            if (!CheckAvailable("SetInitialTime"))
            {
                return this;
            }

            pd.initialTime = time;
            Log.LevelInfo(ELogLevel.Timeline, $"{ToString()} -> SetInitialTime({time})");
            return this;
        }

        public TimelineData SetWrapMode(DirectorWrapMode wrapMode)
        {
            if (!CheckAvailable("SetWrapMode"))
            {
                return this;
            }

            pd.extrapolationMode = wrapMode;
            Log.LevelInfo(ELogLevel.Timeline, $"{ToString()} -> SetWrapMode({wrapMode})");
            return this;
        }

        public TimelineData RebuildGraph()
        {
            if (!CheckAvailable("RebuildGraph"))
            {
                return this;
            }

            pd.RebuildGraph();
            Log.LevelInfo(ELogLevel.Timeline, $"{ToString()} -> RebuildGraph()");
            return this;
        }

        public void Destroy()
        {
            if (IsDestroyed)
            {
                return;
            }

            IsDestroyed = true;
            GameObject destroyGo = go;

            if (pd != null)
            {
                UnRegisterCallback();
                pd.Stop();
            }

            OnDestroyed?.Invoke(this);
            ClearCallbacks();

            if (bindingDict != null)
            {
                bindingDict.Clear();
                bindingDict = null;
            }

            go = null;
            pd = null;

            destroyGo?.DestroySmart();
        }

        public TimelineData DestroyOnStop()
        {
            if (IsDestroyed)
            {
                return this;
            }

            InnerOnStopped += (td) =>
            {
                Destroy();
            };

            return this;
        }

        public TimelineData SetDestroyCallback(Action<TimelineData> onDestroyed)
        {
            if (IsDestroyed)
            {
                onDestroyed?.Invoke(this);
                return this;
            }

            OnDestroyed = onDestroyed;
            return this;
        }

        public override string ToString()
        {
            if (go == null)
            {
                return "<Timeline-Destroyed>";
            }

            return $"<Timeline-{go.name}-{go.GetInstanceID()}>";
        }

        public TimelineData(GameObject go)
        {
            Assert.NotNullArg(go, "go");
            this.go = go;
            this.pd = go.GetAndAssertComponent<PlayableDirector>($"无法从{ToString()}上获取PlayableDirector");

            this.go.SetParent(Root.TimelineRoot);
            this.pd.playOnAwake = false;

            InitBindingDict();
            RegisterCallback();
        }

        /// <summary>
        /// 初始化绑定映射字典
        /// </summary>
        /// <param name="force">是否强制刷新</param>
        public TimelineData InitBindingDict(bool force = false)
        {
            if (!CheckAvailable("InitBindingDict"))
            {
                return this;
            }

            if (!force && bindingDict != null)
            {
                return this;
            }

            bindingDict = new Dictionary<string, PlayableBinding>();
            if (pd.playableAsset == null)
            {
                Log.LevelError(ELogLevel.Timeline, $"{ToString()}未设置PlayableAsset，无法初始化绑定映射");
                return this;
            }

            foreach (PlayableBinding binding in pd.playableAsset.outputs)
            {
                if (string.IsNullOrEmpty(binding.streamName))
                {
                    Log.LevelWarn(ELogLevel.Timeline, $"{ToString()}存在空绑定Key，已忽略");
                    continue;
                }

                if (bindingDict.ContainsKey(binding.streamName))
                {
                    Log.LevelWarn(ELogLevel.Timeline, $"{ToString()}存在重复绑定Key: {binding.streamName}，已忽略后续重复项");
                    continue;
                }

                bindingDict.Add(binding.streamName, binding);
            }

            return this;
        }

        private void RegisterCallback()
        {
            if (pd == null)
            {
                return;
            }

            playedHandler = (pd) =>
            {
                InnerOnPlayed?.Invoke(this);
                OnPlayed?.Invoke(this);
            };

            pausedHandler = (pd) =>
            {
                InnerOnPaused?.Invoke(this);
                OnPaused?.Invoke(this);
            };

            stoppedHandler = (pd) =>
            {
                Action<TimelineData> onStopped = OnStopped;
                InnerOnStopped?.Invoke(this);
                onStopped?.Invoke(this);
            };

            pd.played += playedHandler;
            pd.paused += pausedHandler;
            pd.stopped += stoppedHandler;
        }

        private void UnRegisterCallback()
        {
            if (pd == null)
            {
                return;
            }

            if (playedHandler != null)
            {
                pd.played -= playedHandler;
                playedHandler = null;
            }

            if (pausedHandler != null)
            {
                pd.paused -= pausedHandler;
                pausedHandler = null;
            }

            if (stoppedHandler != null)
            {
                pd.stopped -= stoppedHandler;
                stoppedHandler = null;
            }
        }

        private void ClearCallbacks()
        {
            OnPlayed = null;
            OnPaused = null;
            OnStopped = null;
            OnDestroyed = null;
            InnerOnPlayed = null;
            InnerOnPaused = null;
            InnerOnStopped = null;
        }

        private bool CheckAvailable(string apiName)
        {
            if (!IsDestroyed && go != null && pd != null)
            {
                return true;
            }

            Log.LevelWarn(ELogLevel.Timeline, $"{ToString()}已销毁，无法执行{apiName}");
            return false;
        }
    }
}