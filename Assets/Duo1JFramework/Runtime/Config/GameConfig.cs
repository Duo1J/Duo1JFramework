using Duo1JFramework.Asset;
using UnityEngine;

namespace Duo1JFramework.Config
{
    /// <summary>
    /// 游戏配置
    /// </summary>
    public class GameConfig : MonoSingleton<GameConfig>
    {
        #region Runtime

        [Header("Runtime")]
        [Label("运行时资源加载类型")]
        public EAssetLoaderType runtimeAssetLoaderType = EAssetLoaderType.AssetBundle;

        #endregion Runtime

        #region Editor

        [Header("Editor")]
        [Label("编辑器下资源加载类型")]
        public EAssetLoaderType editorAssetLoaderType = EAssetLoaderType.AssetDatabase;

        #endregion Editor

        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}