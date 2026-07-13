namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 效果持续策略
    /// </summary>
    public enum EEffectDuration
    {
        /// <summary>
        /// 瞬时
        /// </summary>
        Instant = 0,

        /// <summary>
        /// 持续 (自动到期)
        /// </summary>
        Duration,

        /// <summary>
        /// 无限 (需外部移除)
        /// </summary>
        Infinite,
    }
}
