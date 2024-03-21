using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 杂项扩展方法
    /// </summary>
    public static class MiscExtend
    {
        #region Color

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

        #endregion Color
    }
}