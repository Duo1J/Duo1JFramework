using Duo1JFramework.Event;
using Duo1JFramework.TimerUpdate;
using System;
using System.Collections.Generic;

namespace Duo1JFramework
{
    /// <summary>
    /// 注册器基类
    /// </summary>
    /// <see cref="Register"/>
    /// <see cref="MonoRegister"/>
    public abstract class BaseRegister : IDispose
    {
        /// <summary>
        /// PreUpdate注册的更新回调
        /// </summary>
        private Action preUpdater;

        /// <summary>
        /// Update注册的更新回调
        /// </summary>
        private Action updater;

        /// <summary>
        /// LateUpdate注册的更新回调
        /// </summary>
        private Action lateUpdater;

        /// <summary>
        /// FixedUpdate注册的更新回调
        /// </summary>
        private Action fixedUpdater;

        /// <summary>
        /// 计时器列表
        /// </summary>
        private List<Timer> timerList;

        /// <summary>
        /// 事件列表
        /// </summary>
        private Dictionary<eEvent, List<Action<object>>> eventDict;

        /// <summary>
        /// 该类是否已准备销毁
        /// </summary>
        public bool Disposed { get; protected set; }

        #region Update

        /// <summary>
        /// 注册PreUpdate回调
        /// </summary>
        public void RegisterPreUpdate(Action _preUpdater)
        {
            if (CheckDisposed())
            {
                return;
            }

            UpdateManager.Instance.RegisterPreUpdate(_preUpdater);
            preUpdater = _preUpdater;
        }

        /// <summary>
        /// 取消注册PreUpdate回调
        /// </summary>
        public void UnRegisterPreUpdate()
        {
            if (CheckDisposed())
            {
                return;
            }

            if (preUpdater == null)
            {
                return;
            }

            UpdateManager.Instance.UnRegisterPreUpdate(preUpdater);
            preUpdater = null;
        }

        /// <summary>
        /// 注册Update回调
        /// </summary>
        public void RegisterUpdate(Action _updater)
        {
            if (CheckDisposed())
            {
                return;
            }

            UpdateManager.Instance.RegisterUpdate(_updater);
            updater = _updater;
        }

        /// <summary>
        /// 取消注册Update回调
        /// </summary>
        public void UnRegisterUpdate()
        {
            if (CheckDisposed())
            {
                return;
            }

            if (updater == null)
            {
                return;
            }

            UpdateManager.Instance.UnRegisterUpdate(updater);
            updater = null;
        }

        /// <summary>
        /// 注册LateUpdate回调
        /// </summary>
        public void RegisterLateUpdate(Action _lateUpdater)
        {
            if (CheckDisposed())
            {
                return;
            }

            UpdateManager.Instance.RegisterLateUpdate(_lateUpdater);
            lateUpdater = _lateUpdater;
        }

        /// <summary>
        /// 取消注册Update回调
        /// </summary>
        public void UnRegisterLateUpdate()
        {
            if (CheckDisposed())
            {
                return;
            }

            if (lateUpdater == null)
            {
                return;
            }

            UpdateManager.Instance.UnRegisterLateUpdate(lateUpdater);
            lateUpdater = null;
        }

        /// <summary>
        /// 注册FixedUpdate回调
        /// </summary>
        public void RegisterFixedUpdate(Action _fixedUpdater)
        {
            if (CheckDisposed())
            {
                return;
            }

            UpdateManager.Instance.RegisterFixedUpdate(_fixedUpdater);
            fixedUpdater = _fixedUpdater;
        }

        /// <summary>
        /// 取消注册FixedUpdate回调
        /// </summary>
        public void UnRegisterFixedUpdate()
        {
            if (CheckDisposed())
            {
                return;
            }

            if (fixedUpdater == null)
            {
                return;
            }

            UpdateManager.Instance.UnRegisterFixedUpdate(fixedUpdater);
            fixedUpdater = null;
        }

        #endregion Update

