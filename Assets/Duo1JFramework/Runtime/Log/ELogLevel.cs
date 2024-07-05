using System;

namespace Duo1JFramework
{
    /// <summary>
    /// 日志打印等级
    /// </summary>
    [Flags]
    public enum ELogLevel
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 通知
        /// </summary>
        Info = 1,

        /// <summary>
        /// 警告
        /// </summary>
        Warn = 1 << 1,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 1 << 2,

        /// <summary>
        /// Timeline
        /// </summary>
        Timeline = 1 << 3,

        /// <summary>
        /// 全部
        /// </summary>
        All = 1 << 30
    }
}
