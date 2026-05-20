namespace Duo1JFramework.PlatformAPI
{
    /// <summary>
    /// 平台接口
    /// </summary>
    public interface IPlatform
    {
        /// <summary>
        /// 平台类型枚举
        /// </summary>
        EPlatform Type { get; }

        /// <summary>
        /// 是否编辑器平台
        /// </summary>
        bool IsEditor { get; }

        /// <summary>
        /// 是否PC平台
        /// </summary>
        bool IsPC { get; }

        /// <summary>
        /// 是否移动平台
        /// </summary>
        bool IsMobile { get; }

        #region Memory

        /// <summary>
        /// 获取总内存 (MB)
        /// </summary>
        int GetTotalMemory();

        /// <summary>
        /// 获取总保留内存 (B)
        /// </summary>
        long GetTotalReservedMemory();

        /// <summary>
        /// 获取已使用堆内存 (B)
        /// </summary>
        long GetUsedHeapSize();

        /// <summary>
        /// 获取总堆内存 (B)
        /// </summary>
        long GetTotalHeapSize();

        #endregion Memory
    }
}
