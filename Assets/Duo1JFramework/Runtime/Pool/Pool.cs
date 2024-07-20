using System;

namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// 池
    /// </summary>
    public static class Pool
    {
        /// <summary>
        /// 创建通用对象池
        /// </summary>
        public static CommonPool<T> Create<T>(Func<T, T> initCall) where T : class, new()
        {
            return new CommonPool<T>(initCall);
        }

        /// <summary>
        /// StringBuilder池
        /// </summary>
        public static StringBuilderPool StringBuilderPool = new StringBuilderPool();

        /// <summary>
        /// Timer池
        /// </summary>
        public static TimerPool TimerPool = new TimerPool();
    }
}
