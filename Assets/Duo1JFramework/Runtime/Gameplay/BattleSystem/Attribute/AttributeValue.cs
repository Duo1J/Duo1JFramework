using System;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 单个属性数值 (含基础值与由修改器计算的当前值)
    /// </summary>
    [Serializable]
    public class AttributeValue
    {
        /// <summary>
        /// 属性类型
        /// </summary>
        public EAttribute Type { get; private set; }

        /// <summary>
        /// 基础值
        /// </summary>
        public float BaseValue { get; private set; }

        /// <summary>
        /// 当前值
        /// </summary>
        public float CurrentValue { get; private set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public float MinValue { get; set; } = 0f;

        /// <summary>
        /// 最大值 (float.MaxValue表示无上限)
        /// </summary>
        public float MaxValue { get; set; } = float.MaxValue;

        /// <summary>
        /// 数值变化事件 <old, new>
        /// </summary>
        public event Action<float, float> OnValueChanged;

        public AttributeValue(EAttribute type, float baseValue)
        {
            Type = type;
            BaseValue = baseValue;
            CurrentValue = baseValue;
        }

        /// <summary>
        /// 设置基础值 (会同步刷新当前值, 修改器需外部叠加计算)
        /// </summary>
        public void SetBaseValue(float value)
        {
            BaseValue = value;
        }

        /// <summary>
        /// 设置当前值
        /// </summary>
        public void SetCurrentValue(float value)
        {
            float clamped = value < MinValue ? MinValue : (value > MaxValue ? MaxValue : value);
            if (Math.Abs(clamped - CurrentValue) < 1e-6f)
            {
                return;
            }

            float oldValue = CurrentValue;
            CurrentValue = clamped;
            OnValueChanged?.Invoke(oldValue, CurrentValue);
        }

        /// <summary>
        /// 应用修改运算
        /// </summary>
        public void ApplyModifier(EAttributeModifierOp op, float magnitude)
        {
            switch (op)
            {
                case EAttributeModifierOp.Add:
                    SetCurrentValue(CurrentValue + magnitude);
                    break;
                case EAttributeModifierOp.Multiply:
                    SetCurrentValue(CurrentValue * (1f + magnitude));
                    break;
                case EAttributeModifierOp.Override:
                    SetCurrentValue(magnitude);
                    break;
            }
        }

        public override string ToString()
        {
            return $"<Attr-{Type}: {CurrentValue}/{BaseValue}>";
        }
    }
}
