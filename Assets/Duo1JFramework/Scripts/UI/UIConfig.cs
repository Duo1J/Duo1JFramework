using Duo1JFramework.Asset;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI配置
    /// </summary>
    public class UIConfig
    {
        /// <summary>
        /// 窗口预制体路径
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// 窗口层级
        /// </summary>
        public EUILayer Layer { get; private set; } = EUILayer.Normal;

        /// <summary>
        /// 加载方式
        /// </summary>
        public EAssetLoadType LoadType { get; private set; } = EAssetLoadType.AssetBundle;

        /// <summary>
        /// 是否是全屏窗口
        /// </summary>
        public bool IsFullScreen { get; private set; } = false;

        public UIConfig SetLayer(EUILayer layer)
        {
            Layer = layer;
            return this;
        }

        public UIConfig SetLoadType(EAssetLoadType loadType)
        {
            LoadType = loadType;
            return this;
        }

        public UIConfig SetIsFullScreen(bool isFullScreen)
        {
            IsFullScreen = isFullScreen;
            return this;
        }

        public UIConfig(string path)
        {
            Path = path;
        }
    }
}