namespace Duo1JFramework.Pattern.Pipeline
{
    /// <summary>
    /// 管线状态
    /// </summary>
    public enum EPipelineState
    {
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 0,

        /// <summary>
        /// 成功
        /// </summary>
        Success,

        /// <summary>
        /// 中断
        /// </summary>
        Break
    }
}
