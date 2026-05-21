using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色控制器基类 - Camera
    /// </summary>
    public abstract partial class BaseActorController
    {
        /// <summary>
        /// 角色相机Rig
        /// </summary>
        public IActorCameraRig CameraRig
        {
            get
            {
                if (cameraRig == null)
                {
                    cameraRig = CreateCameraRig();
                }
                return cameraRig;
            }
        }

        private IActorCameraRig cameraRig;

        /// <summary>
        /// 创建角色相机Rig
        /// </summary>
        protected virtual IActorCameraRig CreateCameraRig()
        {
            return new ActorCameraRig(this);
        }

        /// <summary>
        /// 是否绑定了相机
        /// </summary>
        public bool CameraBinded => Logic == null ? false : Logic.CameraBinded;

        /// <summary>
        /// 相机X轴左右偏移
        /// </summary>
        public float CameraOffsetX
        {
            get => CameraRig.Offset.x;
            set
            {
                Vector3 offset = CameraRig.Offset;
                offset.x = value;
                CameraRig.Offset = offset;
            }
        }

        /// <summary>
        /// 相机Y轴上下偏移
        /// </summary>
        public float CameraOffsetY
        {
            get => CameraRig.Offset.y;
            set
            {
                Vector3 offset = CameraRig.Offset;
                offset.y = value;
                CameraRig.Offset = offset;
            }
        }

        /// <summary>
        /// 相机Z轴前后偏移
        /// </summary>
        public float CameraOffsetZ
        {
            get => CameraRig.Offset.z;
            set
            {
                Vector3 offset = CameraRig.Offset;
                offset.z = value;
                CameraRig.Offset = offset;
            }
        }

        /// <summary>
        /// 旋转相机挂点
        /// </summary>
        public void RotateCameraPoint(float mx, float my)
        {
            CameraRig.Rotate(new Vector2(mx, my));
        }

        /// <summary>
        /// 更新相机坐标 (旋转后更新)
        /// </summary>
        public void UpdateCameraPointPos()
        {
            CameraRig.UpdatePosition();
        }
    }
}
