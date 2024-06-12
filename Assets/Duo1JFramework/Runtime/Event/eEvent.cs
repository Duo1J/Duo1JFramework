namespace Duo1JFramework.Event
{
    /// <summary>
    /// 事件枚举
    /// </summary>
    public enum eEvent
    {
        #region APP

        /// <summary>
        /// 程序退出
        /// </summary>
        APP_QUIT,

        /// <summary>
        /// 程序聚焦
        /// </summary>
        APP_FOCUS,

        /// <summary>
        /// 程序失焦
        /// </summary>
        APP_UNFOCUS,

        /// <summary>
        /// 程序暂停
        /// </summary>
        APP_PAUSE,

        /// <summary>
        /// 程序继续
        /// </summary>
        APP_RESUME,

        #endregion APP

        #region Audio

        /// <summary>
        /// 停止所有单次音频播放
        /// </summary>
        AUDIO_STOP_ALL_ONE_SHOT,

        /// <summary>
        /// 停止所有持续音频播放
        /// </summary>
        AUDIO_STOP_ALL_KEEP,

        #endregion Audio

        /// <summary>
        /// 枚举结束值
        /// </summary>
        END
    }
}
