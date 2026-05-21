using System;
using UnityEngine;

namespace Duo1JFramework.Scheduling
{
    /// <summary>
    /// 计时器实体
    /// </summary>
    public class Timer : IEditorDrawer
    {
        /// <summary>
        /// 周期 (毫秒数/帧数)
        /// </summary>
        private float interval;

        /// <summary>
        /// 当前运行时间 (毫秒数/帧数)
        /// </summary>
        private float curInterval;

        /// <summary>
        /// 重复次数，<0 (Def.TIMER_REPEAT_FOREVER)为无限
        /// </summary>
        /// <see cref="Def.TIMER_REPEAT_FOREVER"/>
        private int repeat;

        /// <summary>
        /// 当前重复次数
        /// </summary>
        private int curRepeat;

        /// <summary>
        /// 是否运行中
        /// </summary>
        private bool isRunning;

        /// <summary>
        /// 是否是帧计时器
        /// </summary>
        private bool isFrameTimer;

        /// <summary>
        /// 计时回调
        /// </summary>
        private Action callback;

        /// <summary>
        /// 启动时间
        /// </summary>
        private float startTime;

        /// <summary>
        /// 初始化
        /// </summary>
        private bool init;

        /// <summary>
        /// 调试的栈信息
        /// </summary>
        private string stackTrace_Debug;

        /// <summary>
        /// 开启计时器
        /// </summary>
        public Timer Start()
        {
            Assert.Guard(init, "Timer尚未初始化，请调用Init进行初始化");

            if (isRunning)
            {
                return this;
            }
            isRunning = true;
            TimerManager.Instance.RegisterTimer(this);

            if (Game.IsDebug)
            {
                stackTrace_Debug = DbgUtil.GetStackTrace();
            }

            return this;
        }

        /// <summary>
        /// 暂停计时器
        /// </summary>
        public Timer Stop()
        {
            isRunning = false;
            TimerManager.Instance.UnRegisterTimer(this);

            return this;
        }

        /// <summary>
        /// 重置计时器
        /// </summary>
        public Timer Reset()
        {
            Stop();

            curInterval = 0;
            curRepeat = 0;
            startTime = Time.unscaledTime;

            return this;
        }

        /// <summary>
        /// 销毁计时器，再次使用需要重新初始化
        /// </summary>
        public void Dispose()
        {
            init = false;
            Stop();
        }

        /// <summary>
        /// 计时器初始化
        /// </summary>
        /// <param name="interval">毫秒数/帧数</param>
        /// <param name="isFrameTimer">是否是帧计时器</param>
        /// <param name="repeat">重复次数，<0 (Def.TIMER_REPEAT_FOREVER)为无限</param>
        /// <param name="callback">计时回调</param>
        /// <see cref="Def.TIMER_REPEAT_FOREVER"/>
        public Timer Init(float interval, bool isFrameTimer, Action callback, int repeat)
        {
            if (interval <= 0)
            {
                Log.ErrorForce(isFrameTimer ? "帧计时器frame必须大于0" : "计时器interval必须大于0");
                interval = 1;
            }

            if (repeat == 0)
            {
                Log.ErrorForce("计时器重复次数repeat不可为0");
                repeat = 1;
            }
            else if (repeat < 0)
            {
                repeat = Def.Timer.REPEAT_FOREVER;
            }

            this.interval = interval;
            this.repeat = repeat;
            this.isFrameTimer = isFrameTimer;
            this.callback = callback;

            Reset();
            init = true;

            return this;
        }

        /// <summary>
        /// 计时器更新
        /// </summary>
        /// <see cref="TimerManager.Update"/>
        public void Tick()
        {
            if (!isRunning)
            {
                return;
            }

            if (repeat != Def.Timer.REPEAT_FOREVER && curRepeat >= repeat)
            {
                Stop();
                return;
            }

            if (isFrameTimer)
            {
                curInterval++;
            }
            else
            {
                curInterval = Time.unscaledTime - startTime;
            }

            if (curInterval >= interval)
            {
                Execute();
            }
        }

        private void Execute()
        {
            if (isFrameTimer)
            {
                InvokeCallbackSafe();
                startTime = Time.unscaledTime;
            }
            else
            {
                int executeTimes = Mathf.Max(1, Mathf.FloorToInt(curInterval / interval));
                for (int i = 0; i < executeTimes; i++)
                {
                    InvokeCallbackSafe();
                }
                startTime = Mathf.Max(0, Time.unscaledTime - curInterval % interval);
            }

            curInterval = 0;
            curRepeat++;
        }

        private void InvokeCallbackSafe()
        {
            try
            {
                callback?.Invoke();
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, "计时器回调异常");
            }
        }

        public Timer(float interval, bool isFrameTimer, Action callback, int repeat)
        {
            Init(interval, isFrameTimer, callback, repeat);
        }

        private Vector2 scrollPos;

        /// <summary>
        /// 绘制监视器信息
        /// </summary>
        public void DrawEditorInfo()
        {
            ED.Vertical(() =>
            {
                ED.Horizontal(() =>
                {
                    GUILayout.Label($"是否初始化: {init}");
                    GUILayout.Label($"帧计时器: {isFrameTimer}");
                    GUILayout.Label($"周期: {interval}");
                    GUILayout.Label($"重复次数: {curRepeat}/{repeat}");
                });

                ED.Horizontal(() =>
                {
                    GUILayout.Label($"运行中: {isRunning}");
                    GUILayout.Label($"启动时间: {startTime}");
                    GUILayout.Label($"当前运行时间: {curInterval}");
                });

                if (Game.IsDebug)
                {
                    GUILayout.Label("Start()调用栈");
                    ED.Scroll(ref scrollPos, () =>
                    {
                        GUILayout.Label(stackTrace_Debug);
                    }, "box", GUILayout.MaxHeight(70));
                }
            });
        }

        public Timer()
        {
        }
    }
}