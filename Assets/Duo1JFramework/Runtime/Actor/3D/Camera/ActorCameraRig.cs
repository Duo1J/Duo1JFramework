using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 默认角色相机Rig
    /// </summary>
    public class ActorCameraRig : IActorCameraRig
    {
        private readonly BaseActorController controller;

        public bool Valid => CameraPoint != null;

        public Transform CameraPoint
        {
            get
            {
                if (controller == null || controller.Point == null)
                {
                    return null;
                }

                controller.Point.TryGetCameraPoint(out Transform point);
                return point;
            }
        }

        public Vector3 Offset { get; set; }

        public ActorCameraRig(BaseActorController controller)
        {
            this.controller = controller;
        }

        public void Rotate(Vector2 lookInput)
        {
            if (!Valid || lookInput == Vector2.zero)
            {
                return;
            }

            ActorParam param = controller.Param;
            Transform cameraPoint = CameraPoint;

            Vector3 angle = cameraPoint.eulerAngles;
            float x = angle.x - lookInput.y * param.mouseSpeedY * Time.deltaTime;
            if (x > 180) x -= 360;
            x = Mathf.Clamp(x, param.cameraMinRotate, param.cameraMaxRotate);

            cameraPoint.localRotation = Quaternion.Euler(
                x,
                angle.y + lookInput.x * param.mouseSpeedX * Time.deltaTime,
                angle.z
            );
        }

        public void UpdatePosition()
        {
            if (!Valid)
            {
                return;
            }

            ActorParam param = controller.Param;
            Transform cameraPoint = CameraPoint;
            cameraPoint.localPosition = (controller.Point.OriCameraPointLocPos - cameraPoint.forward * param.cameraToActorLen) +
                cameraPoint.forward * Offset.z + cameraPoint.right * Offset.x + cameraPoint.up * Offset.y;
        }
    }
}