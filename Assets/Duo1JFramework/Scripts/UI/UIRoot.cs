using UnityEngine;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI物体根节点
    /// </summary>
    [DisallowMultipleComponent]
    public class UIRoot : MonoBehaviour
    {
        public Transform bottomLayer;
        public Transform normalLayer;
        public Transform topLayer;
        public Transform constLayer;

        public Camera uiCamera;

        public void AddToLayer(Window wnd)
        {
            switch (wnd.Config.layer)
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
                        Log.Error($"UIRoot中无法找到层级`{wnd.Config.layer}`");
                        break;
                    }
            }
        }
    }
}