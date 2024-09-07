using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 颜色相关扩展
    /// </summary>
    public static class ColorExtend
    {
        /// <summary>
        /// 颜色转16进制色值
        /// </summary>
        public static string ToHex(this Color color)
        {
            return ColorUtil.ColorToHex(color);
        }

        /// <summary>
        /// 16进制色值转颜色
        /// </summary>
        public static Color ToColor(this string hex)
        {
            return ColorUtil.HexToColor(hex);
        }

        /// <summary>
        /// 用color标签包裹字符串
        /// </summary>
        public static string WithColor(this string str, Color color)
        {
            return str.WithColor(color.ToHex());
        }

        /// <summary>
        /// 用color标签包裹字符串
        /// </summary>
        public static string WithColor(this string str, string hex)
        {
            return $"<color={hex}>{str}</color>";
        }

        /// <summary>
        /// 设置R值
        /// </summary>
        public static Color R(this Color color, float r)
        {
            color.r = r;
            return color;
        }

        /// <summary>
        /// 设置G值
        /// </summary>
        public static Color G(this Color color, float g)
        {
            color.g = g;
            return color;
        }

        /// <summary>
        /// 设置B值
        /// </summary>
        public static Color B(this Color color, float b)
        {
            color.b = b;
            return color;
        }

        /// <summary>
        /// 设置Alpha值
        /// </summary>
        public static Color A(this Color color, float a)
        {
            color.a = a;
            return color;
        }

        /// <summary>
        /// 设置RGB值
        /// </summary>
        public static Color RGB(this Color color, float r, float g, float b)
        {
            color.r = r;
            color.g = g;
            color.b = b;
            return color;
        }

        /// <summary>
        /// 设置RGBA值
        /// </summary>
        public static Color RGBA(this Color color, float r, float g, float b, float a)
        {
            color.r = r;
            color.g = g;
            color.b = b;
            color.a = a;
            return color;
        }
    }
}
