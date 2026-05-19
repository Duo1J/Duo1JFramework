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
        /// 返回对象池
        /// </summary>
        public void Return()
        {
            Pool?.Push(this);
        }
    }
}
