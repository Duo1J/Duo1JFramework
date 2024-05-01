namespace Duo1JFramework.GamerInput
{
    /// <summary>
    /// 输入限制
    /// </summary>
    public enum eInputLimit
    {
        /// <summary>
        /// 不可输入
        /// </summary>
        None = 0,

        /// <summary>
        /// 按键可输入
        /// </summary>
        Key = 1 << 0,

        /// <summary>
        /// 轴可输入
        /// </summary>
        Axis = 1 << 1,

        /// <summary>
        /// 鼠标轴可输入
        /// </summary>
        MouseAxis = 1 << 2,

        /// <summary>
        /// 全部可输入
        /// </summary>
        All = Key | Axis | MouseAxis
    }
}
