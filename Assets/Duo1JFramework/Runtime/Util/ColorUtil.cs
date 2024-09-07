using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 颜色工具类
    /// </summary>
    public static class ColorUtil
    {
        private static Dictionary<string, Color> hexColorCache = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);

        private static Dictionary<Color32, string> colorHexCache = new Dictionary<Color32, string>();

        /// <summary>
        /// 16进制色值转Color
        /// </summary>
        public static Color HexToColor(string hex)
        {
            if (hexColorCache.TryGetValue(hex, out Color ret))
            {
                return ret;
            }
            try
            {
                if (hex.Length == 7)
                {
                    hex = hex.Substring(1, 6);
                }
                if (hex.Length != 6)
                {
                    throw CommonException.Create($"Hex color length error: {hex}");
                }
                byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                ret = new Color32(r, g, b, 255);
                return hexColorCache[hex] = ret;
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, $"HexToColor转换失败: {hex}");
                return Color.white;
            }
        }

        /// <summary>
        /// Color转16进制色值
        /// </summary>
        public static string ColorToHex(Color32 color)
        {
            if (colorHexCache.TryGetValue(color, out string ret))
            {
                return ret;
            }
            ret = ("#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2")).ToUpper();
            return colorHexCache[color] = ret;
        }

        /// <summary>
        /// 以0~255数值创建颜色
        /// </summary>
        public static Color Create(int r, int g, int b, int a = 255)
        {
            return Create(r / 255f, g / 255f, b / 255f, a / 255f);
        }

        /// <summary>
        /// 以0~1数值创建颜色
        /// </summary>
        public static Color Create(float r, float g, float b, float a = 1)
        {
            return new Color(r, g, b, a);
        }

        /// <summary>
        /// 以16进制色值创建颜色
        /// </summary>
        public static Color Create(string hexColor)
        {
            return HexToColor(hexColor);
        }
    }
}