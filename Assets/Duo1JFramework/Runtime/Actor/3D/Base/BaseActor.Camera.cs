using Duo1JFramework.CameraAPI;
using Duo1JFramework.GamerInput;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色逻辑基类 - Camera
    /// </summary>
    public abstract partial class BaseActor
    {
        /// <summary>
        /// 是否绑定了相机
        /// </summary>
        public bool CameraBinded { get; private set; }

        /// <summary>
        /// 相机跟随点
        /// </summary>
        public virtual Transform CameraFollowPoint => GetCameraPoint();

        /// <summary>
        /// 相机注视点
        /// </summary>
        public virtual Transform CameraLookAtPoint => GetCameraPoint();

        /// <summary>
        /// 更新相机
        /// </summary>
        protected virtual void UpdateCamera()
        {
            float mx = InputManager.GetAxisMX();
            float my = InputManager.GetAxisMY();
            Controller.RotateCameraPoint(mx, my);
            Controller.UpdateCameraPointPos();
        }

        /// <summary>
        /// 获取相机绑定点
        /// </summary>
        public Transform GetCameraPoint()
        {
            if (Controller == null)
            {
                return null;
            }
            return Point.CameraPoint;
        }

        /// <summary>
        /// 绑定相机
        /// </summary>
        public virtual void BindCamera()
        {
            CameraManager.Instance.LookAt = this;
            CameraManager.Instance.Follow = this;
            CameraBinded = true;
        }

        /// <summary>
        /// 解绑相机
        /// </summary>
        public virtual void UnBindCamera()
        {
            CameraBinded = false;
            if (CameraManager.Instance.LookAt == this)
            {
                CameraManager.Instance.LookAt = null;
            }
            if (CameraManager.Instance.Follow == this)
            {
                CameraManager.Instance.Follow = null;
            }
        }
    }
}
