using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 全局效果配置集合, 供技能间共享的Effect配置
    /// </summary>
    [CreateAssetMenu(fileName = "EffectConfigLib", menuName = BattleDef.EffectConfigLibMenuName, order = 3)]
    public class EffectConfigLib : ScriptableObject
    {
        /// <summary>
        /// 效果配置列表
        /// </summary>
        public List<EffectConfig> Effects = new List<EffectConfig>();

        /// <summary>
        /// 查找Id
        /// </summary>
        public EffectConfig Find(string id)
        {
            if (Effects == null || string.IsNullOrEmpty(id))
            {
                return null;
            }

            for (int i = 0; i < Effects.Count; i++)
            {
                if (Effects[i] != null && Effects[i].Id == id)
                {
                    return Effects[i];
                }
            }

            return null;
        }
    }
}
