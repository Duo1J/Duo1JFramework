namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// 池
    /// </summary>
    public static class Pool
    {
        /// <summary>
        /// 创建通用对象池
        /// 需要在Pop后自行初始化
        /// </summary>
        public static CommonPool<T> Create<T>() where T : new()
        {
            return new CommonPool<T>();
        }

        /// <summary>
        /// StringBuilder池
        /// </summary>
        public static StringBuilderPool StringBuilderPool = new StringBuilderPool();

        /// <summary>
        /// Timer池
        /// </summary>
        public static TimerPool TimerPool = new TimerPool();

        /// <summary>
        /// 响应式监听者池
        /// </summary>
        public static RxObserverPool RxObserverPool = new RxObserverPool();
    }
}
