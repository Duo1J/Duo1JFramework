using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 技能配置集合, 提供全局按Id查找SkillConfig
    /// </summary>
    [CreateAssetMenu(fileName = "SkillConfigLib", menuName = BattleDef.SkillConfigLibMenuName, order = 2)]
    public class SkillConfigLib : ScriptableObject
    {
        /// <summary>
        /// 技能配置列表
        /// </summary>
        public List<SkillConfig> Skills = new List<SkillConfig>();

        /// <summary>
        /// 查找Id
        /// </summary>
        public SkillConfig Find(string id)
        {
            if (Skills == null || string.IsNullOrEmpty(id))
            {
                return null;
            }

            for (int i = 0; i < Skills.Count; i++)
            {
                if (Skills[i] != null && Skills[i].Id == id)
                {
                    return Skills[i];
                }
            }

            return null;
        }
    }
}
