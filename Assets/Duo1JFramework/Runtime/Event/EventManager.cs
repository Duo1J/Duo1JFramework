using System;

namespace Duo1JFramework.Event
{
    /// <summary>
    /// 事件管理器
    /// </summary>
    public class EventManager : MonoSingleton<EventManager>, IEventModel
    {
        /// <summary>
        /// 事件模型
        /// </summary>
        private IEventModel eventModel;

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
        /// 发布事件
        /// </summary>
        public void Broadcast(object e, object args = null)
        {
            GetEventModel().Broadcast(e, args);
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

        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
            SetEventModel(null);
        }
    }
}
