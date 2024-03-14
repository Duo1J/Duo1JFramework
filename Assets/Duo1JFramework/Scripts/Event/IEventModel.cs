using System;

namespace Duo1JFramework.Event
{
    public interface IEventModel
    {
        /// <summary>
        /// 订阅事件
        /// </summary>
        void AddEvent(eEvent e, Action<object> callback);

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        bool RemoveEvent(eEvent e, Action<object> callback);

        /// <summary>
        /// 取消订阅事件下所有注册
        /// </summary>
        bool RemoveEvent(eEvent e);

        /// <summary>
        /// 取消订阅所有事件
        /// </summary>
        void RemoveAllEvent();

        /// <summary>
        /// 发布事件
        /// </summary>
        void Broadcast(eEvent e, object args = null);
    }
}