using Cinemachine;

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
            Brain = gameObject.GetOrAddComponent<CinemachineBrain>();
        }
    }
}