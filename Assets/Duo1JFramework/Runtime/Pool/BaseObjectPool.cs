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
        /// 对象与池包装对象映射
        /// </summary>
        protected Dictionary<T, ObjectPoolItem<T>> poolItemMap;

        /// <summary>
        /// 池对象总数
        /// </summary>
        public int CountAll => poolItemList.Count;

        /// <summary>
        /// 池中空闲对象数
        /// </summary>
        public int CountInactive => pool.CountInactive;

        /// <summary>
        /// 使用中对象数
        /// </summary>
        public int CountUsing => CountAll - CountInactive;

        /// <summary>
        /// 最大空闲容量，<0表示不限制
        /// </summary>
        public int MaxCapacity
        {
            get => pool.MaxCapacity;
            set => pool.MaxCapacity = value;
        }

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

            if (!poolItem.Using)
            {
                Log.Warn("对象池对象重复入池");
                return;
            }

            if (!IsValidObject(item))
            {
                pool.Destroy(poolItem);
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
            while (!IsValidObject(ret.Value))
            {
                pool.Destroy(ret);
                ret = pool.Pop();
            }

            OnPopObject(ret.Value);
            return ret.Value;
        }

        /// <summary>
        /// 预热对象池
        /// </summary>
        public void Prewarm(int count)
        {
            if (count <= 0)
            {
                return;
            }

            while (CountInactive < count)
            {
                if (MaxCapacity >= 0 && CountInactive >= MaxCapacity)
                {
                    return;
                }

                ObjectPoolItem<T> item = pool.CreateNew();
                OnPushObject(item.Value);
                pool.Push(item);
            }
        }

        /// <summary>
        /// 清空对象池中的空闲对象
        /// </summary>
        public void Clear()
        {
            pool.Clear();
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
            if (item == null)
            {
                return null;
            }

            poolItemMap.TryGetValue(item, out ObjectPoolItem<T> poolItem);
            return poolItem;
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
        /// 对象是否有效
        /// </summary>
        protected virtual bool IsValidObject(T o)
        {
            return o != null;
        }

        /// <summary>
        /// 初始化对象池实现
        /// </summary>
        public virtual void InitPool()
        {
            SetPool(new ObjectPoolModel<T>());
        }

        /// <summary>
        /// 设置对象池实现
        /// </summary>
        protected void SetPool(ObjectPoolModel<T> objectPool)
        {
            Assert.NotNullArg(objectPool, "objectPool");

            pool = objectPool;
            pool.OnCreateNew = OnCreateNew;
            pool.OnDestroyItem = OnDestroyItem;
        }

        private void OnCreateNew(ObjectPoolItem<T> item)
        {
            item.SetOwnerPool(this);
            poolItemList.Add(item);
            poolItemMap[item.Value] = item;
        }

        private void OnDestroyItem(ObjectPoolItem<T> item)
        {
            if (item == null)
            {
                return;
            }

            item.SetOwnerPool(null);
            poolItemList.Remove(item);

            if (!ReferenceEquals(item.Value, null))
            {
                poolItemMap.Remove(item.Value);
            }
        }

        public BaseObjectPool()
        {
            poolItemList = new List<ObjectPoolItem<T>>();
            poolItemMap = new Dictionary<T, ObjectPoolItem<T>>();
            SetPool(new ObjectPoolModel<T>());
        }
    }
}