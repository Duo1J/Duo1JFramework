using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 片段属性绘制器
    /// </summary>
    public static class SegmentInspector
    {
        /// <summary>
        /// 绘制片段
        /// </summary>
        public static void Draw(SequenceSegment seg, SkillConfig ownerSkillConfig)
        {
            if (seg == null)
            {
                return;
            }

            EditorGUILayout.LabelField(seg.GetType().Name, EditorStyles.boldLabel);
            seg.Name = EditorGUILayout.TextField("名称", seg.Name);
            ED.Horizontal(() =>
            {
                seg.StartTime = EditorGUILayout.FloatField("起始", seg.StartTime);
                seg.EndTime = EditorGUILayout.FloatField("结束", seg.EndTime);
            });

            EditorGUILayout.Space();
            FieldInfo[] fields = seg.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo f = fields[i];
                if (f.Name == nameof(SequenceSegment.Name) || f.Name == nameof(SequenceSegment.StartTime) || f.Name == nameof(SequenceSegment.EndTime))
                {
                    continue;
                }
                if (f.IsNotSerialized || f.GetCustomAttribute<NonSerializedAttribute>() != null)
                {
                    continue;
                }

                DrawField(seg, f, ownerSkillConfig);
            }
        }

        private static void DrawField(object host, FieldInfo f, SkillConfig ownerSkillConfig)
        {
            Type ft = f.FieldType;
            object cur = f.GetValue(host);
            string label = ObjectNames.NicifyVariableName(f.Name);

            EditorGUI.BeginChangeCheck();
            object next = cur;

            if (ft == typeof(int))
            {
                next = EditorGUILayout.IntField(label, (int)cur);
            }
            else if (ft == typeof(float))
            {
                next = EditorGUILayout.FloatField(label, (float)cur);
            }
            else if (ft == typeof(bool))
            {
                next = EditorGUILayout.Toggle(label, (bool)cur);
            }
            else if (ft == typeof(string))
            {
                next = EditorGUILayout.TextField(label, (string)cur);
            }
            else if (ft == typeof(Vector3))
            {
                next = EditorGUILayout.Vector3Field(label, (Vector3)cur);
            }
            else if (ft == typeof(Vector2))
            {
                next = EditorGUILayout.Vector2Field(label, (Vector2)cur);
            }
            else if (ft == typeof(Color))
            {
                next = EditorGUILayout.ColorField(label, (Color)cur);
            }
            else if (ft == typeof(LayerMask))
            {
                LayerMask lm = (LayerMask)cur;
                lm.value = EditorGUILayout.MaskField(label, lm.value, UnityEditorInternal.InternalEditorUtility.layers);
                next = lm;
            }
            else if (ft.IsEnum)
            {
                next = EditorGUILayout.EnumPopup(label, (Enum)cur);
            }
            else if (ft == typeof(List<string>))
            {
                DrawStringList(label, (List<string>)cur, ownerSkillConfig);
                return;
            }
            else
            {
                EditorGUILayout.LabelField(label, "<不支持的类型>");
                return;
            }

            if (EditorGUI.EndChangeCheck())
            {
                f.SetValue(host, next);
                if (ownerSkillConfig != null)
                {
                    EditorUtility.SetDirty(ownerSkillConfig);
                }
            }
        }

        private static void DrawStringList(string label, List<string> list, SkillConfig ownerSkillConfig)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            for (int i = 0; i < list.Count; i++)
            {
                if (!DrawStringListItem(label, list, ownerSkillConfig, i))
                {
                    break;
                }
            }
            if (GUILayout.Button("+ 添加"))
            {
                list.Add("");
                if (ownerSkillConfig != null)
                {
                    EditorUtility.SetDirty(ownerSkillConfig);
                }
            }
            EditorGUI.indentLevel--;
        }

        /// <summary>
        /// 绘制单项字符串
        /// </summary>
        private static bool DrawStringListItem(string label, List<string> list, SkillConfig ownerSkillConfig, int i)
        {
            bool removed = false;
            ED.Horizontal(() =>
            {
                if (ownerSkillConfig != null && ownerSkillConfig.Effects != null && ownerSkillConfig.Effects.Count > 0
                    && label.ToLower().Contains("effect"))
                {
                    List<string> ids = new List<string> { "<无>" };
                    for (int j = 0; j < ownerSkillConfig.Effects.Count; j++)
                    {
                        ids.Add(ownerSkillConfig.Effects[j].Id);
                    }
                    int idx = Mathf.Max(0, ids.IndexOf(list[i]));
                    int newIdx = EditorGUILayout.Popup(idx, ids.ToArray());
                    list[i] = newIdx == 0 ? "" : ids[newIdx];
                }
                else
                {
                    list[i] = EditorGUILayout.TextField(list[i]);
                }

                if (GUILayout.Button("-", GUILayout.Width(22)))
                {
                    list.RemoveAt(i);
                    if (ownerSkillConfig != null)
                    {
                        EditorUtility.SetDirty(ownerSkillConfig);
                    }
                    removed = true;
                }
            });
            return !removed;
        }
    }
}
