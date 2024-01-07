using UnityEngine;

namespace Duo1JFramework.Camera3D
{
    /// <summary>
    /// 3D相机管理器
    /// </summary>
    public class CameraManager : MonoSingleton<CameraManager>
    {
        public ICamera Camera { get; private set; }

        /// <summary>
        /// 跟随
        /// </summary>
        public void Follow(Transform t)
        {
            CheckCamera();
            Camera.Follow(t);
        }

        /// <summary>
        /// 注视
        /// </summary>
        public void LookAt(Transform t)
        {
            CheckCamera();
            Camera.LookAt(t);
        }

        /// <summary>
        /// 初始化相机
        /// </summary>
        public ICamera InitCamera<T>(params object[] param) where T : ICamera, new()
        {
            if (Camera != null)
            {
                Log.ErrorForce($"相机已初始化为`{Camera.GetType()}`，不可重复初始化");
                return Camera;
            }
            Camera = new T();
            Camera.InitCamera(param);

            return Camera;
        }

        private void CheckCamera()
        {
            Assert.NotNull(Camera, "相机未初始化");
        }

        protected override void OnDispose()
        {
        }

        protected override void OnInit()
        {
        }

        /// <summary>
        /// 获取或创建主相机
        /// </summary>
        /// <returns></returns>
        public Camera GetMainCamera()
        {
            Camera mainCamera = UnityEngine.Camera.main;
            if (mainCamera == null)
            {
                GameObject cameraGo = new GameObject("[Render]Camera");
                cameraGo.tag = "MainCamera";
                mainCamera = cameraGo.AddComponent<Camera>();
            }
            else
            {
                mainCamera.name = $"[Render]{mainCamera.name}";
            }

            return mainCamera;
        }

        /// <summary>
        /// 销毁当前主相机
        /// </summary>
        public void DestroyMainCamera()
        {
            Camera mainCamera = UnityEngine.Camera.main;
            if (mainCamera != null)
            {
                mainCamera.gameObject.DestroyImmediate();
            }
        }
    }
}