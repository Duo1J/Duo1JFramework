using System;
using System.Collections.Generic;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 技能上下文, 序列运行时数据
    /// </summary>
    public class SkillContext
    {
        /// <summary>
        /// 释放者
        /// </summary>
        public CombatUnitController Owner;

        /// <summary>
        /// 目标
        /// </summary>
        public CombatUnitController Target;

        /// <summary>
        /// 归属的Ability, 可为null (预览模式)
        /// </summary>
        public Ability Ability;

        /// <summary>
        /// 技能配置
        /// </summary>
        public SkillConfig Config;

        /// <summary>
        /// 效果查找回调
        /// </summary>
        public Func<string, EffectConfig> EffectResolver;

        /// <summary>
        /// 参数字典, 供片段间共享
        /// </summary>
        public Dictionary<string, object> Params = new Dictionary<string, object>();

        /// <summary>
        /// 查找效果配置
        /// </summary>
        public EffectConfig GetEffectConfig(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            if (Config != null && Config.Effects != null)
            {
                for (int i = 0; i < Config.Effects.Count; i++)
                {
                    if (Config.Effects[i].Id == id) return Config.Effects[i];
                }
            }

            return EffectResolver?.Invoke(id);
        }
    }
}
