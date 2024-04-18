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
        public UILayer Layer { get; private set; } = UILayer.Normal;

        /// <summary>
        /// 同步加载
        /// </summary>
        public bool Sync { get; private set; } = false;

        /// <summary>
        /// 是否是Resources下资源
        /// </summary>
        public bool IsResource { get; private set; } = false;

        /// <summary>
        /// 是否是全屏窗口
        /// </summary>
        public bool IsFullScreen { get; private set; } = false;

        public UIConfig SetLayer(UILayer layer)
        {
            Layer = layer;
            return this;
        }

        public UIConfig SetSync(bool sync)
        {
            Sync = sync;
            return this;
        }

        public UIConfig SetIsResource(bool isResource)
        {
            IsResource = isResource;
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