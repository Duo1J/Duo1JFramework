using Duo1JFramework.Scheduling;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using DTimer = Duo1JFramework.Scheduling.Timer;

namespace Duo1JFramework
{
    /// <summary>
    /// 委托扩展
    /// </summary>
    public static class DelegateExtend
    {
        /// <summary>
        /// 委托安全调用
        /// </summary>
        public static void InvokeSafe(this Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e);
            }
        }

        /// <summary>
        /// 泛型委托安全调用
        /// </summary>
        public static void InvokeSafe<T>(this Action<T> action, T param)
        {
            try
            {
                action.Invoke(param);
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e);
            }
        }

        /// <summary>
        /// 延迟时间执行
        /// </summary>
        public static DTimer InvokeDelay(this Action action, float interval)
        {
            return Scheduler.Delay(interval, action);
        }

        /// <summary>
        /// 延迟帧执行
        /// </summary>
        public static DTimer InvokeDelayFrame(this Action action, int frame)
        {
            return Scheduler.DelayFrame(frame, action);
        }

        /// <summary>
        /// 延迟一帧执行
        /// 重复传入相同的委托只执行一次
        /// 不可取消
        /// </summary>
        public static void InvokeDelayOneFrame(this Action action)
        {
            Scheduler.DelayOneFrame(action);
        }

        /// <summary>
        /// 主线程队列执行
        /// </summary>
        public static void InvokeOnMainThread(this Action action)
        {
            Scheduler.QueueOnMainThread(action);
        }

        /// <summary>
        /// 线程池队列执行
        /// </summary>
        public static void InvokeOnThreadPool(this Action action)
        {
            Scheduler.QueueOnThreadPool(action);
        }

        /// <summary>
        /// Task队列执行
        /// </summary>
        public static void InvokeOnTask(this Action action)
        {
            Scheduler.QueueOnTask(action);
        }

        /// <summary>
        /// Task队列执行, 可取消
        /// </summary>
        public static void InvokeOnTask(this Action action, CancellationToken cancellationToken)
        {
            Scheduler.QueueOnTask(action, cancellationToken);
        }

        /// <summary>
        /// 开启协程
        /// </summary>
        public static Coroutine StartCoro(this IEnumerator e)
        {
            return Scheduler.StartCoro(e);
        }

        /// <summary>
        /// 停止协程
        /// </summary>
        public static void StopCoro(this IEnumerator e)
        {
            Scheduler.StopCoro(e);
        }

        /// <summary>
        /// 停止协程
        /// </summary>
        public static void StopCoro(this Coroutine c)
        {
            Scheduler.StopCoro(c);
        }
    }
}
