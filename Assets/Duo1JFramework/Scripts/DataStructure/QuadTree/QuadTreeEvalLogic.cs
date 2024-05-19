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
        public static bool EvalByConeOfVision(IQuadTreeNode node, object param)
        {
            return node.Bounds.CheckBoundsIsInCamera(CameraManager.Instance.EvalCamera);
        }

        /// <summary>
        /// 视锥体检测 (忽略Y轴)
        /// </summary>
        public static bool EvalByConeOfVisionIgnoreY(IQuadTreeNode node, object param)
        {
            return node.Bounds.CheckBoundsIsInCameraIgnoreY(CameraManager.Instance.EvalCamera);
        }

        /// <summary>
        /// 矩形区域检测
        /// </summary>
        /// <param name="param">Bounds区域</param>
        public static bool EvalByRectArea(IQuadTreeNode node, object param)
        {
            Bounds bounds = param.StructConvert<Bounds>();
            Bounds nodeBounds = node.Bounds;

            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            Vector3 nodeMin = nodeBounds.min;
            Vector3 nodeMax = nodeBounds.max;

            return min.x < nodeMax.x && min.z < nodeMax.z && max.x > nodeMin.x && max.z > nodeMin.z;
        }

        /// <summary>
        /// 通过包围盒检查对象是否被节点包含
        /// </summary>
        public static bool CheckByBounds(IQuadTreeNode node, IQuadTreeItem item)
        {
            Vector3 nodeMin = node.Bounds.min;
            Vector3 nodeMax = node.Bounds.max;
            Vector3 itemMin = item.Bounds.min;
            Vector3 itemMax = item.Bounds.max;

            return nodeMin.x < itemMax.x && nodeMin.z < itemMax.z &&
                nodeMax.x > itemMin.x && nodeMax.z > itemMin.z;
        }
    }
}