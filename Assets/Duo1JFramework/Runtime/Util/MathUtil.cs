using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 数学工具类
    /// </summary>
    public static class MathUtil
    {
        /// <summary>
        /// 椭圆映射
        /// </summary>
        public static void CircleMapping(ref float h, ref float v)
        {
            h *= Mathf.Sqrt(1 - 0.5f * Mathf.Pow(v, 2));
            v *= Mathf.Sqrt(1 - 0.5f * Mathf.Pow(h, 2));
        }
    }
}