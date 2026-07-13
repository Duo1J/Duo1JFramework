namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 能力状态
    /// </summary>
    public enum EAbilityState
    {
        /// <summary>
        /// 空闲
        /// </summary>
        Idle = 0,

        /// <summary>
        /// 激活中 (施法/生效中)
        /// </summary>
        Active,

        /// <summary>
        /// 冷却
        /// </summary>
        Cooldown,
    }
}
