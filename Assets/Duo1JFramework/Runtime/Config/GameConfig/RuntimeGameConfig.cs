using Duo1JFramework.Asset;
using System;

namespace Duo1JFramework.Config
{
    /// <summary>
    /// 运行时游戏配置
    /// </summary>
    [Serializable]
    public class RuntimeGameConfig
    {
        [Label("运行时资源加载类型")]
        public EAssetLoaderType assetLoaderType = EAssetLoaderType.AssetBundle;
    }
}
