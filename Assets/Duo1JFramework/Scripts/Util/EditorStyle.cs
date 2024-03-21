using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器样式
    /// </summary>
    public static class EditorStyle
    {
#if UNITY_EDITOR
        public static bool IsProSkin => UnityEditor.EditorGUIUtility.isProSkin;
#else
        public static bool IsProSkin => true;
#endif

        public static void EnableLabelRichText()
        {
            GUI.skin.label.richText = true;
        }

        public static void DisableLabelRichText()
        {
            GUI.skin.label.richText = false;
        }

        #region Color

        //todo hlj 调整

        public static Color NormalBtnC1
        {
            get
            {
                if (IsProSkin)
                    return Color.green;
                else
                    return ColorUtil.Create(152, 198, 164);
            }
        }

        public static Color NormalBtnC2
        {
            get
            {
                if (IsProSkin)
                    return Color.cyan;
                else
                    return ColorUtil.Create(152, 198, 164);
            }
        }

        public static Color WarnBtnC1
        {
            get
            {
                if (IsProSkin)
                    return Color.yellow;
                else
                    return Color.yellow;
            }
        }

        public static Color WarnBtnC2
        {
            get
            {
                if (IsProSkin)
                    return Color.red;
                else
                    return Color.red;
            }
        }

        public static Color WarnTextC1
        {
            get
            {
                if (IsProSkin)
                    return Color.yellow;
                else
                    return Color.yellow;
            }
        }

        public static Color WarnTextC2
        {
            get
            {
                if (IsProSkin)
                    return Color.red;
                else
                    return Color.red;
            }
        }

        #endregion Color
    }
}