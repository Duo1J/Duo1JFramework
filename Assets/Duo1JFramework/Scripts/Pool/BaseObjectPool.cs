using System;

namespace Duo1JFramework.Pool
{
    /// <summary>
    /// 对象池单例基类
    /// </summary>
    public class BaseObjectPool<T> : Singleton<BaseObjectPool<T>> where T : new()
    {
        private ObjectPool<T> pool;

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
            InitObject(ret.Value);
            ret.Using = true;
            return ret;
        }

        /// <summary>
        /// 使用一个对象，使用完毕后自动入池
        /// </summary>
        public void Using(Action<ObjectPoolItem<T>> action)
        {
            pool.Using(action);
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

        protected override void OnInit()
        {
            pool = new ObjectPool<T>();
        }
    }
}