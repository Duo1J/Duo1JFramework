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
        /// <summary>
        /// 运行中计时器集合
        /// </summary>
        private HashSet<Timer> timerSet;

        /// <summary>
        /// 待移除计时器集合
        /// </summary>
        private HashSet<Timer> removeSet;

        /// <summary>
        /// 获取一个计时器
        /// </summary>
        /// <param name="interval">毫秒数</param>
        /// <param name="repeat">重复次数，<0 (Def.TIMER_REPEAT_FOREVER)为无限</param>
        public Timer GetTimer(float interval, Action callback, int repeat = 1)
        {
            return _GetTimer(interval, false, callback, repeat);
        }

        /// <summary>
        /// 获取一个帧计时器
        /// </summary>
        /// <param name="interval">帧数</param>
        /// <param name="repeat">重复次数，<0 (Def.TIMER_REPEAT_FOREVER)为无限</param>
        public Timer GetFrameTimer(int frame, Action callback, int repeat = 1)
        {
            return _GetTimer(frame, true, callback, repeat);
        }

        /// <summary>
        /// 从池中启动一个一次性计时器
        /// </summary>
        /// <param name="interval">秒数</param>
        public void GetTimerFromPool(float interval, Action callback)
        {
            Assert.NotNull(callback, "计时器回调不可为空");
            Timer timer = Pool.TimerPool.Pop();
            timer.Init(interval, false, () =>
            {
                try
                {
                    callback.Invoke();
                }
                catch (Exception e)
                {
                    Assert.ExceptHandle(e, "GetTimerFromPool计时器回调异常");
                }
                finally
                {
                    Pool.TimerPool.Push(timer);
                }
            }, 1);
        }

        /// <summary>
        /// 从池中启动一个一次性帧计时器
        /// </summary>
        public void GetFrameTimerFromPool(int frame, Action callback)
        {
            Assert.NotNull(callback, "计时器回调不可为空");
            Timer timer = Pool.TimerPool.Pop();
            timer.Init(frame, true, () =>
            {
                try
                {
                    callback.Invoke();
                }
                catch (Exception e)
                {
                    Assert.ExceptHandle(e, "GetFrameTimerFromPool计时器回调异常");
                }
                finally
                {
                    Pool.TimerPool.Push(timer);
                }
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
            if (timerSet == null)
            {
                return;
            }
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

        private Timer _GetTimer(float interval, bool isFrameTimer, Action callback, int repeat)
        {
            Assert.NotNull(callback, "计时器回调不可为空");
            return new Timer(interval, isFrameTimer, callback, repeat);
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

#if UNITY_EDITOR

        public HashSet<Timer> TimerSet_Editor => timerSet;
        public HashSet<Timer> RemoveSet_Editor => removeSet;

#endif
    }
}