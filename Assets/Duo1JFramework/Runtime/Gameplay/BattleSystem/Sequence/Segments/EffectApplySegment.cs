using System;
using System.Collections.Generic;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 对Owner自身或指定目标应用Effect
    /// </summary>
    [Serializable]
    public class EffectApplySegment : SequenceSegment
    {
        /// <summary>
        /// 应用目标
        /// </summary>
        public enum EApplyTarget
        {
            Self,
            Caster,
            Target,
        }

        /// <summary>
        /// 目标类型
        /// </summary>
        public EApplyTarget TargetType = EApplyTarget.Self;

        /// <summary>
        /// 效果Id列表
        /// </summary>
        public List<string> EffectIds = new List<string>();

        public override void OnEnter(SkillContext ctx)
        {
            if (ctx == null || EffectIds == null)
            {
                return;
            }

            CombatUnitController target;
            switch (TargetType)
            {
                case EApplyTarget.Caster:
                case EApplyTarget.Self:
                    target = ctx.Owner;
                    break;
                case EApplyTarget.Target:
                    target = ctx.Target;
                    break;
                default:
                    target = ctx.Owner;
                    break;
            }

            if (target == null)
            {
                return;
            }

            for (int i = 0; i < EffectIds.Count; i++)
            {
                EffectConfig effectConfig = ctx.GetEffectConfig(EffectIds[i]);
                if (effectConfig == null) continue;
                target.Effects.Apply(effectConfig, ctx.Owner);
            }
        }
    }
}