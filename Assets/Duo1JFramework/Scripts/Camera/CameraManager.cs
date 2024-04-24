using Duo1JFramework.Config;
using UnityEngine;

namespace Duo1JFramework.CameraAPI
{
    /// <summary>
    /// 相机管理器
    /// </summary>
    public class CameraManager : MonoSingleton<CameraManager>
    {
        /// <summary>
        /// 当前相机
        /// </summary>
        public ICamera Camera { get; private set; }

        /// <summary>
        /// 当前相机跟随
        /// </summary>
        public ICameraFollow Follow { get; set; }

        /// <summary>
        /// 当前相机注视
        /// </summary>
        public ICameraLookAt LookAt { get; set; }

        /// <summary>
        /// 初始化当前相机
        /// </summary>
        public ICamera InitCamera<T>(params object[] param) where T : ICamera, new()
        {
            if (Camera != null)
            {
                Log.ErrorForce($"相机已初始化为`{Camera.GetType()}`，不可重复初始化");
                return Camera;
            }
            Camera = CreateCamera<T>(param);
            return Camera;
        }

        /// <summary>
        /// 销毁当前相机
        /// </summary>
        public void DestroyCamera()
        {
            if (Camera != null)
            {
                Camera.DestroyCamera();
                Camera = null;
            }
        }

        /// <summary>
        /// 检查当前相机是否创建
        /// </summary>
        private bool CheckCamera()
        {
            return Camera != null;
        }

        /// <summary>
        /// 创建相机
        /// </summary>
        public ICamera CreateCamera<T>(params object[] param) where T : ICamera, new()
        {
            ICamera camera = new T();
            camera.InitCamera(param);
            return camera;
        }

        protected override void OnDispose()
        {
        }

        protected override void OnInit()
        {
            Register.RegisterLateUpdate(OnUpdate);
        }

        private void OnUpdate()
        {
            if (!CheckCamera())
            {
                return;
            }

            Camera.LookAt(LookAt);
            Camera.Follow(Follow);
        }

        public Camera MainCamera => UnityEngine.Camera.main;

        /// <summary>
        /// 获取或创建主相机
        /// </summary>
        public Camera GetOrCreateMainCamera()
        {
            Camera mainCamera = MainCamera;
            if (mainCamera == null)
            {
                GameObject cameraGo = new GameObject("NewMainCamera");
                mainCamera = cameraGo.AddComponent<Camera>();
                mainCamera.tag = TagDef.MAIN_CAMERA;
            }
            SetMainCamera(mainCamera);

            return mainCamera;
        }

        /// <summary>
        /// 设置主相机
        /// </summary>
        public void SetMainCamera(Camera tarCamera)
        {
            if (MainCamera != null && MainCamera != tarCamera)
            {
                DestroyMainCamera();
            }
            tarCamera.tag = TagDef.MAIN_CAMERA;
            tarCamera.name = $"[Render]MainCamera";
            tarCamera.GetOrAddComponent<AudioListener>();
            DontDestroyOnLoad(tarCamera.gameObject);
        }

        /// <summary>
        /// 销毁当前主相机
        /// </summary>
        public void DestroyMainCamera()
        {
            if (MainCamera != null)
            {
                MainCamera.gameObject.DestroyImmediate();
            }
        }
    }
}