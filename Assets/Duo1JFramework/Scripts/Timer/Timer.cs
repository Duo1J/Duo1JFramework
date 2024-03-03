using System;
using UnityEngine;

namespace Duo1JFramework.TimerUpdate
{
    /// <summary>
    /// 计时器结构
    /// </summary>
    public class Timer : IEditorDrawer
    {
        /// <summary>
        /// 周期 (毫秒数/帧数)
        /// </summary>
        private float interval;

        /// <summary>
        /// 是否是帧计时器
        /// </summary>
        private bool isFrameTimer;

        /// <summary>
        /// 计时回调
        /// </summary>
        private Action callback;

        /// <summary>
        /// 重复次数，-1为无限
        /// </summary>
        /// <see cref="Def.TIMER_REPEAT_FOREVER"/>
        private int repeat;

        /// <summary>
        /// 是否运行中
        /// </summary>
        private bool isRunning;

        /// <summary>
        /// 当前运行时间
        /// </summary>
        private float curInterval;

        /// <summary>
        /// 当前重复次数
        /// </summary>
        private int curRepeat;

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
            Assert.Guard(init, "Timer尚未初始化，请调用Init初始化");
            if (isRunning) return this;

            isRunning = true;
            startTime = Time.unscaledTime;
            TimerManager.Instance.RegisterTimer(this);

            if (Game.IsDebug)
            {
                stackTrace_Debug = Log.GetStackTrace();
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
        /// 计时器更新
        /// </summary>
        /// <see cref="TimerManager.Update"/>
        public void Tick()
        {
            if (!isRunning) return;

            if (repeat != Def.TIMER_REPEAT_FOREVER && curRepeat >= repeat)
            {
                Stop();
            }

            if (curInterval >= interval)
            {
                Execute();
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
        }

        private void Execute()
        {
            if (isFrameTimer)
            {
                callback?.Invoke();
                startTime = Time.unscaledTime;
            }
            else
            {
                int executeTimes = Mathf.Max(1, Mathf.FloorToInt(curInterval / interval));
                for (int i = 0; i < executeTimes; i++)
                {
                    callback?.Invoke();
                }
                startTime = Mathf.Max(0, Time.unscaledTime - curInterval % interval);
            }
            curInterval = 0;
            curRepeat++;
        }

        public Timer(float interval, bool isFrameTimer, Action callback, int repeat)
        {
            Init(interval, isFrameTimer, callback, repeat);
        }

        public Timer Init(float interval, bool isFrameTimer, Action callback, int repeat)
        {
            if (repeat == 0)
            {
                Log.Error("计时器重复次数repeat不可为0");
                repeat = 1;
            }
            else if (repeat < 0)
            {
                repeat = Def.TIMER_REPEAT_FOREVER;
            }

            this.interval = interval;
            this.isFrameTimer = isFrameTimer;
            this.callback = callback;
            this.repeat = repeat;

            Reset();
            init = true;
            return this;
        }

        private Vector2 scrollPos;

        public void Draw()
        {
            LU.Vertical(() =>
            {
                LU.Horizontal(() =>
                {
                    GUILayout.Label($"是否初始化: {init}");
                    GUILayout.Label($"帧计时器: {isFrameTimer}");
                    GUILayout.Label($"周期: {interval}");
                    GUILayout.Label($"重复次数: {curRepeat}/{repeat}");
                });

                LU.Horizontal(() =>
                {
                    GUILayout.Label($"运行中: {isRunning}");
                    GUILayout.Label($"启动时间: {startTime}");
                    GUILayout.Label($"当前运行时间: {curInterval}");
                });

                if (Game.IsDebug)
                {
                    GUILayout.Label("Start()调用栈");
                    LU.Scroll(ref scrollPos, () =>
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