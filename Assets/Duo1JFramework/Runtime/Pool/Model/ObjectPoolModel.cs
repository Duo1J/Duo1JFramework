using System;
using System.Collections.Generic;

namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// 通用对象池实现
    /// </summary>
    public class ObjectPoolModel<T> where T : class, new()
    {
        /// <summary>
        /// 创建新对象钩子
        /// </summary>
        public Action<ObjectPoolItem<T>> OnCreateNew;

        /// <summary>
        /// 对象池栈
        /// </summary>
        private Stack<ObjectPoolItem<T>> poolStack;

        /// <summary>
        /// 入池
        /// </summary>
        public void Push(ObjectPoolItem<T> item)
        {
            if (item == null)
            {
                return;
            }

            if (!item.Using)
            {
                return;
            }

            OnPushObject(item.Value);
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
                ret = CreateNew();
            }
            else
            {
                ret = poolStack.Pop();
            }
            OnPopObject(ret.Value);
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
        /// 使用一个对象，使用完毕后自动入池
        /// </summary>
        public object Using(Func<ObjectPoolItem<T>, object> action)
        {
            if (action == null)
            {
                return null;
            }
            ObjectPoolItem<T> item = Pop();
            object ret = action(item);
            Push(item);
            return ret;
        }

        /// <summary>
        /// 创建一个新对象出池
        /// </summary>
        public virtual ObjectPoolItem<T> CreateNew()
        {
            ObjectPoolItem<T> newItem = new ObjectPoolItem<T>(this, new T());
            OnCreateNew?.Invoke(newItem);
            newItem.Using = true;
            return newItem;
        }

        /// <summary>
        /// 入池对象处理
        /// </summary>
        public virtual void OnPushObject(T o)
        {
        }

        /// <summary>
        /// 出池对象处理
        /// </summary>
        public virtual T OnPopObject(T o)
        {
            return o;
        }

        public ObjectPoolModel()
        {
            poolStack = new Stack<ObjectPoolItem<T>>();
        }
    }
}