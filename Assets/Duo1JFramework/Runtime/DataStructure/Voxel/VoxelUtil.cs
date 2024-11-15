using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.DataStructure
{
    /// <summary>
    /// 体素工具类
    /// </summary>
    public class VoxelUtil
    {
        /// <summary>
        /// 通过Mesh生成体素组
        /// </summary>
        public static VoxelGroup GenerateVoxelGroup(float voxelSize, Mesh mesh, Vector3 rootPos, Vector3 scale, int layerMask = Def.Physics.DEFAULT_MASK)
        {
            Assert.NotNullArg(mesh, "mesh");

            Bounds bounds = mesh.CalculateBounds();
            List<Vector3> voxelList = new List<Vector3>();

            Vector3 min = bounds.min - Vector3.one;
            Vector3 max = bounds.max + Vector3.one;
            Vector3 voxelExtend = Vector3.one * (voxelSize * 0.5f);

            for (float x = Mathf.CeilToInt(min.x / voxelSize) * voxelSize * scale.x; x <= max.x * scale.x; x += voxelSize)
            {
                for (float y = Mathf.CeilToInt(min.y / voxelSize) * voxelSize * scale.y; y <= max.y * scale.y; y += voxelSize)
                {
                    for (float z = Mathf.CeilToInt(min.z / voxelSize) * voxelSize * scale.z; z <= max.z * scale.z; z += voxelSize)
                    {
                        Vector3 center = new Vector3(x, y, z);
                        if (Physics.CheckBox(center + rootPos, voxelExtend, Quaternion.identity, layerMask))
                        {
                            voxelList.Add(center);
                        }
                    }
                }
            }

            return new VoxelGroup(voxelSize, voxelList);
        }

        private VoxelUtil()
        {
        }
    }
}
