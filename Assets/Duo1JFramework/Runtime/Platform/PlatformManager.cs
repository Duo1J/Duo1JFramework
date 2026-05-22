namespace Duo1JFramework.PlatformAPI
{
    /// <summary>
    /// 平台管理器
    /// </summary>
    public class PlatformManager : MonoSingleton<PlatformManager>
    {
        protected override void OnInit()
        {
            Platform.Init();
        }

        protected override void OnDispose()
        {
            Platform.Reset();
        }
    }
}
