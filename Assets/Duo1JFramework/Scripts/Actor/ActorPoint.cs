using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色挂点
    /// </summary>
    public class ActorPoint : MonoBehaviour
    {
        /// <summary>
        /// 根位置
        /// </summary>
        public Transform Root
        {
            get => root;
        }
        [SerializeField]
        [Label("根节点 (可空)")]
        private Transform root;

        /// <summary>
        /// 相机挂点
        /// </summary>
        public Transform CameraPoint
        {
            get
            {
                Assert.NotNull(cameraPoint, "相机挂点为空");
                return cameraPoint;
            }
        }
        [SerializeField]
        [Label("相机挂点")]
        private Transform cameraPoint;

        /// <summary>
        /// 相机挂点原始本地坐标
        /// </summary>
        public Vector3 OriCameraPointLocPos { get; private set; }

        /// <summary>
        /// 自动匹配节点
        /// </summary>
        public void AutoMatch()
        {
            root = transform;
        }

        private void Awake()
        {
            if (root == null) root = transform;

            if (cameraPoint != null)
            {
                OriCameraPointLocPos = cameraPoint.localPosition;
            }
        }
    }
}