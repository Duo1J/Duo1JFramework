using System;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 属性修改器
    /// </summary>
    [Serializable]
    public class EffectModifier
    {
        /// <summary>
        /// 目标属性
        /// </summary>
        public EAttribute Attribute;

        /// <summary>
        /// 操作
        /// </summary>
        public EAttributeModifierOp Op = EAttributeModifierOp.Add;

        /// <summary>
        /// 基础数值
        /// </summary>
        public float Magnitude;

        /// <summary>
        /// 是否作为伤害/治疗汇总 (为true时会计入伤害管线)
        /// </summary>
        public bool AsDamage;

        public EffectModifier()
        {
        }

        public EffectModifier(EAttribute attribute, EAttributeModifierOp op, float magnitude, bool asDamage = false)
        {
            Attribute = attribute;
            Op = op;
            Magnitude = magnitude;
            AsDamage = asDamage;
        }
    }
}
