using Cinemachine;
using UnityEngine;

namespace Duo1JFramework.CameraAPI
{
    /// <summary>
    /// Cinemachine Camera 控制器
    /// 可将CinemachineVirtualCamera转为CMCamera
    /// </summary>
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    [DisallowMultipleComponent]
    public class CMCameraController : BaseMono
    {
        public CMCamera Camera { get; private set; }
        public CinemachineVirtualCamera VirtualCamera { get; private set; }

        private void Awake()
        {
            VirtualCamera = this.GetAndAssertComponent<CinemachineVirtualCamera>($"{ToString()}必须挂载`CinemachineVirtualCamera`组件");
            Camera = CMBrain.Instance.CreateCamera(VirtualCamera);
        }
    }
}
