using System;

namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// 对象池单例基类
    /// </summary>
    public abstract class BaseObjectPool<T> where T : new()
    {
        private ObjectPool<T> pool = new ObjectPool<T>();

        /// <summary>
        /// 入池
        /// </summary>
        public void Push(ObjectPoolItem<T> item)
        {
            pool.Push(item);
        }

        /// <summary>
        /// 出池
        /// </summary>
        public ObjectPoolItem<T> Pop()
        {
            ObjectPoolItem<T> ret = pool.Pop();
            ret.Value = InitObject(ret.Value);
            ret.Using = true;
            return ret;
        }

        /// <summary>
        /// 使用一个对象，使用完毕后自动入池
        /// </summary>
        public void Using(Action<ObjectPoolItem<T>> action)
        {
            pool.Using((item) =>
            {
                item.Value = InitObject(item.Value);
                action(item);
                item.Using = true;
            });
        }

        /// <summary>
        /// 创建一个新对象出池
        /// </summary>
        public virtual ObjectPoolItem<T> CreateNew(T o)
        {
            return pool.CreateNew(o);
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        public virtual T InitObject(T o)
        {
            return o;
        }
    }
}