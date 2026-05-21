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
        private static readonly object locker = new object();

        /// <summary>
        /// 主线程ID
        /// </summary>
        private int mainThreadID;

        /// <summary>
        /// 主线程任务列表
        /// </summary>
        private List<Action> mainThreadTaskList;

        /// <summary>
        /// 主线程任务执行缓存
        /// </summary>
        private List<Action> mainThreadExecutingList;

        /// <summary>
        /// 延迟一帧执行任务集合
        /// </summary>
        private HashSet<Action> delayOneFrameTaskSet;

        /// <summary>
        /// 延迟一帧执行缓存
        /// </summary>
        private List<Action> delayOneFrameTaskList;

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

            if (Instance.mainThreadID == Thread.CurrentThread.ManagedThreadId)
            {
                action.InvokeSafe();
                return;
            }

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
            return QueueOnThreadPool((state) => { action(); });
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

        private void OnEarlyUpdate()
        {
            if (delayOneFrameTaskSet != null && delayOneFrameTaskSet.Count > 0)
            {
                delayOneFrameTaskList.Clear();
                delayOneFrameTaskList.AddRange(delayOneFrameTaskSet);
                delayOneFrameTaskSet.Clear();

                foreach (Action action in delayOneFrameTaskList)
                {
                    action.InvokeSafe();
                }

                delayOneFrameTaskList.Clear();
            }
        }

        private void OnLateUpdate()
        {
            lock (locker)
            {
                if (mainThreadTaskList != null && mainThreadTaskList.Count > 0)
                {
                    List<Action> temp = mainThreadExecutingList;
                    mainThreadExecutingList = mainThreadTaskList;
                    mainThreadTaskList = temp;
                }
            }

            if (mainThreadExecutingList != null && mainThreadExecutingList.Count > 0)
            {
                foreach (Action action in mainThreadExecutingList)
                {
                    action.InvokeSafe();
                }

                mainThreadExecutingList.Clear();
            }
        }

        protected override void OnInit()
        {
            mainThreadID = Thread.CurrentThread.ManagedThreadId;
            mainThreadTaskList = new List<Action>();
            mainThreadExecutingList = new List<Action>();
            delayOneFrameTaskSet = new HashSet<Action>();
            delayOneFrameTaskList = new List<Action>();

            Reg.RegisterEarlyUpdate(OnEarlyUpdate);
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

                if (mainThreadExecutingList != null)
                {
                    mainThreadExecutingList.Clear();
                    mainThreadExecutingList = null;
                }
            }

            if (delayOneFrameTaskSet != null)
            {
                delayOneFrameTaskSet.Clear();
                delayOneFrameTaskSet = null;
            }

            if (delayOneFrameTaskList != null)
            {
                delayOneFrameTaskList.Clear();
                delayOneFrameTaskList = null;
            }
        }

        #endregion Inner
    }
}