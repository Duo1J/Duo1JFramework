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
        }
    }
}