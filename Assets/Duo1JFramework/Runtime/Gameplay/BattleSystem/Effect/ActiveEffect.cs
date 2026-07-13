namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 生效中的Effect实例
    /// </summary>
    public class ActiveEffect
    {
        /// <summary>
        /// 唯一Id
        /// </summary>
        public long Handle { get; internal set; }

        /// <summary>
        /// 效果配置
        /// </summary>
        public EffectConfig Config { get; }

        /// <summary>
        /// 施加者
        /// </summary>
        public CombatUnitController Source { get; }

        /// <summary>
        /// 目标
        /// </summary>
        public CombatUnitController Target { get; }

        /// <summary>
        /// 到期时间戳(s)
        /// </summary>
        public float ExpireTime { get; internal set; }

        /// <summary>
        /// 下一次周期触发的时间戳(s)
        /// </summary>
        public float NextPeriodTime { get; internal set; }

        /// <summary>
        /// 剩余时间(s)
        /// </summary>
        public float RemainTime
        {
            get
            {
                if (float.IsPositiveInfinity(ExpireTime))
                {
                    return float.PositiveInfinity;
                }
                float remain = ExpireTime - UnityEngine.Time.time;
                return remain > 0f ? remain : 0f;
            }
        }

        /// <summary>
        /// 到下次周期触发的剩余时间(s)
        /// </summary>
        public float PeriodRemain
        {
            get
            {
                float remain = NextPeriodTime - UnityEngine.Time.time;
                return remain > 0f ? remain : 0f;
            }
        }

        /// <summary>
        /// 叠层数
        /// </summary>
        public int StackCount { get; internal set; } = 1;

        /// <summary>
        /// 是否已过期
        /// </summary>
        public bool Expired { get; internal set; }

        public ActiveEffect(EffectConfig effectConfig, CombatUnitController source, CombatUnitController target)
        {
            Config = effectConfig;
            Source = source;
            Target = target;
        }
    }
}
