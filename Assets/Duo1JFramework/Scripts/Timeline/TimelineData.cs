using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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

        public GameObject GO => go;
        public PlayableDirector PD => pd;

        public void Play()
        {
            pd.Play();
            Log.Level(LogLevel.Timeline, $"{ToString()} -> Play()");
        }

        public void Stop()
        {
            pd.Stop();
            Log.Level(LogLevel.Timeline, $"{ToString()} -> Stop()");
        }

        public void Pause()
        {
            pd.Pause();
            Log.Level(LogLevel.Timeline, $"{ToString()} -> Pause()");
        }

        public void Resume()
        {
            pd.Resume();
            Log.Level(LogLevel.Timeline, $"{ToString()} -> Resume()");
        }

        public void Restart()
        {
            Pause();
            SetTime(0);
            Play();
            Log.Level(LogLevel.Timeline, $"{ToString()} -> Restart()");
        }

        public void SetTime(float time)
        {
            pd.time = time;
            Log.Level(LogLevel.Timeline, $"{ToString()} -> SetTime({time})");
        }

        public void SetInitialTime(float time)
        {
            pd.initialTime = time;
            Log.Level(LogLevel.Timeline, $"{ToString()} -> SetInitialTime({time})");
        }

        public void SetWrapMode(DirectorWrapMode wrapMode)
        {
            pd.extrapolationMode = wrapMode;
            Log.Level(LogLevel.Timeline, $"{ToString()} -> SetWrapMode({wrapMode})");
        }

        public void RebuildGraph()
        {
            pd.RebuildGraph();
            Log.Level(LogLevel.Timeline, $"{ToString()} -> RebuildGraph()");
        }

        public void SetGenericBinding(string key, Object tarObj)
        {
            if (bindingDict.TryGetValue(key, out PlayableBinding binding))
            {
                Object preObj = pd.GetGenericBinding(binding.sourceObject);
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
        }

        public void InitBindingDict(bool force = false)
        {
            if (!force && bindingDict != null)
            {
                return;
            }
            bindingDict = new Dictionary<string, PlayableBinding>();
            foreach (PlayableBinding binding in pd.playableAsset.outputs)
            {
                bindingDict.Add(binding.streamName, binding);
            }
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
        }
    }
}