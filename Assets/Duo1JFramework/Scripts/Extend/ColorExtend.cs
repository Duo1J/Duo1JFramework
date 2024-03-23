using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// ÑÕÉ«À©Õ¹
    /// </summary>
    public static class ColorExtend
    {
        public static string ToHex(this Color color)
        {
            return ColorUtil.ColorToHex(color);
        }

        public static Color ToColor(this string hex)
        {
            return ColorUtil.HexToColor(hex);
        }

        public static string WithColor(this string str, Color color)
        {
            return str.WithColor(color.ToHex());
        }

        public static string WithColor(this string str, string hex)
        {
            return $"<color={hex}>{str}</color>";
        }

        public static Color R(this Color color, float r)
        {
            color.r = r;
            return color;
        }

        public static Color G(this Color color, float g)
        {
            color.g = g;
            return color;
        }

        public static Color B(this Color color, float b)
        {
            color.b = b;
            return color;
        }

        public static Color Alpha(this Color color, float a)
        {
            color.a = a;
            return color;
        }

        public static Color RGB(this Color color, float r, float g, float b)
        {
            color.r = r;
            color.g = g;
            color.b = b;
            return color;
        }

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
