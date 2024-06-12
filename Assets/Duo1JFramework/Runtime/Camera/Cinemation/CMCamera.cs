using Cinemachine;
using UnityEngine;

namespace Duo1JFramework.CameraAPI
{
    /// <summary>
    /// Cinemachine相机
    /// </summary>
    public class CMCamera : ICamera
    {
        public CinemachineVirtualCamera VirtualCamera
        {
            get => virtualCamera;
            private set
            {
                virtualCamera = value;
                Priority = virtualCamera.Priority;
            }
        }
        private CinemachineVirtualCamera virtualCamera;

        public int Priority
        {
            get
            {
                if (VirtualCamera == null)
                {
                    return int.MinValue;
                }

                return VirtualCamera.Priority;
            }
            set
            {
                priority = value;
                VirtualCamera.Priority = value;
            }
        }
        private int priority;

        public void Follow(ICameraFollow t)
        {
            if (t == null)
            {
                VirtualCamera.Follow = null;
                return;
            }
            VirtualCamera.Follow = t.CameraFollowPoint;
        }

        public void LookAt(ICameraLookAt t)
        {
            if (t == null)
            {
                VirtualCamera.LookAt = null;
                return;
            }
            VirtualCamera.LookAt = t.CameraLookAtPoint;
        }

        /// <summary>
        /// 设置临时优先级
        /// </summary>
        public void SetTempPriority(int priority)
        {
            if (VirtualCamera == null)
            {
                Log.ErrorForce($"{ToString()} 虚拟相机未初始化");
                return;
            }

            VirtualCamera.Priority = priority;
        }

        /// <summary>
        /// 重置临时优先级
        /// </summary>
        public void ResetTempPriority()
        {
            Priority = priority;
        }

        public void InitCamera(params object[] param)
        {
            DestroyCamera();

            if (param.Length == 0)
            {
                throw CommonException.Create("CMCamera初始化参数错误");
            }
            string prefabPath = param[0] as string;
            Assert.NotNullOrEmpty(prefabPath, "主虚拟相机路径不可为空");

            VirtualCamera = CMBrain.Instance.LoadVirtualCamera(prefabPath);
        }

        public void InitCamera(CinemachineVirtualCamera virtualCamera)
        {
            DestroyCamera();
            VirtualCamera = virtualCamera;
        }

        /// <summary>
        /// 创建相机
        /// </summary>
        public static CMCamera CreateCamera(string prefabPath)
        {
            return CMBrain.Instance.CreateCamera(prefabPath);
        }

        public void DestroyCamera()
        {
            if (VirtualCamera != null)
            {
                VirtualCamera.gameObject.DestroyImmediate();
            }
        }

        public CMCamera()
        {
            CMBrain.Instance.Trigger();
            CMBrain.Instance.AddCMCamera(this);
        }
    }
}