using System;

namespace Duo1JFramework.Event
{
    public interface IEventModel
    {
        /// <summary>
        /// 订阅事件
        /// </summary>
        void Register(eEvent e, Action<object> callback);

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        bool UnRegister(eEvent e, Action<object> callback);

        /// <summary>
        /// 取消订阅事件下所有注册
        /// </summary>
        bool UnRegister(eEvent e);

        /// <summary>
        /// 取消订阅所有事件
        /// </summary>
        void UnRegisterAll();

        /// <summary>
        /// 发布事件
        /// </summary>
        void Dispatch(eEvent e, object args = null);
    }
}