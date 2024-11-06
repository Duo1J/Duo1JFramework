using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Duo1JFramework.Scheduling
{
    /// <summary>
    /// 任务调度器
    /// </summary>
    public class Scheduler : MonoSingleton<Scheduler>
    {
        #region Delay

        /// <summary>
        /// 延迟时间执行
        /// </summary>
        public static Timer Delay(float interval, Action action)
        {
            return TimerManager.Instance.StartTimer(interval, action, 1);
        }

        /// <summary>
        /// 延迟帧执行
        /// </summary>
        public static Timer DelayFrame(int frame, Action action)
        {
            return TimerManager.Instance.StartFrameTimer(frame, action, 1);
        }

        /// <summary>
        /// 延迟一帧执行
        /// 重复传入相同的委托只执行一次
        /// 不可取消
        /// </summary>
        public static void DelayOneFrame(Action action)
        {
            Assert.NotNullArg(action, "action");
            Instance.delayOneFrameTaskSet.Add(action);
        }

        #endregion Delay

        #region Coroutine

        /// <summary>
        /// 开启协程
        /// </summary>
        public static Coroutine StartCoro(IEnumerator e)
        {
            return CoroManager.StartCoro(e);
        }

        /// <summary>
        /// 停止协程
        /// </summary>
        public static void StopCoro(IEnumerator e)
        {
            CoroManager.StopCoro(e);
        }

        /// <summary>
        /// 停止协程
        /// </summary>
        public static void StopCoro(Coroutine c)
        {
            CoroManager.StopCoro(c);
        }

        #endregion Coroutine

        #region Thread

        /// <summary>
        /// 主线程队列执行
        /// </summary>
        public static void QueueOnMainThread(Action action)
        {
            Assert.NotNullArg(action, "action");

            if (Instance.mainThreadTaskList == null)
            {
                Log.ErrorForce("主线程任务队列已销毁");
                return;
            }

            lock (locker)
            {
                Instance.mainThreadTaskList.Add(action);
            }
        }

        /// <summary>
        /// 线程池队列执行
        /// </summary>
        public static bool QueueOnThreadPool(WaitCallback action)
        {
            Assert.NotNullArg(action, "action");
            return ThreadPool.QueueUserWorkItem(action);
        }

        /// <summary>
        /// 线程池队列执行
        /// </summary>
        public static bool QueueOnThreadPool(Action action)
        {
            Assert.NotNullArg(action, "action");
            return QueueOnThreadPool((state) =>
            {
                action();
            });
        }

        /// <summary>
        /// Task队列执行
        /// </summary>
        public static Task QueueOnTask(Action action)
        {
            Assert.NotNullArg(action, "action");
            return Task.Run(action);
        }

        /// <summary>
        /// Task队列执行, 可取消
        /// </summary>
        public static Task QueueOnTask(Action action, CancellationToken cancellationToken)
        {
            Assert.NotNullArg(action, "action");
            return Task.Run(action, cancellationToken);
        }

        #endregion Thread

        #region Inner

        private static readonly object locker = new object();

        /// <summary>
        /// 主线程任务列表
        /// </summary>
        private List<Action> mainThreadTaskList;

        /// <summary>
        /// 延迟一帧执行任务集合
        /// </summary>
        private HashSet<Action> delayOneFrameTaskSet;

        private void OnPreUpdate()
        {
            if (delayOneFrameTaskSet != null)
            {
                foreach (Action action in delayOneFrameTaskSet)
                {
                    action();
                }

                delayOneFrameTaskSet.Clear();
            }
        }

        private void OnLateUpdate()
        {
            lock (locker)
            {
                if (mainThreadTaskList != null)
                {
                    foreach (Action action in mainThreadTaskList)
                    {
                        action();
                    }

                    mainThreadTaskList.Clear();
                }
            }
        }

        protected override void OnInit()
        {
            mainThreadTaskList = new List<Action>();
            delayOneFrameTaskSet = new HashSet<Action>();

            Reg.RegisterLateUpdate(OnPreUpdate);
            Reg.RegisterLateUpdate(OnLateUpdate);
        }

        protected override void OnDispose()
        {
            lock (locker)
            {
                if (mainThreadTaskList != null)
                {
                    mainThreadTaskList.Clear();
                    mainThreadTaskList = null;
                }
            }

            if (delayOneFrameTaskSet != null)
            {
                delayOneFrameTaskSet.Clear();
                delayOneFrameTaskSet = null;
            }
        }

        #endregion Inner
    }
}
