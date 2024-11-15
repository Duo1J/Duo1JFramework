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
        public static VoxelGroup GenerateVoxelGroup(float voxelSize, Mesh mesh, Vector3 rootPos, int layerMask = Def.Physics.DEFAULT_MASK)
        {
            Assert.NotNullArg(mesh, "mesh");

            Bounds bounds = mesh.CalculateBounds();
            List<Vector3> voxelList = new List<Vector3>();

            Vector3 voxelExtend = Vector3.one * (voxelSize * 0.5f);

            for (float x = Mathf.FloorToInt(bounds.min.x / voxelSize) * voxelSize; x <= bounds.max.x; x += voxelSize)
            {
                for (float y = Mathf.FloorToInt(bounds.min.y / voxelSize) * voxelSize; y <= bounds.max.y; y += voxelSize)
                {
                    for (float z = Mathf.FloorToInt(bounds.min.z / voxelSize) * voxelSize; z <= bounds.max.z; z += voxelSize)
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
