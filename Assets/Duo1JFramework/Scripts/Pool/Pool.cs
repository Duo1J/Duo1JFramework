namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// 池
    /// </summary>
    public static class Pool
    {
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
