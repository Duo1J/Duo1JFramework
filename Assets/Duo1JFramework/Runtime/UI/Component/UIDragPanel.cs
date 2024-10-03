using UnityEngine;
using UnityEngine.EventSystems;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI可拖拽面板
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UIDragPanel : BaseMono, IBeginDragHandler, IDragHandler
    {
        /// <summary>
        /// 动态绑定拖拽面板
        /// </summary>
        public static UIDragPanel Bind(RectTransform rectTF)
        {
            return rectTF.GetOrAddComponent<UIDragPanel>();
        }

        /// <summary>
        /// 面板根节点
        /// </summary>
        private RectTransform canvasRectTF;

        /// <summary>
        /// 面板根Canvas
        /// </summary>
        private Canvas rootCanvas;

        /// <summary>
        /// UI相机
        /// </summary>
        private Camera uiCamera;

        /// <summary>
        /// 是否允许拖拽
        /// </summary>
        private bool allowDrag;

        /// <summary>
        /// 鼠标按下坐标
        /// </summary>
        private Vector3 mouseDownPos;

        /// <summary>
        /// 面板原始坐标
        /// </summary>
        private Vector3 oriPanelPos;

        private void Start()
        {
            rootCanvas = GetComponentInParent<Canvas>();
            canvasRectTF = rootCanvas.transform as RectTransform;
            if (rootCanvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                uiCamera = rootCanvas.worldCamera;
            }

            allowDrag = rootCanvas != null;
            if (!allowDrag)
            {
                Debug.LogError($"{ToString()}未找到RootCanvas，不可拖拽");
                enabled = false;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!allowDrag)
            {
                return;
            }

            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvasRectTF,
                Input.mousePosition,
                uiCamera,
                out mouseDownPos);
            oriPanelPos = transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!allowDrag)
            {
                return;
            }

            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvasRectTF,
                Input.mousePosition,
                uiCamera,
                out Vector3 curMousePos);
            transform.position = oriPanelPos + (curMousePos - mouseDownPos);
        }
    }
}
