using UnityEngine;

namespace Duo1JFramework.UI
{
    public abstract class Window
    {
        /// <summary>
        /// UI物体
        /// </summary>
        public GameObject Go { get; set; }

        /// <summary>
        /// UI配置
        /// </summary>
        public UIConfig Config
        {
            get
            {
                if (config == null)
                {
                    config = CreateUIConfig();
                }
                return config;
            }
        }
        private UIConfig config;

        /// <summary>
        /// 子类创建UI配置
        /// </summary>
        protected abstract UIConfig CreateUIConfig();

        /// <summary>
        /// 设置父节点
        /// </summary>
        public void SetParent(Transform par)
        {
            Assert.NotNull(par, "参数par为空");
            if (Go == null)
            {
                Log.Error($"窗口`{GetType().FullName}`未加载资源，无法设置父节点");
                return;
            }
            Go.transform.SetParent(par);
        }
    }
}