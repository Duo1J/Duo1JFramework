using System;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 杂项扩展
    /// </summary>
    public static class MiscExtend
    {
        #region Enum

        /// <summary>
        /// 获取枚举名
        /// </summary>
        public static string GetName(this Enum e)
        {
            return EnumUtil.GetName(e);
        }

        #endregion Enum

        #region Delegate

        /// <summary>
        /// 委托安全调用
        /// </summary>
        public static void InvokeSafe(this Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e);
            }
        }

        #endregion Delegate

        #region Bounds

        /// <summary>
        /// 检测包围盒是否在相机范围内
        /// </summary>
        public static bool IsInCamera(this Bounds bound, Camera camera)
        {
            Vector4 worldPos = Vector4.one;
            //111111
            int code = 63;
            for (int i = -1; i <= 1; i += 2)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    for (int k = -1; k <= 1; k += 2)
                    {
                        worldPos.x = bound.center.x + i * bound.extents.x;
                        worldPos.y = bound.center.y + j * bound.extents.y;
                        worldPos.z = bound.center.z + k * bound.extents.z;
                        code &= IsInCamera_ComputeOutCode(camera.projectionMatrix * camera.worldToCameraMatrix * worldPos);

                        if (code == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return code == 0;
        }

        private static int IsInCamera_ComputeOutCode(Vector4 projectionPos)
        {
            int code = 0;
            if (projectionPos.x < -projectionPos.w) code |= 1;
            if (projectionPos.x > projectionPos.w) code |= 2;
            if (projectionPos.y < -projectionPos.w) code |= 4;
            if (projectionPos.y > projectionPos.w) code |= 8;
            if (projectionPos.z < -projectionPos.w) code |= 16;
            if (projectionPos.z > projectionPos.w) code |= 32;
            return code;
        }

        /// <summary>
        /// 检测包围盒是否在相机范围内 (忽略Y轴)
        /// </summary>
        public static bool IsInCameraIgnoreY(this Bounds bound, Camera camera)
        {
            Vector4 worldPos = Vector4.one;
            //1111
            int code = 15;
            for (int i = -1; i <= 1; i += 2)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    worldPos.x = bound.center.x + i * bound.extents.x;
                    worldPos.z = bound.center.z + j * bound.extents.z;
                    code &= IsInCameraIgnoreY_ComputeOutCode(camera.projectionMatrix * camera.worldToCameraMatrix * worldPos);

                    if (code == 0)
                    {
                        return true;
                    }
                }
            }
            return code == 0;
        }

        private static int IsInCameraIgnoreY_ComputeOutCode(Vector4 projectionPos)
        {
            int code = 0;
            if (projectionPos.x < -projectionPos.w) code |= 1;
            if (projectionPos.x > projectionPos.w) code |= 2;
            if (projectionPos.z < -projectionPos.w) code |= 4;
            if (projectionPos.z > projectionPos.w) code |= 8;
            return code;
        }

        /// <summary>
        /// 使用相机平头锥体平面检测包围盒是否在相机范围内(完全包含)
        /// </summary>
        public static bool IsInCameraByFrustum(Bounds bound, Camera camera)
        {
            return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), bound);
        }

        #endregion Bounds
    }
}