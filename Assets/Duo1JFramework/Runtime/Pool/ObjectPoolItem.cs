namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// 对象池对象包装
    /// </summary>
    public class ObjectPoolItem<T> where T : class, new()
    {
        /// <summary>
        /// 所属对象池
        /// </summary>
        public ObjectPoolModel<T> Pool { get; private set; }

        /// <summary>
        /// 所属对象池实例
        /// </summary>
        private BaseObjectPool<T> ownerPool;

        /// <summary>
        /// 对象值
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// 是否使用中
        /// </summary>
        public bool Using { get; set; }

        public ObjectPoolItem(ObjectPoolModel<T> pool, T o)
        {
            Pool = pool;
            Value = o;
            Using = true;
        }

        /// <summary>
        /// 设置所属对象池实例
        /// </summary>
        public void SetOwnerPool(BaseObjectPool<T> pool)
        {
            ownerPool = pool;
        }

        /// <summary>
        /// 返回对象池
        /// </summary>
        public void Return()
        {
            if (ownerPool != null)
            {
                ownerPool.Push(Value);
                return;
            }

            Pool?.Push(this);
        }
    }
}
