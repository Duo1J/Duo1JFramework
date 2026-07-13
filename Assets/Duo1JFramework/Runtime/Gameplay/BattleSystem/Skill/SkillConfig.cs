using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 技能配置, 用于编辑器序列化的战斗配置
    /// </summary>
    [CreateAssetMenu(fileName = "SkillConfig", menuName = BattleDef.SkillConfigMenuName, order = 1)]
    public class SkillConfig : ScriptableObject
    {
        /// <summary>
        /// 唯一Id
        /// </summary>
        public string Id;

        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName;

        /// <summary>
        /// 描述
        /// </summary>
        [TextArea]
        public string Description;

        /// <summary>
        /// 图标
        /// </summary>
        public Sprite Icon;

        /// <summary>
        /// 输入槽
        /// </summary>
        public EAbilityInputId InputId = EAbilityInputId.None;

        /// <summary>
        /// 冷却时间
        /// </summary>
        public float Cooldown;

        /// <summary>
        /// 消耗法力
        /// </summary>
        public float CostMana;

        /// <summary>
        /// 激活需要标签 (逗号分隔)
        /// </summary>
        public List<string> RequiredTags = new List<string>();

        /// <summary>
        /// 激活阻止标签
        /// </summary>
        public List<string> BlockedTags = new List<string>();

        /// <summary>
        /// 激活期间给予标签
        /// </summary>
        public List<string> OwnedTags = new List<string>();

        /// <summary>
        /// 技能序列
        /// </summary>
        public SkillSequence Sequence = new SkillSequence();

        /// <summary>
        /// 附属效果配置
        /// </summary>
        public List<EffectConfig> Effects = new List<EffectConfig>();
    }
}
