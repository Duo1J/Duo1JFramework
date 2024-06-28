using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// GUI颜色调整器
    /// </summary>
    public class GUIColorViewer : BaseEditorWindow<GUIColorViewer>
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

            LU.Horizontal(() => { boxBg = GUILayout.Toggle(boxBg, "是否打开背景"); });

            LU.SurrondSpace(5, () =>
            {
                LU.Horizontal(() =>
                {
                    GUILayout.Label("字体大小");
                    size = EditorGUILayout.IntSlider(size, 6, 70);
                });
            });

            LU.SurrondSpace(5, () =>
            {
                LU.Horizontal(() =>
                {
                    GUILayout.Label("颜色");
                    color = EditorGUILayout.ColorField(color);
                });
            });

            string colorHex = "#" + ColorUtility.ToHtmlStringRGB(color);
            LU.Horizontal(() =>
            {
                GUILayout.Label(colorHex);
                if (GUILayout.Button("复制"))
                {
                    EditorUtil.CopyText(colorHex);
                }
            });

            LU.SurrondSpace(30, () =>
            {
                GUILayout.Label($"<size={size}><color={colorHex}>测试 - Test</color></size>");
            });

            LU.SurrondColor(colorHex.ToColor(), () =>
            {
                GUILayout.Button("测试 - Test");
            });

            GUILayout.EndVertical();
        }
    }
}