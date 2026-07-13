namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 属性修改操作
    /// </summary>
    public enum EAttributeModifierOp
    {
        /// <summary>
        /// 加法叠加
        /// </summary>
        Add = 0,

        /// <summary>
        /// 乘法百分比叠加, 0.1 = +10%
        /// </summary>
        Multiply,

        /// <summary>
        /// 直接覆盖
        /// </summary>
        Override,
    }
}
