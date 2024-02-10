using Duo1JFramework.ObjectPool;
using System;
using System.Collections.Generic;

namespace Duo1JFramework.TimerUpdate
{
    /// <summary>
    /// 计时器管理器
    /// </summary>
    public class TimerManager : MonoSingleton<TimerManager>
    {
        private HashSet<Timer> timerSet;
        private HashSet<Timer> removeSet;

        /// <summary>
        /// 获取一个计时器
        /// </summary>
        /// <param name="interval">秒数</param>
        public Timer GetTimer(float interval, Action callback, int repeat = 1)
        {
            return _GetTimer(interval, false, callback, repeat);
        }

        /// <summary>
        /// 获取一个帧计时器
        /// </summary>
        public Timer GetFrameTimer(int frame, Action callback, int repeat = 1)
        {
            return _GetTimer(frame, true, callback, repeat);
        }

        private Timer _GetTimer(float interval, bool isFrameTimer, Action callback, int repeat)
        {
            Assert.NotNull(callback, "计时器回调不可为空");
            return new Timer(interval, isFrameTimer, callback, repeat);
        }

        /// <summary>
        /// 从池中启动一个一次性计时器
        /// </summary>
        /// <param name="interval">秒数</param>
        public void GetTimerFromPool(float interval, Action callback)
        {
            Assert.NotNull(callback, "计时器回调不可为空");
            ObjectPoolItem<Timer> item = Pool.TimerPool.Pop();
            item.Value.Init(interval, false, () =>
            {
                callback.Invoke();
                Pool.TimerPool.Push(item);
            }, 1);
        }

        /// <summary>
        /// 从池中启动一个一次性帧计时器
        /// </summary>
        public void GetFrameTimerFromPool(int frame, Action callback)
        {
            Assert.NotNull(callback, "计时器回调不可为空");
            ObjectPoolItem<Timer> item = Pool.TimerPool.Pop();
            item.Value.Init(frame, true, () =>
            {
                callback.Invoke();
                Pool.TimerPool.Push(item);
            }, 1);
        }

        public void RegisterTimer(Timer timer)
        {
            if (timerSet == null)
            {
                timerSet = new HashSet<Timer>();
            }
            timerSet.Add(timer);
            removeSet?.Remove(timer);
        }

        public void UnRegisterTimer(Timer timer)
        {
            if (timerSet == null) return;
            if (removeSet == null)
            {
                removeSet = new HashSet<Timer>();
            }
            removeSet.Add(timer);
        }

        private void Update()
        {
            if (timerSet != null)
            {
                if (removeSet != null)
                {
                    foreach (Timer timer in removeSet)
                    {
                        timerSet.Remove(timer);
                    }
                    removeSet.Clear();
                }

                foreach (Timer timer in timerSet)
                {
                    timer.Tick();
                }
            }
        }

        protected override void OnDispose()
        {
            timerSet = null;
            removeSet = null;
        }

        protected override void OnInit()
        {
            timerSet = new HashSet<Timer>();
            removeSet = new HashSet<Timer>();
        }
    }
}