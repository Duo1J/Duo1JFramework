using System;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// Gizmos工具类
    /// </summary>
    public static class GizmosUtil
    {
        public static void SurrondColor(Color color, Action action)
        {
            Color oriColor = Gizmos.color;
            Gizmos.color = color;
            action?.InvokeSafe();
            Gizmos.color = oriColor;
        }

        public static void SurrondColor(bool con, Color color, Action action)
        {
            if (con)
            {
                SurrondColor(color, action);
            }
            else
            {
                action?.InvokeSafe();
            }
        }

        /// <summary>
        /// 绘制线框胶囊
        /// </summary>
        public static void DrawWireCapsule(Vector3 point1, Vector3 point2, float radius)
        {
            Gizmos.DrawWireSphere(point1, radius);
            Gizmos.DrawWireSphere(point2, radius);
            Gizmos.DrawLine(point1, point2);
        }
    }
}