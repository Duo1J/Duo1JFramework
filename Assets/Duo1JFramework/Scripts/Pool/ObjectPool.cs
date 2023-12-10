using System;
using System.Collections.Generic;

namespace Duo1JFramework.Pool
{
    /// <summary>
    /// 基础对象池
    /// </summary>
    public class ObjectPool<T> where T : new()
    {
        /// <summary>
        /// 对象池栈
        /// </summary>
        private Stack<ObjectPoolItem<T>> poolStack;

        /// <summary>
        /// 入池
        /// </summary>
        public void Push(ObjectPoolItem<T> item)
        {
            if (!item.Using)
            {
                return;
            }
            poolStack.Push(item);
        }

        /// <summary>
        /// 出池
        /// </summary>
        public ObjectPoolItem<T> Pop()
        {
            ObjectPoolItem<T> ret;
            if (poolStack.Count == 0)
            {
                ret = CreateNew(new T());
            }
            else
            {
                ret = poolStack.Pop();
            }
            InitObject(ret.Value);
            ret.Using = true;
            return ret;
        }

        /// <summary>
        /// 使用一个对象，使用完毕后自动入池
        /// </summary>
        public void Using(Action<ObjectPoolItem<T>> action)
        {
            if (action == null)
            {
                return;
            }
            ObjectPoolItem<T> item = Pop();
            action(item);
            Push(item);
        }

        /// <summary>
        /// 创建一个新对象出池
        /// </summary>
        public virtual ObjectPoolItem<T> CreateNew(T o)
        {
            return new ObjectPoolItem<T>(o);
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        public virtual T InitObject(T o)
        {
            return o;
        }

        public ObjectPool()
        {
            poolStack = new Stack<ObjectPoolItem<T>>();
        }
    }

    /// <summary>
    /// 对象池对象包装
    /// </summary>
    public class ObjectPoolItem<T>
    {
        public T Value { get; set; }
        public bool Using { get; set; }

        public ObjectPoolItem(T o)
        {
            Value = o;
            Using = false;
        }
    }
}