using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器样式
    /// </summary>
    public static class EditorStyle
    {
        public static bool IsProSkin => EditorGUIUtility.isProSkin;

        #region Color

        public static Color NormalBtnColor1
        {
            get
            {
                if (IsProSkin)
                    return Color.green;
                else
                    return ColorUtil.Create(152, 198, 164);
            }
        }

        public static Color WarnBtnColor1
        {
            get
            {
                if (IsProSkin)
                    return Color.red;
                else
                    return Color.red;
            }
        }

        public static Color WarnBtnColor2
        {
            get
            {
                if (IsProSkin)
                    return Color.yellow;
                else
                    return Color.yellow;
            }
        }

        #endregion Color
    }
}