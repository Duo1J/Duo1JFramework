using Duo1JFramework.ObjectPool;
using System;
using System.Collections.Generic;

namespace Duo1JFramework.Scheduling
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
        /// 待添加计时器集合
        /// </summary>
        private HashSet<Timer> addSet;

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
        /// 开启一个计时器
        /// </summary>
        /// <param name="interval">毫秒数</param>
        /// <param name="repeat">重复次数，<0 (Def.TIMER_REPEAT_FOREVER)为无限</param>
        public Timer StartTimer(float interval, Action callback, int repeat = 1)
        {
            return GetTimer(interval, callback, repeat).Start();
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
        /// 开启一个帧计时器
        /// </summary>
        /// <param name="interval">帧数</param>
        /// <param name="repeat">重复次数，<0 (Def.TIMER_REPEAT_FOREVER)为无限</param>
        public Timer StartFrameTimer(int frame, Action callback, int repeat = 1)
        {
            return GetFrameTimer(frame, callback, repeat).Start();
        }

        /// <summary>
        /// 从池中启动一个一次性计时器
        /// </summary>
        /// <param name="interval">秒数</param>
        public void GetTimerFromPool(float interval, Action callback)
        {
            Assert.NotNullArg(callback, "callback");
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

            timer.Start();
        }

        /// <summary>
        /// 从池中启动一个一次性帧计时器
        /// </summary>
        public void GetFrameTimerFromPool(int frame, Action callback)
        {
            Assert.NotNullArg(callback, "callback");
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

        #region Inner

        /// <summary>
        /// 内部注册计时器
        /// </summary>
        public void RegisterTimer(Timer timer)
        {
            if (timerSet == null)
            {
                timerSet = new HashSet<Timer>();
            }

            if (addSet == null)
            {
                addSet = new HashSet<Timer>();
            }

            if (removeSet != null)
            {
                removeSet.Remove(timer);
            }

            if (!timerSet.Contains(timer))
            {
                addSet.Add(timer);
            }
        }

        /// <summary>
        /// 内部取消注册计时器
        /// </summary>
        public void UnRegisterTimer(Timer timer)
        {
            if (timerSet == null)
            {
                return;
            }

            addSet?.Remove(timer);

            if (removeSet == null)
            {
                removeSet = new HashSet<Timer>();
            }

            removeSet.Add(timer);
        }

        private void OnLateUpdate()
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

                if (addSet != null)
                {
                    foreach (Timer timer in addSet)
                    {
                        timerSet.Add(timer);
                    }
                    addSet.Clear();
                }

                foreach (Timer timer in timerSet)
                {
                    try
                    {
                        timer.Tick();
                    }
                    catch (Exception e)
                    {
                        Assert.ExceptHandle(e, "计时器Tick异常");
                    }
                }
            }
        }

        /// <summary>
        /// 内部获取计时器
        /// </summary>
        private Timer _GetTimer(float interval, bool isFrameTimer, Action callback, int repeat)
        {
            Assert.NotNullArg(callback, "callback");
            return new Timer(interval, isFrameTimer, callback, repeat);
        }

        protected override void OnDispose()
        {
            timerSet = null;
            removeSet = null;
            addSet = null;
        }

        protected override void OnInit()
        {
            timerSet = new HashSet<Timer>();
            removeSet = new HashSet<Timer>();
            addSet = new HashSet<Timer>();

            Reg.RegisterLateUpdate(OnLateUpdate);
        }

#if UNITY_EDITOR

        public HashSet<Timer> TimerSet_Editor => timerSet;
        public HashSet<Timer> RemoveSet_Editor => removeSet;
        public HashSet<Timer> AddSet_Editor => addSet;

#endif

        #endregion Inner
    }
}
