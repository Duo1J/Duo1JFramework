using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 效果配置绘制器
    /// </summary>
    public static class EffectListInspector
    {
        private static Vector2 scroll;

        public static void Draw(SkillConfig skillConfig)
        {
            if (skillConfig == null)
            {
                return;
            }

            ED.Scroll(ref scroll, () =>
            {
                for (int i = 0; i < skillConfig.Effects.Count; i++)
                {
                    if (!DrawOne(skillConfig, i))
                    {
                        break;
                    }
                }

                if (GUILayout.Button("+ 添加效果"))
                {
                    skillConfig.Effects.Add(new EffectConfig { Id = "NewEffect" });
                    EditorUtility.SetDirty(skillConfig);
                }
            }, GUILayout.MaxHeight(250));
        }

        /// <summary>
        /// 绘制单个Effect
        /// </summary>
        private static bool DrawOne(SkillConfig skillConfig, int i)
        {
            EffectConfig effectConfig = skillConfig.Effects[i];
            bool removed = false;

            ED.Vertical(() =>
            {
                ED.Horizontal(() =>
                {
                    effectConfig.Id = EditorGUILayout.TextField("Id", effectConfig.Id);
                    if (GUILayout.Button("X", GUILayout.Width(22)))
                    {
                        skillConfig.Effects.RemoveAt(i);
                        EditorUtility.SetDirty(skillConfig);
                        removed = true;
                    }
                });

                if (removed)
                {
                    return;
                }

                effectConfig.DurationPolicy = (EEffectDuration)EditorGUILayout.EnumPopup("持续策略", effectConfig.DurationPolicy);
                if (effectConfig.DurationPolicy == EEffectDuration.Duration)
                {
                    effectConfig.Duration = EditorGUILayout.FloatField("持续", effectConfig.Duration);
                    effectConfig.Period = EditorGUILayout.FloatField("周期", effectConfig.Period);
                }
                effectConfig.MaxStack = Mathf.Max(1, EditorGUILayout.IntField("最大叠层", effectConfig.MaxStack));

                EditorGUILayout.LabelField("修改器", EditorStyles.miniBoldLabel);
                for (int j = 0; j < effectConfig.Modifiers.Count; j++)
                {
                    if (!DrawModifier(skillConfig, effectConfig, j))
                    {
                        break;
                    }
                }
                if (GUILayout.Button("+ 修改器", GUILayout.Height(16)))
                {
                    effectConfig.Modifiers.Add(new EffectModifier());
                    EditorUtility.SetDirty(skillConfig);
                }
            }, GUI.skin.box);

            return !removed;
        }

        /// <summary>
        /// 绘制单个Modifier
        /// </summary>
        private static bool DrawModifier(SkillConfig skillConfig, EffectConfig effectConfig, int j)
        {
            bool removed = false;
            ED.Horizontal(() =>
            {
                EffectModifier m = effectConfig.Modifiers[j];
                m.Attribute = (EAttribute)EditorGUILayout.EnumPopup(m.Attribute, GUILayout.Width(90));
                m.Op = (EAttributeModifierOp)EditorGUILayout.EnumPopup(m.Op, GUILayout.Width(70));
                m.Magnitude = EditorGUILayout.FloatField(m.Magnitude, GUILayout.Width(60));
                m.AsDamage = GUILayout.Toggle(m.AsDamage, "伤害", GUILayout.Width(45));
                if (GUILayout.Button("-", GUILayout.Width(22)))
                {
                    effectConfig.Modifiers.RemoveAt(j);
                    EditorUtility.SetDirty(skillConfig);
                    removed = true;
                }
            });
            return !removed;
        }
    }
}
