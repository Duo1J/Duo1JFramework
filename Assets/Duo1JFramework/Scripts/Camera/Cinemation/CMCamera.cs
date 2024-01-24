using Cinemachine;
using UnityEngine;

namespace Duo1JFramework.Camera3D
{
    /// <summary>
    /// Cinemachine相机
    /// </summary>
    public class CMCamera : ICamera
    {
        public CinemachineVirtualCamera CM { get; private set; }

        public void Follow(Transform t)
        {
            CM.Follow = t;
        }

        public void LookAt(Transform t)
        {
            CM.LookAt = t;
        }

        public void SetPriority(int priority)
        {
            CM.Priority = priority;
        }

        public void InitCamera(params object[] param)
        {
            DestroyCamera();

            if (param.Length == 0)
            {
                Assert.Throw("CMCamera初始化参数错误");
                return;
            }
            string prefabPath = param[0] as string;
            Assert.NotNullOrEmpty(prefabPath, "主虚拟相机路径不可为空");

            CM = CMBrain.Instance.LoadVirtualCamera(prefabPath);
        }

        /// <summary>
        /// 创建相机
        /// </summary>
        public static CMCamera CreateCamera(string prefabPath)
        {
            return CMBrain.CreateCamera(prefabPath);
        }

        public void DestroyCamera()
        {
            if (CM != null)
            {
                CM.gameObject.DestroyImmediate();
            }
        }

        public CMCamera()
        {
            CMBrain.Instance.Trigger();
        }
    }
}