namespace Duo1JFramework.PlatformAPI
{
    /// <summary>
    /// 平台设备信息
    /// </summary>
    public struct PlatformDeviceInfo
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName;

        /// <summary>
        /// 设备型号
        /// </summary>
        public string DeviceModel;

        /// <summary>
        /// 设备唯一标识
        /// </summary>
        public string DeviceUniqueIdentifier;

        /// <summary>
        /// 操作系统信息
        /// </summary>
        public string OperatingSystem;

        /// <summary>
        /// 处理器类型
        /// </summary>
        public string ProcessorType;

        /// <summary>
        /// 处理器核心数
        /// </summary>
        public int ProcessorCount;

        /// <summary>
        /// 显卡设备名称
        /// </summary>
        public string GraphicsDeviceName;

        /// <summary>
        /// 显存大小MB
        /// </summary>
        public int GraphicsMemorySize;

        /// <summary>
        /// 系统语言
        /// </summary>
        public string SystemLanguage;
    }
}
