using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色控制器基类 - Camera
    /// </summary>
    public abstract partial class BaseActorController
    {
        /// <summary>
        /// 是否绑定了相机
        /// </summary>
        public bool CameraBinded => Logic == null ? false : Logic.CameraBinded;

        /// <summary>
        /// 相机X轴左右偏移
        /// </summary>
        public float CameraOffsetX { get; set; }

        /// <summary>
        /// 相机Y轴上下偏移
        /// </summary>
        public float CameraOffsetY { get; set; }

        /// <summary>
        /// 相机Z轴前后偏移
        /// </summary>
        public float CameraOffsetZ { get; set; }

        /// <summary>
        /// 旋转相机挂点
        /// </summary>
        public void RotateCameraPoint(float mx, float my)
        {
            if (CheckAxisZero(mx, my))
            {
                return;
            }

            Transform cameraPoint = point.CameraPoint;

            Vector3 angle = cameraPoint.eulerAngles;
            float x = angle.x - my * param.mouseSpeedY * Time.deltaTime;
            if (x > 180) x -= 360;
            x = Mathf.Clamp(x, param.cameraMinRotate, param.cameraMaxRotate);

            cameraPoint.localRotation = Quaternion.Euler(
                    x,
                    angle.y + mx * param.mouseSpeedX * Time.deltaTime,
                    angle.z
                );
        }

        /// <summary>
        /// 更新相机坐标 (旋转后更新)
        /// </summary>
        public void UpdateCameraPointPos()
        {
            Transform cameraPoint = point.CameraPoint;
            point.CameraPoint.localPosition = (point.OriCameraPointLocPos - cameraPoint.forward * param.cameraToActorLen) +
                cameraPoint.forward * CameraOffsetZ + cameraPoint.right * CameraOffsetX + cameraPoint.up * CameraOffsetY;
        }
    }
}
