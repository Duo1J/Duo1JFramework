using Duo1JFramework.Config;
using UnityEngine;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI物体根节点
    /// </summary>
    [DisallowMultipleComponent]
    public class UIRoot : BaseMono
    {
        public static Transform BottomLayer => Root.Instance.UIRoot.bottomLayer;
        public static Transform NormalLayer => Root.Instance.UIRoot.normalLayer;
        public static Transform TopLayer => Root.Instance.UIRoot.topLayer;
        public static Transform ConstLayer => Root.Instance.UIRoot.constLayer;

        public static Camera UICamera => Root.Instance.UIRoot.uiCamera;

        public static Canvas UICanvas => Root.Instance.UIRoot.uiCanvas;
        public static Canvas BottomCanvas => Root.Instance.UIRoot.bottomCanvas;
        public static Canvas NormalCanvas => Root.Instance.UIRoot.normalCanvas;
        public static Canvas TopCanvas => Root.Instance.UIRoot.topCanvas;
        public static Canvas ConstCanvas => Root.Instance.UIRoot.constCanvas;

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
            uiCamera.depth = Def.UI.UI_CAMERA_DEPTH;
            uiCamera.cullingMask = Def.UI.UI_CULLING_MASK;
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