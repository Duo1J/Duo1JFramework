using Duo1JFramework.CameraAPI;
using UnityEngine;

namespace Duo1JFramework.DataStructure
{
    /// <summary>
    /// 四叉树评估检测算法
    /// </summary>
    public static class QuadTreeEvalLogic
    {
        /// <summary>
        /// 视锥体检测
        /// </summary>
        public static bool EvalByConeOfVision(QuadTreeNode node, object param)
        {
            return node.Bounds.CheckBoundsIsInCamera(CameraManager.Instance.EvalCamera);
        }

        /// <summary>
        /// 矩形区域检测
        /// </summary>
        /// <param name="param">Bounds区域</param>
        public static bool EvalByRectArea(QuadTreeNode node, object param)
        {
            Bounds bounds = param.StructConvert<Bounds>();
            Bounds nodeBounds = node.Bounds;

            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            Vector3 nodeMin = nodeBounds.min;
            Vector3 nodeMax = nodeBounds.max;

            return min.x < nodeMax.x && min.z < nodeMax.z && max.x > nodeMin.x && max.z > nodeMin.z;
        }
    }
}