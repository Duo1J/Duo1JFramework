using Cinemachine;
using Duo1JFramework.Asset;
using UnityEngine;

namespace Duo1JFramework.Camera3D
{
    /// <summary>
    /// CinemachineBarin
    /// </summary>
    public class CMBrain : MonoSingleton<CMBrain>
    {
        public CinemachineBrain Brain { get; private set; }

        /// <summary>
        /// 加载虚拟相机预制体
        /// </summary>
        public CinemachineVirtualCamera LoadVirtualCamera(string prefabPath)
        {
            GameObject cameraGo = AssetManager.Instance.LoadSync<GameObject>(prefabPath);
            if (cameraGo == null)
            {
                return null;
            }
            CinemachineVirtualCamera ret = cameraGo.GetComponent<CinemachineVirtualCamera>();
            if (ret == null)
            {
                Log.ErrorForce($"无法从{cameraGo.name}上获取到CinemachineVirtualCamera组件");
                cameraGo.DestroyImmediate();
                return null;
            }
            cameraGo.SetParent(Root.Instance.VirtualCameraRoot);
            return ret;
        }

        /// <summary>
        /// 创建相机
        /// </summary>
        public static CMCamera CreateCamera(string prefabPath)
        {
            CMCamera camera = new CMCamera();
            camera.InitCamera(prefabPath);
            return camera;
        }

        protected override void OnDispose()
        {
        }

        protected override void OnInit()
        {
            Camera mainCamera = CameraManager.Instance.GetOrCreateMainCamera();
            Brain = mainCamera.gameObject.GetOrAddComponent<CinemachineBrain>();
        }
    }
}