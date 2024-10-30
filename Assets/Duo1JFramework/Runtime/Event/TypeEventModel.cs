using System;
using System.Collections.Generic;

namespace Duo1JFramework.Event
{
    /// <summary>
    /// 类型事件模型
    /// </summary>
    public class TypeEventModel : ITypeEventModel
    {
        /// <summary>
        /// 类型事件字典
        /// </summary>
        private Dictionary<Type, HashSet<object>> typeEventDict = null;

        /// <summary>
        /// 订阅类型事件
        /// </summary>
        public void RegisterType<T>(TypeEventFunc<T> callback) where T : BaseTypeEvent
        {
            Assert.NotNullArg(callback, "callback");

            RegisterType(typeof(T), callback);
        }

        /// <summary>
        /// 订阅类型事件
        /// </summary>
        public void RegisterType(Type t, TypeEventFunc<BaseTypeEvent> callback)
        {
            Assert.NotNullArg(t, "t");
            Assert.NotNullArg(callback, "callback");

            RegisterType(t, callback);
        }

        /// <summary>
        /// 订阅类型事件
        /// </summary>
        private void RegisterType(Type t, object callback)
        {
            if (!typeEventDict.TryGetValue(t, out HashSet<object> set))
            {
                set = new HashSet<object>();
                typeEventDict.Add(t, set);
            }

            set.Add(callback);
        }

        /// <summary>
        /// 取消订阅类型事件
        /// </summary>
        public bool UnRegisterType<T>(TypeEventFunc<T> callback) where T : BaseTypeEvent
        {
            Assert.NotNullArg(callback, "callback");

            return UnRegisterType(typeof(T), callback);
        }

        /// <summary>
        /// 取消订阅类型事件
        /// </summary>
        public bool UnRegisterType(Type t, TypeEventFunc<BaseTypeEvent> callback)
        {
            Assert.NotNullArg(t, "t");
            Assert.NotNullArg(callback, "callback");

            return UnRegisterType(t, callback);
        }

        /// <summary>
        /// 取消订阅类型事件
        /// </summary>
        public bool UnRegisterType(Type t, object callback)
        {
            Assert.NotNullArg(t, "t");
            Assert.NotNullArg(callback, "callback");

            if (!typeEventDict.TryGetValue(t, out HashSet<object> set))
            {
                Log.ErrorForce($"类型事件 `{t.FullName}` 未找到任何订阅，无法取消");
                return false;
            }

            return set.Remove(callback);
        }

        /// <summary>
        /// 取消订阅类型事件下所有注册
        /// </summary>
        public bool UnRegisterType<T>() where T : BaseTypeEvent
        {
            return UnRegisterType(typeof(T));
        }

        /// <summary>
        /// 取消订阅类型事件下所有注册
        /// </summary>
        public bool UnRegisterType(Type t)
        {
            Assert.NotNullArg(t, "t");

            return typeEventDict.Remove(t);
        }

        /// <summary>
        /// 取消订阅类型事件下所有注册
        /// </summary>
        public void UnRegisterTypeAll()
        {
            typeEventDict.Clear();
        }

        /// <summary>
        /// 广播类型事件
        /// </summary>
        public void BroadcastType<T>(T e) where T : BaseTypeEvent
        {
            Assert.NotNullArg(e, "e");

            if (typeEventDict.TryGetValue(typeof(T), out HashSet<object> set))
            {
                foreach (object action in set)
                {
                    action.Convert<TypeEventFunc<T>>()?.Invoke(e);
                }
            }
        }

        public TypeEventModel()
        {
            typeEventDict = new Dictionary<Type, HashSet<object>>();
        }
    }
}
