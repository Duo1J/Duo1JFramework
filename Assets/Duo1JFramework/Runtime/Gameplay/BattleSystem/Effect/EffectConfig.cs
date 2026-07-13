using System;
using System.Collections.Generic;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 战斗效果配置
    /// </summary>
    [Serializable]
    public class EffectConfig
    {
        /// <summary>
        /// 效果Id
        /// </summary>
        public string Id;

        /// <summary>
        /// 显示名
        /// </summary>
        public string Name;

        /// <summary>
        /// 持续策略
        /// </summary>
        public EEffectDuration DurationPolicy = EEffectDuration.Instant;

        /// <summary>
        /// 持续时间 (EEffectDuration.Duration模式使用)
        /// </summary>
        public float Duration = 1f;

        /// <summary>
        /// 周期性触发间隔 (0表示不周期触发, 仅进入和退出触发)
        /// </summary>
        public float Period = 0f;

        /// <summary>
        /// 修改器列表
        /// </summary>
        public List<EffectModifier> Modifiers = new List<EffectModifier>();

        /// <summary>
        /// 附加到目标的标签
        /// </summary>
        public List<AbilityTag> GrantedTags = new List<AbilityTag>();

        /// <summary>
        /// 目标含以下任一标签时会拒绝
        /// </summary>
        public List<AbilityTag> BlockedByTags = new List<AbilityTag>();

        /// <summary>
        /// 目标需要包含以下所有标签才生效
        /// </summary>
        public List<AbilityTag> RequiredTags = new List<AbilityTag>();

        /// <summary>
        /// 叠层上限 (1为不叠加)
        /// </summary>
        public int MaxStack = 1;

        /// <summary>
        /// 创建瞬时效果
        /// </summary>
        public static EffectConfig CreateInstant(string id, params EffectModifier[] mods)
        {
            EffectConfig effectConfig = new EffectConfig
            {
                Id = id,
                DurationPolicy = EEffectDuration.Instant
            };

            if (mods != null)
            {
                effectConfig.Modifiers.AddRange(mods);
            }

            return effectConfig;
        }

        /// <summary>
        /// 创建持续效果
        /// </summary>
        public static EffectConfig CreateDuration(string id, float duration, float period, params EffectModifier[] mods)
        {
            EffectConfig effectConfig = new EffectConfig
            {
                Id = id,
                DurationPolicy = EEffectDuration.Duration,
                Duration = duration,
                Period = period
            };

            if (mods != null)
            {
                effectConfig.Modifiers.AddRange(mods);
            }

            return effectConfig;
        }
    }
}
