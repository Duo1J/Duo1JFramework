using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI工具类
    /// </summary>
    public class UIUtil
    {
        /// <summary>
        /// 深度细分三角形
        /// </summary>
        public static void SubdivideTriangleDepth(UIVertex a, UIVertex b, UIVertex c, int depth, List<UIVertex> outList)
        {
            if (depth <= 0)
            {
                outList.AddRange(new UIVertex[] { a, b, c });
                return;
            }

            UIVertex[] list = SubDivideTriangle(a, b, c);

            int d = depth - 1;
            for (int i = 0; i < list.Length; i += 3)
            {
                SubdivideTriangleDepth(list[i], list[i + 1], list[i + 2], d, outList);
            }
        }

        /// <summary>
        /// 细分三角形
        /// </summary>
        public static UIVertex[] SubDivideTriangle(UIVertex a, UIVertex b, UIVertex c)
        {
            UIVertex ab = LerpUIVertex(a, b, 0.5f);
            UIVertex bc = LerpUIVertex(b, c, 0.5f);
            UIVertex ca = LerpUIVertex(c, a, 0.5f);

            return new UIVertex[]
            {
                a, ab, ca,
                ab, bc, ca,
                ab, b, bc,
                ca, bc, c
            };
        }

        /// <summary>
        /// UIVertex插值
        /// </summary>
        public static UIVertex LerpUIVertex(UIVertex a, UIVertex b, float t)
        {
            UIVertex ret = new UIVertex();

            ret.position = Vector3.Lerp(a.position, b.position, t);
            ret.normal = Vector3.Lerp(a.normal, b.normal, t);
            ret.tangent = Vector3.Lerp(a.tangent, b.tangent, t);
            ret.color = Color.Lerp(a.color, b.color, t);

            ret.uv0 = Vector2.Lerp(a.uv0, b.uv0, t);
            ret.uv1 = Vector2.Lerp(a.uv1, b.uv1, t);
            ret.uv2 = Vector2.Lerp(a.uv2, b.uv2, t);
            ret.uv3 = Vector2.Lerp(a.uv3, b.uv3, t);

            return ret;
        }

        private UIUtil()
        {
        }
    }
}
