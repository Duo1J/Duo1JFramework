using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.DataStructure
{
    /// <summary>
    /// 体素组
    /// </summary>
    [Serializable]
    public class VoxelGroup : IGizmosDrawer
    {
        /// <summary>
        /// 体素大小
        /// </summary>
        public float VoxelSize { get; private set; }

        /// <summary>
        /// 体素中心点列表
        /// </summary>
        public List<Vector3> VoxelList
        {
            get
            {
                if (voxelList == null)
                {
                    voxelList = new List<Vector3>();
                }

                return voxelList;
            }
        }

        private List<Vector3> voxelList;

        public VoxelGroup(float voxelSize, List<Vector3> voxelList)
        {
            VoxelSize = voxelSize;
            this.voxelList = voxelList;
        }

        public VoxelGroup(float voxelSize, Vector3[] voxels)
        {
            VoxelSize = voxelSize;
            voxelList = new List<Vector3>(voxels);
        }

        public void DrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (Vector3 voxel in VoxelList)
            {
                Gizmos.DrawWireCube(voxel, Vector3.one * VoxelSize);
            }
        }
    }
}
