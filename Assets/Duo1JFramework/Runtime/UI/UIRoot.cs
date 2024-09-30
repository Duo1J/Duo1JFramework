using UnityEngine;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI物体根节点
    /// </summary>
    [DisallowMultipleComponent]
    public class UIRoot : BaseMono
    {
        /// <summary>
        /// 底层层级
        /// </summary>
        public static Transform BottomLayer => Root.UIRoot.bottomLayer;

        /// <summary>
        /// 普通层层级
        /// </summary>
        public static Transform NormalLayer => Root.UIRoot.normalLayer;

        /// <summary>
        /// 顶层层级
        /// </summary>
        public static Transform TopLayer => Root.UIRoot.topLayer;

        /// <summary>
        /// 常驻层层级
        /// </summary>
        public static Transform ConstLayer => Root.UIRoot.constLayer;

        /// <summary>
        /// UI相机
        /// </summary>
        public static Camera UICamera => Root.UIRoot.uiCamera;

        /// <summary>
        /// UI画布
        /// </summary>
        public static Canvas UICanvas => Root.UIRoot.uiCanvas;

        /// <summary>
        /// 底层UI画布
        /// </summary>
        public static Canvas BottomCanvas => Root.UIRoot.bottomCanvas;

        /// <summary>
        /// 普通层UI画布
        /// </summary>
        public static Canvas NormalCanvas => Root.UIRoot.normalCanvas;

        /// <summary>
        /// 顶层UI画布
        /// </summary>
        public static Canvas TopCanvas => Root.UIRoot.topCanvas;

        /// <summary>
        /// 常驻层UI画布
        /// </summary>
        public static Canvas ConstCanvas => Root.UIRoot.constCanvas;

        [SerializeField]
        private Transform bottomLayer;
        [SerializeField]
        private Transform normalLayer;
        [SerializeField]
        private Transform topLayer;
        [SerializeField]
        private Transform constLayer;

        private Canvas bottomCanvas;
        private Canvas normalCanvas;
        private Canvas topCanvas;
        private Canvas constCanvas;

        [SerializeField]
        [Label("UI相机")]
        private Camera uiCamera;
        [SerializeField]
        [Label("UI画布")]
        private Canvas uiCanvas;

        /// <summary>
        /// 添加窗口到对应层级
        /// </summary>
        public void AddToLayer(Window wnd)
        {
            switch (wnd.Layer)
            {
                case EUILayer.Bottom:
                    {
                        wnd.SetParent(bottomLayer);
                        break;
                    }
                case EUILayer.Normal:
                    {
                        wnd.SetParent(normalLayer);
                        break;
                    }
                case EUILayer.Top:
                    {
                        wnd.SetParent(topLayer);
                        break;
                    }
                case EUILayer.Const:
                    {
                        wnd.SetParent(constLayer);
                        break;
                    }
                default:
                    {
                        Log.Error($"未处理的层级 `{wnd.Layer}`");
                        break;
                    }
            }
        }

        /// <summary>
        /// 获取层级的基础排序层级
        /// </summary>
        public int GetBaseSortingOrder(EUILayer layer)
        {
            switch (layer)
            {
                case EUILayer.Bottom:
                    {
                        return bottomCanvas.sortingOrder;
                    }
                case EUILayer.Normal:
                    {
                        return normalCanvas.sortingOrder;
                    }
                case EUILayer.Top:
                    {
                        return topCanvas.sortingOrder;
                    }
                case EUILayer.Const:
                    {
                        return constCanvas.sortingOrder;
                    }
                default:
                    {
                        Log.Error($"未处理的层级 `{layer}`");
                        return 0;
                    }
            }
        }

        private void Awake()
        {
            gameObject.name = GetType().Name;
            DontDestroyOnLoad(gameObject);

            InitUICamera();
            InitCanvas();
        }

        private void InitUICamera()
        {
            if (uiCamera == null)
            {
                throw CommonException.Create("UICamera为空");
            }
            uiCamera.depth = Def.UI.CAMERA_DEPTH;
            uiCamera.cullingMask = Def.UI.CULLING_MASK;
            uiCamera.gameObject.layer = Def.Layer.UI;
            uiCamera.name = "[Render]UICamera";
        }

        private void InitCanvas()
        {
            bottomCanvas = bottomLayer.GetAndAssertComponent<Canvas>($"`{bottomLayer.name}` 未找到Canvasn组件");
            normalCanvas = normalLayer.GetAndAssertComponent<Canvas>($"`{normalLayer.name}` 未找到Canvasn组件");
            topCanvas = topLayer.GetAndAssertComponent<Canvas>($"`{topLayer.name}` 未找到Canvasn组件");
            constCanvas = constLayer.GetAndAssertComponent<Canvas>($"`{constLayer.name}` 未找到Canvasn组件");
        }
    }
}