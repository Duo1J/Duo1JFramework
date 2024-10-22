using UnityEngine;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI窗口逻辑
    /// </summary>
    public abstract class Window : BaseRegister
    {
        #region Field

        /// <summary>
        /// 窗口ID
        /// </summary>
        public long ID { get; private set; }

        /// <summary>
        /// UI配置数据
        /// </summary>
        public UIData Config
        {
            get
            {
                if (config == null)
                {
                    config = GetUIConfig();
                }

                return config;
            }
        }
        private UIData config;

        /// <summary>
        /// UI物体
        /// </summary>
        public GameObject Go
        {
            get => go;
            set
            {
                go = value;

                if (go == null)
                {
                    RectTF = null;
                    Controller = null;
                }
                else
                {
                    RectTF = go.GetAndAssertComponent<RectTransform>($"{ToString()} 未包含RectTransform组件");
                    Controller = go.GetAndAssertComponent<UIController>($"{ToString()} 未包含UIController组件");
                }
            }
        }
        private GameObject go;

        public RectTransform RectTF { get; private set; }

        /// <summary>
        /// UI控制器
        /// </summary>
        public UIController Controller { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public EUILayer Layer => Config.Layer;

        /// <summary>
        /// 是否是常驻层级
        /// </summary>
        public bool IsConstLayer => Layer == EUILayer.Const;

        /// <summary>
        /// 排序层级
        /// </summary>
        public int SortingOrder
        {
            get => Controller.SortingOrder;
            set => Controller.SortingOrder = value;
        }

        /// <summary>
        /// 排序层级偏移
        /// </summary>
        public int SortingOrderOffset
        {
            get
            {
                return Mathf.Max(0, SortingOrder - ParSortingOrder);
            }
            set
            {
                SortingOrder = ParSortingOrder + value;
            }
        }

        /// <summary>
        /// 父Canvas的排序层级
        /// </summary>
        public int ParSortingOrder => Root.UIRoot.GetBaseSortingOrder(Config.Layer);

        /// <summary>
        /// 是否初始化
        /// </summary>
        private bool init = false;

        #endregion Field

        #region Public Method

        /// <summary>
        /// 设置父节点
        /// </summary>
        public void SetParent(Transform par)
        {
            Assert.NotNullArg(par, "par");
            if (RectTF == null)
            {
                Log.ErrorForce($"{ToString()} RectTF为空，无法设置其父节点");
                return;
            }

            RectTF.SetParent(par);
            ResetRectTransform();
        }

        /// <summary>
        /// 设置显隐
        /// </summary>
        public virtual void SetActive(bool active)
        {
            if (Go == null)
            {
                return;
            }

            Go.SetActive(active);
        }

        /// <summary>
        /// 重置RectTransform，铺满Canvas
        /// </summary>
        public void ResetRectTransform()
        {
            if (RectTF == null)
            {
                return;
            }

            RectTF.ExpandAnchor();
            RectTF.ResetSRT();
            RectTF.sizeDelta = Vector2.zero;
        }

        /// <summary>
        /// 移到远处，用以处理全屏策略
        /// </summary>
        public void MoveToFar()
        {
            if (RectTF == null)
            {
                return;
            }

            RectTF.localPosition = Def.UI.FAR_POS;
        }

        /// <summary>
        /// 获取Go
        /// </summary>
        public GameObject GetGo(string goName)
        {
            if (Controller == null)
            {
                return null;
            }

            return Controller.GetGo(goName);
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        public T GetCom<T>(string goName) where T : MonoBehaviour
        {
            if (Controller == null)
            {
                return null;
            }

            return Controller.GetCom<T>(goName);
        }

        /// <summary>
        /// 关闭此窗口
        /// </summary>
        public void CloseWindow()
        {
            UIManager.Instance.CloseWindow(this);
        }

        #endregion Public Method

        #region Lifecycle

        /// <summary>
        /// 子类创建UI配置数据
        /// </summary>
        protected abstract UIData GetUIConfig();

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
        /// 子类初始化
        /// </summary>
        protected abstract void OnInit();

        /// <summary>
        /// 销毁
        /// </summary>
        protected override void OnDispose()
        {
            if (Go != null)
            {
                Go.DestroyImmediate();
                Go = null;
            }
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
