using System;

namespace Duo1JFramework
{
    /// <summary>
    /// 数字扩展方法
    /// </summary>
    public static class NumberExtend
    {
        private const int K = 1024;
        private const int K2 = K * K;
        private const int K3 = K * K * K;

        #region Int

        public static float B2KB(this int i)
        {
            return (float)i / K;
        }

        public static float B2MB(this int i)
        {
            return (float)i / K2;
        }

        public static float B2GB(this int i)
        {
            return (float)i / K3;
        }

        public static float KB2MB(this int i)
        {
            return (float)i / K;
        }

        public static float KB2GB(this int i)
        {
            return (float)i / K2;
        }

        public static float MB2GB(this int i)
        {
            return (float)i / K;
        }

        #endregion Int

        #region Long

        public static double B2KB(this long l)
        {
            return (double)l / K;
        }

        public static double B2MB(this long l)
        {
            return (double)l / K2;
        }

        public static double B2GB(this long l)
        {
            return (double)l / K3;
        }

        public static double KB2MB(this long l)
        {
            return (double)l / K;
        }

        public static double KB2GB(this long l)
        {
            return (double)l / K2;
        }

        public static double MB2GB(this long l)
        {
            return (double)l / K;
        }

        #endregion Long

        #region Float

        public static float Limit(this float f, int num = 2)
        {
            return (float)Math.Round(f, num);
        }

        #endregion Float

        #region Double

        public static double Limit(this double d, int num = 2)
        {
            return Math.Round(d, num);
        }

        #endregion Double
    }
}
