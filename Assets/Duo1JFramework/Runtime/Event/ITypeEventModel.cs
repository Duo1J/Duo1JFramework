using System;

namespace Duo1JFramework.Event
{
    /// <summary>
    /// 类型事件模型接口
    /// </summary>
    public interface ITypeEventModel
    {
        /// <summary>
        /// 订阅类型事件
        /// </summary>
        void RegisterType<T>(TypeEventFunc<T> callback) where T : BaseTypeEvent;

        /// <summary>
        /// 订阅类型事件
        /// </summary>
        void RegisterType(Type t, TypeEventFunc<BaseTypeEvent> callback);

        /// <summary>
        /// 取消订阅类型事件
        /// </summary>
        bool UnRegisterType<T>(TypeEventFunc<T> callback) where T : BaseTypeEvent;

        /// <summary>
        /// 取消订阅类型事件
        /// </summary>
        bool UnRegisterType(Type t, TypeEventFunc<BaseTypeEvent> callback);

        /// <summary>
        /// 取消订阅类型事件
        /// </summary>
        bool UnRegisterType(Type t, object callback);

        /// <summary>
        /// 取消订阅类型事件下所有注册
        /// </summary>
        bool UnRegisterType<T>() where T : BaseTypeEvent;

        /// <summary>
        /// 取消订阅类型事件下所有注册
        /// </summary>
        bool UnRegisterType(Type t);

        /// <summary>
        /// 取消订阅所有类型事件
        /// </summary>
        void UnRegisterTypeAll();

        /// <summary>
        /// 广播类型事件
        /// </summary>
        void BroadcastType<T>(T e) where T : BaseTypeEvent;
    }
}

