using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 技能配置绘制器
    /// </summary>
    [CustomEditor(typeof(SkillConfig))]
    public class SkillConfigInspector : BaseCustomEditor<SkillConfig>
    {
        protected override void DrawInspector()
        {
            EditorGUILayout.Space();
            if (GUILayout.Button("打开技能编辑器", GUILayout.Height(28)))
            {
                SkillEditorWindow.OpenWith(instance);
            }

            EditorGUILayout.Space();
            ED.Horizontal(() =>
            {
                if (GUILayout.Button("导出JSON"))
                {
                    string path = EditorUtility.SaveFilePanel("导出SkillConfig", Application.dataPath, instance.Id + ".json", "json");
                    if (!string.IsNullOrEmpty(path))
                    {
                        string json = JsonUtility.ToJson(instance, true);
                        System.IO.File.WriteAllText(path, json, new System.Text.UTF8Encoding(false));
                    }
                }
                if (GUILayout.Button("导入JSON (覆盖当前)"))
                {
                    string path = EditorUtility.OpenFilePanel("导入SkillConfig", Application.dataPath, "json");
                    if (!string.IsNullOrEmpty(path))
                    {
                        string json = System.IO.File.ReadAllText(path);
                        JsonUtility.FromJsonOverwrite(json, instance);
                        EditorUtility.SetDirty(instance);
                    }
                }
            });
        }

        /// <summary>
        /// 双击SkillConfig资源时打开
        /// </summary>
        [OnOpenAsset(0)]
        public static bool OnOpen(int instanceID, int line)
        {
            Object obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj is SkillConfig skillConfig)
            {
                SkillEditorWindow.OpenWith(skillConfig);
                return true;
            }
            return false;
        }
    }
}
