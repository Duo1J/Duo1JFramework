namespace Duo1JFramework.PlatformAPI
{
    /// <summary>
    /// 平台权限状态枚举
    /// </summary>
    public enum EPlatformPermissionStatus
    {
        /// <summary>
        /// 未知状态。
        /// </summary>
        Unknown,

        /// <summary>
        /// 已授权。
        /// </summary>
        Granted,

        /// <summary>
        /// 已拒绝。
        /// </summary>
        Denied,

        /// <summary>
        /// 不支持该权限。
        /// </summary>
        NotSupported
    }
}
