using System;
using System.Collections.Generic;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 属性集
    /// </summary>
    public class AttributeSet
    {
        private readonly Dictionary<EAttribute, AttributeValue> attrs = new Dictionary<EAttribute, AttributeValue>();

        /// <summary>
        /// 属性变化事件 <attr, old, new>
        /// </summary>
        public event Action<EAttribute, float, float> OnAttributeChanged;

        /// <summary>
        /// 注册属性
        /// </summary>
        public AttributeValue Register(EAttribute type, float baseValue, float min = 0f, float max = float.MaxValue)
        {
            if (attrs.TryGetValue(type, out AttributeValue exist))
            {
                exist.SetBaseValue(baseValue);
                exist.SetCurrentValue(baseValue);
                exist.MinValue = min;
                exist.MaxValue = max;
                return exist;
            }

            AttributeValue val = new AttributeValue(type, baseValue)
            {
                MinValue = min,
                MaxValue = max
            };
            val.OnValueChanged += (o, n) => OnAttributeChanged?.Invoke(type, o, n);
            attrs.Add(type, val);

            return val;
        }

        /// <summary>
        /// 是否存在属性
        /// </summary>
        public bool Has(EAttribute type)
        {
            return attrs.ContainsKey(type);
        }

        /// <summary>
        /// 获取属性 (未注册返回null)
        /// </summary>
        public AttributeValue Get(EAttribute type)
        {
            attrs.TryGetValue(type, out AttributeValue val);
            return val;
        }

        /// <summary>
        /// 获取属性当前值 (未注册返回默认值)
        /// </summary>
        public float GetValue(EAttribute type, float defaultVal = 0f)
        {
            return attrs.TryGetValue(type, out AttributeValue val) ? val.CurrentValue : defaultVal;
        }

        /// <summary>
        /// 修改属性
        /// </summary>
        public void Modify(EAttribute type, EAttributeModifierOp op, float magnitude)
        {
            if (!attrs.TryGetValue(type, out AttributeValue val))
            {
                Log.Warn($"[AttributeSet] 未注册属性 `{type}`，忽略修改");
                return;
            }

            val.ApplyModifier(op, magnitude);
        }

        /// <summary>
        /// 遍历
        /// </summary>
        public Dictionary<EAttribute, AttributeValue>.Enumerator GetEnumerator()
        {
            return attrs.GetEnumerator();
        }
    }
}
