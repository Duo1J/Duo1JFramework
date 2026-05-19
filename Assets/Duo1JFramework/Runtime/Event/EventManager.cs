using System;
using System.Collections.Generic;

namespace Duo1JFramework.Event
{
    /// <summary>
    /// 事件管理器
    /// </summary>
    public class EventManager : MonoSingleton<EventManager>, IEventModel, ITypeEventModel
    {
        /// <summary>
        /// 事件模型
        /// </summary>
        private IEventModel eventModel = null;

        /// <summary>
        /// 类型事件模型
        /// </summary>
        private ITypeEventModel typeEventModel = null;

        /// <summary>
        /// 延迟事件集合
        /// </summary>
        private Queue<DelayEventData> delayEventQueue = null;

        #region Event

        /// <summary>
        /// 订阅事件
        /// </summary>
        public void Register(object e, Action<object> callback)
        {
            GetEventModel().Register(e, callback);
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        public void Register(eEvent e, Action<object> callback)
        {
            Register((object)e, callback);
        }

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        public bool UnRegister(object e, Action<object> callback)
        {
            return GetEventModel().UnRegister(e, callback);
        }

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        public bool UnRegister(eEvent e, Action<object> callback)
        {
            return UnRegister((object)e, callback);
        }

        /// <summary>
        /// 取消订阅事件下所有注册
        /// </summary>
        public bool UnRegister(object e)
        {
            return GetEventModel().UnRegister(e);
        }

        /// <summary>
        /// 取消订阅事件下所有注册
        /// </summary>
        public bool UnRegister(eEvent e)
        {
            return UnRegister((object)e);
        }

        /// <summary>
        /// 取消订阅所有事件
        /// </summary>
        public void UnRegisterAll()
        {
            GetEventModel().UnRegisterAll();
        }

        /// <summary>
        /// 广播事件
        /// </summary>
        public void Broadcast(object e, object args = null)
        {
            GetEventModel().Broadcast(e, args);
        }

        /// <summary>
        /// 广播事件
        /// </summary>
        public void Broadcast(eEvent e, object args = null)
        {
            Broadcast((object)e, args);
        }

        /// <summary>
        /// 广播事件延迟到LateUpdate或下一帧
        /// </summary>
        public void BroadcastDelay(object e, object args = null)
        {
            delayEventQueue.Enqueue(new DelayEventData(e, args));
        }

        /// <summary>
        /// 广播事件延迟到LateUpdate或下一帧
        /// </summary>
        public void BroadcastDelay(eEvent e, object args = null)
        {
            BroadcastDelay((object)e, args);
        }

        /// <summary>
        /// 设置事件模型
        /// </summary>
        public void SetEventModel(IEventModel eventModel)
        {
            if (this.eventModel != null)
            {
                this.eventModel.UnRegisterAll();
            }

            this.eventModel = eventModel;
        }

        /// <summary>
        /// 获取事件模型
        /// </summary>
        private IEventModel GetEventModel()
        {
            if (eventModel == null)
            {
                SetEventModel(new EventModel());
            }

            return eventModel;
        }

        #endregion Event

        #region Type Event

        /// <summary>
        /// 订阅类型事件
        /// </summary>
        public void RegisterType<T>(Action<T> callback) where T : BaseTypeEvent
        {
            GetTypeEventModel().RegisterType<T>(callback);
        }

        /// <summary>
        /// 订阅类型事件
        /// </summary>
        public void RegisterType(Type t, Action<BaseTypeEvent> callback)
        {
            GetTypeEventModel().RegisterType(t, callback);
        }

        /// <summary>
        /// 取消订阅类型事件
        /// </summary>
        public bool UnRegisterType<T>(Action<T> callback) where T : BaseTypeEvent
        {
            return GetTypeEventModel().UnRegisterType<T>(callback);
        }

        /// <summary>
        /// 取消订阅类型事件
        /// </summary>
        public bool UnRegisterType(Type t, Action<BaseTypeEvent> callback)
        {
            return GetTypeEventModel().UnRegisterType(t, callback);
        }

        /// <summary>
        /// 取消订阅类型事件
        /// </summary>
        public bool UnRegisterType(Type t, object callback)
        {
            return GetTypeEventModel().UnRegisterType(t, callback);
        }

        /// <summary>
        /// 取消订阅类型事件下所有注册
        /// </summary>
        public bool UnRegisterType<T>() where T : BaseTypeEvent
        {
            return GetTypeEventModel().UnRegisterType<T>();
        }

        /// <summary>
        /// 取消订阅类型事件下所有注册
        /// </summary>
        public bool UnRegisterType(Type t)
        {
            return GetTypeEventModel().UnRegisterType(t);
        }

        /// <summary>
        /// 取消订阅所有类型事件
        /// </summary>
        public void UnRegisterTypeAll()
        {
            GetTypeEventModel().UnRegisterTypeAll();
        }

        /// <summary>
        /// 广播类型事件
        /// </summary>
        public void BroadcastType<T>(T e) where T : BaseTypeEvent
        {
            GetTypeEventModel().BroadcastType<T>(e);
        }

        /// <summary>
        /// 设置类型事件模型
        /// </summary>
        public void SetTypeEventModel(ITypeEventModel typeEventModel)
        {
            if (this.typeEventModel != null)
            {
                this.typeEventModel.UnRegisterTypeAll();
            }

            this.typeEventModel = typeEventModel;
        }

        /// <summary>
        /// 获取类型事件模型
        /// </summary>
        private ITypeEventModel GetTypeEventModel()
        {
            if (typeEventModel == null)
            {
                SetTypeEventModel(new TypeEventModel());
            }

            return typeEventModel;
        }

        #endregion Type Event

        private void OnLateUpdate()
        {
            if (delayEventQueue.Count > 0)
            {
                Queue<DelayEventData> invokeQueue = delayEventQueue;
                delayEventQueue = new Queue<DelayEventData>();

                while (invokeQueue.Count > 0)
                {
                    DelayEventData data = invokeQueue.Dequeue();
                    Broadcast(data.Event, data.Args);
                }
            }
        }

        protected override void OnInit()
        {
            delayEventQueue = new Queue<DelayEventData>();

            Reg.RegisterLateUpdate(OnLateUpdate);
        }

        protected override void OnDispose()
        {
            SetEventModel(null);
            SetTypeEventModel(null);

            if (delayEventQueue != null)
            {
                delayEventQueue.Clear();
                delayEventQueue = null;
            }
        }

        private struct DelayEventData
        {
            public object Event { get; }
            public object Args { get; }

            public DelayEventData(object e, object args)
            {
                Event = e;
                Args = args;
            }
        }
    }
}
