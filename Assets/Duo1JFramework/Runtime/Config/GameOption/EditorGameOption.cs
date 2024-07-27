using Duo1JFramework.Asset;
using System;
using UnityEngine;

namespace Duo1JFramework.Config
{
    /// <summary>
    /// 编辑器下游戏配置选项
    /// </summary>
    [Serializable]
    public class EditorGameOption
    {
        [Label("资源加载器")]
        public EAssetLoaderType assetLoaderType = EAssetLoaderType.AssetDatabase;

        [Space]
        [Label("使用Log4Net日志")]
        public bool useLog4Net = true;
    }
}
