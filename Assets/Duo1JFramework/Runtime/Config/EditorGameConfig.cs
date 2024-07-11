using Duo1JFramework.Asset;
using System;

namespace Duo1JFramework.Config
{
    /// <summary>
    /// 编辑器下游戏配置
    /// </summary>
    [Serializable]
    public class EditorGameConfig
    {
        [Label("编辑器下资源加载类型")]
        public EAssetLoaderType assetLoaderType = EAssetLoaderType.AssetDatabase;
    }
}
