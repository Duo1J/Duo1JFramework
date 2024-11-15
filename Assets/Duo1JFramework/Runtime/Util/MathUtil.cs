using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 数学工具类
    /// </summary>
    public class MathUtil
    {
        /// <summary>
        /// 椭圆映射
        /// </summary>
        public static void CircleMapping(ref float h, ref float v)
        {
            h *= Mathf.Sqrt(1 - 0.5f * Mathf.Pow(v, 2));
            v *= Mathf.Sqrt(1 - 0.5f * Mathf.Pow(h, 2));
        }

        /// <summary>
        /// 计算多个点的包围盒
        /// </summary>
        public static Bounds CalculateBounds(params Vector3[] vectors)
        {
            Bounds bounds = new Bounds();

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float minZ = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            float maxZ = float.MinValue;

            foreach (Vector3 vec in vectors)
            {
                minX = Mathf.Min(minX, vec.x);
                minY = Mathf.Min(minY, vec.y);
                minZ = Mathf.Min(minZ, vec.z);
                maxX = Mathf.Max(maxX, vec.x);
                maxY = Mathf.Max(maxY, vec.y);
                maxZ = Mathf.Max(maxZ, vec.z);
            }

            Vector3 min = new Vector3(minX, minY, minZ);
            Vector3 max = new Vector3(maxX, maxY, maxZ);

            bounds.size = max - min;
            bounds.center = (min + max) / 2;

            return bounds;
        }

        private MathUtil()
        {
        }
    }
}
