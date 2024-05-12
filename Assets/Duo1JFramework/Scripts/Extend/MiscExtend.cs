using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 杂项扩展方法
    /// </summary>
    public static class MiscExtend
    {
        #region Bounds

        private static int CheckBoundsIsInCamera_ComputeOutCode(Vector4 projectionPos)
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
        /// 检测包围盒是否在相机范围内
        /// </summary>
        public static bool CheckBoundsIsInCamera(this Bounds bound, Camera camera)
        {
            Vector4 worldPos = Vector4.one;
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

                        code &= CheckBoundsIsInCamera_ComputeOutCode(camera.projectionMatrix * camera.worldToCameraMatrix * worldPos);
                    }
                }
            }
            return code == 0;
        }

        #endregion Bounds
    }
}