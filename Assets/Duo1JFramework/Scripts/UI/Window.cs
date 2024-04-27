using UnityEngine;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI逻辑
    /// </summary>
    public abstract class Window : BaseRegister
    {
        #region Field

        /// <summary>
        /// 窗口ID
        /// </summary>
        public long ID { get; private set; }

        /// <summary>
        /// UI物体
        /// </summary>
        public GameObject Go
        {
            get => go;
            set
            {
                go = value;
                RectTF = go.GetAndAssertComponent<RectTransform>($"{ToString()} 未包含RectTransform组件");
                Controller = go.GetAndAssertComponent<UIController>($"{ToString()} 未包含UIController组件");
            }
        }
        private GameObject go;

        public RectTransform RectTF { get; private set; }

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

        #endregion Field

        #region Public

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
            ResetRectTransform();
        }

        /// <summary>
        /// 重置RectTransform，铺满Canvas
        /// </summary>
        public void ResetRectTransform()
        {
            RectTF.ExpandAnchor();
            RectTF.ResetSRT();
            RectTF.sizeDelta = Vector2.zero;
        }

        /// <summary>
        /// 移到远处，用以处理全屏策略
        /// </summary>
        public void MoveToFar()
        {
            RectTF.sizeDelta = Def.UI_FAR_POS;
        }

        /// <summary>
        /// 获取Go
        /// </summary>
        public GameObject GetGo(string goName)
        {
            return Controller.GetGo(goName);
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        public T GetCom<T>(string goName) where T : MonoBehaviour
        {
            return Controller.GetCom<T>(goName);
        }

        #endregion Public

        #region Protected

        #endregion Protected

        #region Lifecycle

        /// <summary>
        /// 子类创建UI配置
        /// </summary>
        protected abstract UIConfig CreateUIConfig();

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (init)
            {
                return;
            }
            init = true;
            OnInit();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            OnDispose();
            Go?.DestroyImmediate();
        }

        /// <summary>
        /// 子类初始化
        /// </summary>
        protected abstract void OnInit();

        /// <summary>
        /// 子类销毁
        /// </summary>
        protected override void OnDispose()
        {
        }

        public Window()
        {
            ID = UIManager.Instance.GetIncID();
        }

        #endregion Lifecycle

        public override string ToString()
        {
            return $"<UI-{ID}-{GetType().Name}-{Go.name}>";
        }
    }
}