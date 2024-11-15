using UnityEngine;

namespace Duo1JFramework.DataStructure
{
    /// <summary>
    /// 体素物体
    /// </summary>
    public class VoxelItem : BaseMono
    {
        [Label("体素大小")]
        public float voxelSize = 0.1f;

        [Label("生成Mesh")]
        public Mesh mesh;

        /// <summary>
        /// 体素组
        /// </summary>
        public VoxelGroup Voxel => voxel;

        [HideInInspector]
        [SerializeField]
        private VoxelGroup voxel;

        /// <summary>
        /// 生成体素
        /// </summary>
        public void GenerateVoxel()
        {
            MeshCollider meshCol = GetOrAddCom<MeshCollider>();
            meshCol.sharedMesh = mesh;

            voxel = VoxelUtil.GenerateVoxelGroup(voxelSize, mesh, transform.position, transform.localScale);
        }

        /// <summary>
        /// 清理体素
        /// </summary>
        public void ClearVoxel()
        {
            voxel = null;
        }

        private void OnDrawGizmosSelected()
        {
            if (Voxel != null)
            {
                voxel.DrawGizmos();
            }
        }
    }
}
