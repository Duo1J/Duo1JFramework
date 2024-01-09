using Cinemachine;
using UnityEngine;

namespace Duo1JFramework.Camera3D
{
    /// <summary>
    /// CinemachineBarin
    /// </summary>
    public class CMBrain : MonoSingleton<CMBrain>
    {
        public CinemachineBrain Brain { get; private set; }

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