using Cinemachine;
using Duo1JFramework.Asset;
using UnityEngine;

namespace Duo1JFramework.Camera3D
{
    /// <summary>
    /// Cinemachine相机
    /// </summary>
    public class CMCamera : ICamera
    {
        public Camera MainCamera { get; private set; }
        public CinemachineVirtualCamera MainCMCamera { get; private set; }

        public void Follow(Transform t)
        {
            MainCMCamera.Follow = t;
        }

        public void LookAt(Transform t)
        {
            MainCMCamera.LookAt = t;
        }

        public void InitCamera(params object[] param)
        {
            CameraManager.Instance.DestroyMainCamera();

            if (param.Length == 0)
            {
                Assert.Throw("CMCamera初始化参数错误");
                return;
            }
            string prefabPath = param[0] as string;
            Assert.NotNullOrEmpty(prefabPath, "主虚拟相机路径不可为空");

            GameObject cameraGo = AssetManager.Instance.LoadSync<GameObject>(prefabPath);
            MainCamera = cameraGo.GetAndAssertComponent<Camera>("主虚拟相机预制体未包含Camera组件");
            MainCMCamera = cameraGo.GetAndAssertComponent<CinemachineVirtualCamera>("主虚拟相机预制体未包含CinemachineVirtualCamera组件");
            CameraManager.Instance.SetMainCamera(MainCamera);
        }

        public CMCamera()
        {
            CMBrain.Instance.Trigger();
        }
    }
}