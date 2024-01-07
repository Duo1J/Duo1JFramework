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
        private Transform cameraPoint;

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
        }
    }
}