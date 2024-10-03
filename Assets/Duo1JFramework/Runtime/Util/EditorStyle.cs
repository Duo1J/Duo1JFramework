using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器样式
    /// </summary>
    public class ES
    {
#if UNITY_EDITOR
        public static bool IsProSkin => UnityEditor.EditorGUIUtility.isProSkin;
#else
        public static bool IsProSkin => true;
#endif

        #region RichText

        /// <summary>
        /// 设置所有样式的富文本
        /// </summary>
        public static void SetRichText(bool isOn = true)
        {
            RichTextLabel = isOn;
            RichTextToggle = isOn;
        }

        /// <summary>
        /// Label富文本
        /// </summary>
        public static bool RichTextLabel
        {
            get => GUI.skin.label.richText;
            set => GUI.skin.label.richText = value;
        }

        /// <summary>
        /// Toggle富文本
        /// </summary>
        public static bool RichTextToggle
        {
            get => GUI.skin.toggle.richText;
            set => GUI.skin.toggle.richText = value;
        }

        #endregion RichText

        #region Color

        //////
        /// 按钮需要用浅色后缀L
        //////

        public static Color Green => IsProSkin ? Color.green : ColorUtil.Create(0, 120, 15);
        public static string GreenS => ColorUtil.ColorToHex(Green);
        public static Color GreenL => IsProSkin ? Color.green : ColorUtil.Create(150, 215, 150);
        public static string GreenSL => ColorUtil.ColorToHex(GreenL);

        public static Color Blue => IsProSkin ? ColorUtil.Create(140, 140, 255) : Color.blue;
        public static string BlueS => ColorUtil.ColorToHex(Blue);
        public static Color BlueL => IsProSkin ? Color.blue : ColorUtil.Create(154, 154, 224);
        public static string BlueSL => ColorUtil.ColorToHex(BlueL);

        public static Color Yellow => IsProSkin ? Color.yellow : ColorUtil.Create(138, 138, 0);
        public static string YellowS => ColorUtil.ColorToHex(Yellow);
        public static Color YellowL => IsProSkin ? Color.yellow : ColorUtil.Create(154, 154, 70);
        public static string YellowSL => ColorUtil.ColorToHex(YellowL);

        public static Color Red => Color.red;
        public static string RedS => ColorUtil.ColorToHex(Red);
        public static Color RedL => IsProSkin ? Color.red : ColorUtil.Create(230, 120, 120);
        public static string RedSL => ColorUtil.ColorToHex(RedL);

        #endregion Color

        protected ES()
        {
        }
    }

    public class EditorStyle : ES
    {
        protected EditorStyle()
        {
        }
    }
}
