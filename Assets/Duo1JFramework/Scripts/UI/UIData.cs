using Duo1JFramework.Asset;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI数据
    /// </summary>
    public class UIData
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

        public UIData SetLayer(EUILayer layer)
        {
            Layer = layer;
            return this;
        }

        public UIData SetLoadType(EAssetLoadType loadType)
        {
            LoadType = loadType;
            return this;
        }

        public UIData SetIsFullScreen(bool isFullScreen)
        {
            IsFullScreen = isFullScreen;
            return this;
        }

        public UIData(string path)
        {
            Path = path;
        }
    }
}