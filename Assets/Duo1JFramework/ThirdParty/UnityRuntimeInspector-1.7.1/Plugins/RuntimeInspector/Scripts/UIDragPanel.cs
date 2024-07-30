using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeInspectorNamespace
{
    /// <summary>
    /// UI可拖拽面板
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    internal class UIDragPanel : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        /// <summary>
        /// 动态绑定拖拽面板
        /// </summary>
        public static UIDragPanel Bind(RectTransform rectTF)
        {
            UIDragPanel uiDragPanel = rectTF.GetComponent<UIDragPanel>();
            if (uiDragPanel == null)
            {
                uiDragPanel = rectTF.gameObject.AddComponent<UIDragPanel>();
            }

            return uiDragPanel;
        }

        /// <summary>
        /// 当前拖拽面板的根节点，一般是Canvas
        /// </summary>
        private RectTransform canvasRectTF;

        private Canvas rootCanvas;
        private Camera uiCamera;

        private bool allowDrag;

        private Vector3 mouseDownPos;
        private Vector3 oriPanelPos;

        private void Awake()
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
                Debug.LogError($"{name}未找到RootCanvas，不可拖拽");
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