        #region Timer

        /// <summary>
        /// 获取一个计时器
        /// </summary>
        public Timer GetTimer(float interval, Action callback, int repeat = 1)
        {
            if (CheckDisposed())
            {
                return null;
            }

            Timer timer = TimerManager.Instance.GetTimer(interval, callback, repeat);
            if (timerList == null)
            {
                timerList = new List<Timer>();
            }
            timerList.Add(timer);
            return timer;
        }

        /// <summary>
        /// 获取一个帧计时器
        /// </summary>
        public Timer GetFrameTimer(int frame, Action callback, int repeat = 1)
        {
            if (CheckDisposed())
            {
                return null;
            }

            Timer timer = TimerManager.Instance.GetFrameTimer(frame, callback, repeat);
            if (timerList == null)
            {
                timerList = new List<Timer>();
            }
            timerList.Add(timer);
            return timer;
        }

        /// <summary>
        /// 停止计时器
        /// </summary>
        public void StopTimer(Timer timer)
        {
            if (CheckDisposed())
            {
                return;
            }

            timer.Stop();
            if (timerList == null) return;
            timerList.Remove(timer);
        }

        /// <summary>
        /// 停止所有计时器
        /// </summary>
        public void StopAllTimer()
        {
            if (CheckDisposed())
            {
                return;
            }

            if (timerList == null)
            {
                return;
            }

            foreach (Timer timer in timerList)
            {
                timer.Stop();
            }
            timerList = null;
        }

        #endregion Timer

        #region Event

        /// <summary>
        /// 注册事件
        /// </summary>
        public void RegisterEvent(eEvent e, Action<object> callback)
        {
            if (CheckDisposed())
            {
                return;
            }

            if (eventDict == null)
            {
                eventDict = new Dictionary<eEvent, List<Action<object>>>();
            }
            if (!eventDict.TryGetValue(e, out List<Action<object>> list))
            {
                list = new List<Action<object>>();
                eventDict.Add(e, list);
            }
            list.Add(callback);

            EventManager.Instance.AddEvent(e, callback);
        }

        /// <summary>
        /// 取消注册事件
        /// </summary>
        public void UnRegisterEvent(eEvent e, Action<object> callback)
        {
            if (CheckDisposed())
            {
                return;
            }

            if (eventDict != null)
            {
                if (eventDict.TryGetValue(e, out List<Action<object>> list))
                {
                    list.Remove(callback);
                }
            }

            EventManager.Instance.RemoveEvent(e, callback);
        }

        /// <summary>
        /// 取消注册所有事件
        /// </summary>
        public void UnRegisterAllEvent()
        {
            if (CheckDisposed())
            {
                return;
            }

            if (eventDict == null)
            {
                return;
            }

            foreach (KeyValuePair<eEvent, List<Action<object>>> kv in eventDict)
            {
                foreach (Action<object> callback in kv.Value)
                {
                    EventManager.Instance.RemoveEvent(kv.Key, callback);
                }
            }
        }

        #endregion Event

        private bool CheckDisposed()
        {
            if (Disposed)
            {
                Log.ErrorForce($"`{GetHashCode()}`计时器已销毁");
                return true;
            }

            return false;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            Dispose();
            Disposed = false;
            OnReset();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            if (Game.IsQuit)
            {
                return;
            }

            if (Disposed)
            {
                return;
            }

            try
            {
                UnRegisterPreUpdate();
                UnRegisterUpdate();
                UnRegisterLateUpdate();
                UnRegisterFixedUpdate();
                StopAllTimer();
                UnRegisterAllEvent();

                OnDispose();
            }
            finally
            {
                Disposed = true;
            }
        }

        /// <summary>
        /// 子类重置
        /// </summary>
        protected virtual void OnReset()
        {
        }

        /// <summary>
        /// 子类销毁
        /// </summary>
        protected abstract void OnDispose();

        ~BaseRegister()
        {
            Dispose();
        }
    }
}