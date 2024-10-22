using System;
using System.Collections.Generic;

namespace Duo1JFramework.DataStructure
{
    /// <summary>
    /// 类型对象集合
    /// </summary>
    public class TypeSet
    {
        /// <summary>
        /// 类型对象字典
        /// </summary>
        private Dictionary<Type, object> typeDict;

        /// <summary>
        /// 添加
        /// </summary>
        public void Add<T>(T obj) where T : class
        {
            Add(typeof(T), obj);
        }

        /// <summary>
        /// 添加
        /// </summary>
        public void Add(Type t, object obj)
        {
            if (typeDict.ContainsKey(t))
            {
                typeDict.Add(t, obj);
            }
            else
            {
                typeDict[t] = obj;
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        public bool Remove<T>() where T : class
        {
            return Remove(typeof(T));
        }

        /// <summary>
        /// 移除
        /// </summary>
        public bool Remove(Type t)
        {
            return typeDict.Remove(t);
        }

        /// <summary>
        /// 是否包含类型
        /// </summary>
        public bool ContainsKey<T>() where T : class
        {
            return ContainsKey(typeof(T));
        }

        /// <summary>
        /// 是否包含类型
        /// </summary>
        public bool ContainsKey(Type t)
        {
            return typeDict.ContainsKey(t);
        }

        /// <summary>
        /// 是否包含值
        /// </summary>
        public bool ContainsValue<T>(T obj)
        {
            return typeDict.ContainsValue(obj);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        public T Get<T>() where T : class
        {
            if (TryGetValue<T>(out T value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// 尝试获取值
        /// </summary>
        public bool TryGetValue<T>(out T value) where T : class
        {
            if (TryGetValue(typeof(T), out object obj))
            {
                value = obj as T;
                return true;
            }

            value = default(T);
            return false;
        }

        /// <summary>
        /// 尝试获取值
        /// </summary>
        public bool TryGetValue(Type t, out object value)
        {
            return typeDict.TryGetValue(t, out value);
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            typeDict.Clear();
        }

        public TypeSet()
        {
            typeDict = new Dictionary<Type, object>();
        }
    }
}
