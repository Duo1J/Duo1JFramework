using Duo1JFramework.Config;
using UnityEngine;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI物体根节点
    /// </summary>
    [DisallowMultipleComponent]
    public class UIRoot : MonoBehaviour
    {
        public static Transform BottomLayer => Root.Instance.UIRoot.bottomLayer;
        public static Transform NormalLayer => Root.Instance.UIRoot.normalLayer;
        public static Transform TopLayer => Root.Instance.UIRoot.topLayer;
        public static Transform ConstLayer => Root.Instance.UIRoot.constLayer;

        public static Canvas UICanvas => Root.Instance.UIRoot.uiCanvas;
        public static Camera UICamera => Root.Instance.UIRoot.uiCamera;

        [SerializeField] private Transform bottomLayer;
        [SerializeField] private Transform normalLayer;
        [SerializeField] private Transform topLayer;
        [SerializeField] private Transform constLayer;

        [SerializeField] private Camera uiCamera;
        [SerializeField] private Canvas uiCanvas;

        public void AddToLayer(Window wnd)
        {
            switch (wnd.Config.Layer)
            {
                case UILayer.Bottom:
                    {
                        wnd.SetParent(bottomLayer);
                        break;
                    }
                case UILayer.Normal:
                    {
                        wnd.SetParent(normalLayer);
                        break;
                    }
                case UILayer.Top:
                    {
                        wnd.SetParent(topLayer);
                        break;
                    }
                case UILayer.Const:
                    {
                        wnd.SetParent(constLayer);
                        break;
                    }
                default:
                    {
                        Log.Error($"UIRoot中无法找到层级`{wnd.Config.Layer}`");
                        break;
                    }
            }
        }

        private void Awake()
        {
            gameObject.name = GetType().Name;
            DontDestroyOnLoad(gameObject);

            InitUICamera();
        }

        private void InitUICamera()
        {
            if (uiCamera == null)
            {
                throw new CommonException("UICamera为空");
            }
            uiCamera.depth = Def.UI_CAMERA_DEPTH;
            uiCamera.cullingMask = Def.UI_CULLING_MASK;
            uiCamera.gameObject.layer = LayerDef.UI;
        }
    }
}