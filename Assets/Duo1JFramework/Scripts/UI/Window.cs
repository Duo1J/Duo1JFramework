using UnityEngine;

namespace Duo1JFramework.UI
{
    public abstract class Window
    {
        /// <summary>
        /// UI物体
        /// </summary>
        public GameObject Go
        {
            get => go;
            set
            {
                go = value;
                Controller = go.GetAndAssertComponent<UIController>($"窗口`{GetType().FullName}`未包含UIController组件");
            }
        }
        private GameObject go;

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
        /// UI控制器
        /// </summary>
        public UIController Controller { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Layer
        {
            get => layer;
            set
            {
                int parLayer = 0;
                Canvas parCanvas = Go.GetComponentInParent<Canvas>();
                if (parCanvas != null)
                {
                    parLayer = parCanvas.sortingOrder;
                }
                layer = parLayer + value;
                Controller.UpdateLayer(layer);
            }
        }
        private int layer;

        private bool init = false;
        private bool dispose = false;

        /// <summary>
        /// 子类创建UI配置
        /// </summary>
        protected abstract UIConfig CreateUIConfig();

        /// <summary>
        /// 初始化
        /// </summary>
        public void OnInit()
        {
            if (init)
            {
                return;
            }
            init = true;
            OnInitInner();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void OnDispose()
        {
            if (dispose)
            {
                return;
            }
            dispose = true;
            OnDisposeInner();
            if (Go != null)
            {
                Object.DestroyImmediate(Go);
            }
        }

        /// <summary>
        /// 子类重写初始化
        /// </summary>
        protected virtual void OnInitInner()
        {
        }

        /// <summary>
        /// 子类重写销毁
        /// </summary>
        protected virtual void OnDisposeInner()
        {
        }

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
            RectTransform rectTf = Go.GetComponent<RectTransform>();
            if (rectTf != null)
            {
                rectTf.ExpandAnchor();
                rectTf.ResetSRT();
                rectTf.sizeDelta = Vector2.zero;
            }
        }
    }
}