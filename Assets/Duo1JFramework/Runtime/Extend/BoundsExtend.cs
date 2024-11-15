using System;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 包围盒扩展
    /// </summary>
    public static class BoundsExtend
    {
        /// <summary>
        /// 计算Mesh的包围盒
        /// </summary>
        public static Bounds CalculateBounds(this Mesh mesh)
        {
            return MathUtil.CalculateBounds(mesh.vertices);
        }

        /// <summary>
        /// 计算多个点的包围盒
        /// </summary>
        public static Bounds CalculateBounds(Vector3 vec, params Vector3[] vectors)
        {
            Vector3[] arr = new Vector3[vectors.Length + 1];
            arr[0] = vec;
            Array.Copy(vectors, 0, arr, 1, vectors.Length);

            return MathUtil.CalculateBounds(arr);
        }

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

            return false;
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

            return false;
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
    }
}
