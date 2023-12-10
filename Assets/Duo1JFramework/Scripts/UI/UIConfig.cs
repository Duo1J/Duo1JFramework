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
        public string path;

        /// <summary>
        /// 窗口层级
        /// </summary>
        public UILayer layer = UILayer.Normal;

        /// <summary>
        /// 同步加载
        /// </summary>
        public bool sync = false;

        public UIConfig(string path)
        {
            this.path = path;
        }
    }
}