using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器颜色查看器
    /// </summary>
    public class EditorColorViewer : BaseEditorWindow<EditorColorViewer>
    {
        private bool boxBg = false;

        private Color color = new Color(0, 0, 0, 1);
        private int size = 16;

        private void OnGUI()
        {
            ES.SetRichText();

            if (boxBg)
            {
                GUILayout.BeginVertical("box");
            }
            else
            {
                GUILayout.BeginVertical();
            }

            ED.Horizontal(() => { boxBg = GUILayout.Toggle(boxBg, "是否打开背景"); });

            ED.Space(5, () =>
            {
                ED.Horizontal(() =>
                {
                    GUILayout.Label("字体大小");
                    size = EditorGUILayout.IntSlider(size, 6, 70);
                });
            });

            ED.Space(5, () =>
            {
                ED.Horizontal(() =>
                {
                    GUILayout.Label("颜色");
                    color = EditorGUILayout.ColorField(color);
                });
            });

            string colorHex = "#" + ColorUtility.ToHtmlStringRGB(color);
            ED.Horizontal(() =>
            {
                GUILayout.Label(colorHex);
                if (GUILayout.Button("复制"))
                {
                    EditorUtil.CopyText(colorHex);
                }
            });

            ED.Space(30, () =>
            {
                GUILayout.Label($"<size={size}><color={colorHex}>测试 - Test</color></size>");
            });

            ED.Color(colorHex.ToColor(), () =>
            {
                GUILayout.Button("测试 - Test");
            });

            GUILayout.EndVertical();
        }
    }
}
