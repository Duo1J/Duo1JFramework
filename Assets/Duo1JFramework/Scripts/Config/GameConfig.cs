using Duo1JFramework.Asset;

namespace Duo1JFramework.Config
{
    /// <summary>
    /// 游戏配置
    /// </summary>
    public class GameConfig : MonoSingleton<GameConfig>
    {
        #region Config

        [Label("编辑器下资源加载类型")]
        public eAssetLoaderType editorAssetLoaderType = eAssetLoaderType.AssetDatabase;

        [Label("运行时资源加载类型")]
        public eAssetLoaderType runtimeAssetLoaderType = eAssetLoaderType.AssetBundle;

        #endregion Config

        protected override void OnDispose()
        {
        }

        protected override void OnInit()
        {
        }
    }
}