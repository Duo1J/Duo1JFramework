using Cinemachine;
using Duo1JFramework.PhysicsAPI;
using UnityEngine;

namespace Duo1JFramework.CameraAPI
{
    /// <summary>
    /// 触发切换相机
    /// </summary>
    [RequireComponent(typeof(CollisionController))]
    public class TriggerChgCMCamera : BaseMono
    {
        public CinemachineVirtualCamera virtualCamera;
        private CMCamera cmCamera;

        private CollisionController collisionController;

        private void Awake()
        {
            if (virtualCamera == null)
            {
                Log.ErrorForce($"{ToString()} 未设置虚拟相机");
                SetEnabled(false);
                return;
            }
            cmCamera = CMBrain.Instance.CreateCamera(virtualCamera);

            collisionController = GetComponent<CollisionController>();
            collisionController.SetCollisionType(ECollisionType.Trigger);

            collisionController.TriggerEnter = TriggerEnterHandle;
            collisionController.TriggerExit = TriggerExitHandle;

        }

        protected void TriggerEnterHandle(CollisionController controller, Collider collider)
        {
            CMBrain.Instance.SetLiveCamera(cmCamera);
        }

        protected void TriggerExitHandle(CollisionController controller, Collider collider)
        {
            cmCamera.ResetTempPriority();
            CMBrain.Instance.SetLiveCameraByPriority();
        }
    }
}
