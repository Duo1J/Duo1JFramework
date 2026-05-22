using UnityEngine;

namespace Duo1JFramework.PlatformAPI
{
    /// <summary>
    /// 平台性能信息
    /// </summary>
    public struct PlatformPerformanceInfo
    {
        /// <summary>
        /// 当前帧率
        /// </summary>
        public float FPS;

        /// <summary>
        /// 总保留内存字节数
        /// </summary>
        public long TotalReservedMemoryBytes;

        /// <summary>
        /// 已分配内存字节数
        /// </summary>
        public long AllocatedMemoryBytes;

        /// <summary>
        /// Mono已使用内存字节数
        /// </summary>
        public long MonoUsedMemoryBytes;

        /// <summary>
        /// Mono堆内存字节数
        /// </summary>
        public long MonoHeapMemoryBytes;

        /// <summary>
        /// 电池电量
        /// </summary>
        public float BatteryLevel;

        /// <summary>
        /// 电池状态
        /// </summary>
        public BatteryStatus BatteryStatus;

        /// <summary>
        /// 网络连接状态
        /// </summary>
        public NetworkReachability NetworkReachability;
    }
}
