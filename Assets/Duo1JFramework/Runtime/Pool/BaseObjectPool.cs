using System;
using System.Collections.Generic;

namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// 对象池实例基类
    /// </summary>
    public abstract class BaseObjectPool<T> where T : class, new()
    {
        /// <summary>
        /// 对象池实现
        /// </summary>
        protected ObjectPoolModel<T> pool;

        /// <summary>
        /// 池包装对象列表
        /// </summary>
        protected List<ObjectPoolItem<T>> poolItemList;

        /// <summary>
        /// 入池
        /// </summary>
        public void Push(T item)
        {
            ObjectPoolItem<T> poolItem = GetPoolItemInList(item);
            if (poolItem == null)
            {
                return;
            }

            OnPushObject(item);
            pool.Push(poolItem);
        }

        /// <summary>
        /// 出池
        /// </summary>
        public T Pop()
        {
            ObjectPoolItem<T> ret = pool.Pop();
            OnPopObject(ret.Value);
            return ret.Value;
        }

        /// <summary>
        /// 使用一个对象，使用完毕后自动入池
        /// </summary>
        public void Using(Action<T> action)
        {
            if (action == null)
            {
                return;
            }

            T item = Pop();
            try
            {
                action(item);
            }
            finally
            {
                Push(item);
            }
        }

        /// <summary>
        /// 使用一个对象，使用完毕后自动入池
        /// </summary>
        public object Using(Func<T, object> action)
        {
            if (action == null)
            {
                return null;
            }

            T item = Pop();
            try
            {
                return action(item);
            }
            finally
            {
                Push(item);
            }
        }

        public virtual ObjectPoolItem<T> GetPoolItemInList(T item)
        {
            foreach (ObjectPoolItem<T> poolItem in poolItemList)
            {
                if (poolItem.Value == item)
                {
                    return poolItem;
                }
            }

            return null;
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
        public virtual void OnPopObject(T o)
        {
        }

        /// <summary>
        /// 初始化对象池实现
        /// </summary>
        public virtual void InitPool()
        {
            pool = new ObjectPoolModel<T>();
        }

        private void OnCreateNew(ObjectPoolItem<T> item)
        {
            poolItemList.Add(item);
        }

        public BaseObjectPool()
        {
            InitPool();
            pool.OnCreateNew = OnCreateNew;
            poolItemList = new List<ObjectPoolItem<T>>();
        }
    }
}