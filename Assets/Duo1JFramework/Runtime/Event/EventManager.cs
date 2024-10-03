using System;

namespace Duo1JFramework.Event
{
    public class EventManager : MonoSingleton<EventManager>, IEventModel
    {
        private IEventModel eventModel;

        /// <summary>
        /// 订阅事件
        /// </summary>
        public void AddEvent(eEvent e, Action<object> callback)
        {
            GetEventModel().AddEvent(e, callback);
        }

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        public bool RemoveEvent(eEvent e, Action<object> callback)
        {
            return GetEventModel().RemoveEvent(e, callback);
        }

        /// <summary>
        /// 取消订阅事件下所有注册
        /// </summary>
        public bool RemoveEvent(eEvent e)
        {
            return GetEventModel().RemoveEvent(e);
        }

        /// <summary>
        /// 取消订阅所有事件
        /// </summary>
        public void RemoveAllEvent()
        {
            GetEventModel().RemoveAllEvent();
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        public void Broadcast(eEvent e, object args = null)
        {
            GetEventModel().Broadcast(e, args);
        }

        /// <summary>
        /// 添加事件模型
        /// </summary>
        public void SetEventModel(IEventModel eventModel)
        {
            if (this.eventModel != null)
            {
                this.eventModel.RemoveAllEvent();
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
