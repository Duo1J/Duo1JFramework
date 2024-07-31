using Duo1JFramework.UI;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// RectTransform扩展方法
    /// </summary>
    public static class RectTransformExtend
    {
        #region Transform

        /// <summary>
        /// 重置旋转、缩放、坐标
        /// </summary>
        public static void ResetSRT(this RectTransform rectTf)
        {
            rectTf.transform.ResetSRT();
            rectTf.anchoredPosition = Vector3.zero;
        }

        /// <summary>
        /// 铺开Anchor
        /// </summary>
        public static void ExpandAnchor(this RectTransform rectTf)
        {
            rectTf.anchorMin = Vector3.zero;
            rectTf.anchorMax = Vector3.one;
        }

        /// <summary>
        /// 设置Anchored宽度
        /// </summary>
        public static void SetWidth(this RectTransform rectTf, float width)
        {
            rectTf.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }

        /// <summary>
        /// 设置Anchored高度
        /// </summary>
        public static void SetHeight(this RectTransform rectTf, float height)
        {
            rectTf.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, height);
        }

        /// <summary>
        /// 设置Anchored宽高
        /// </summary>
        public static void SetRectSize(this RectTransform rectTf, float width, float height)
        {
            rectTf.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rectTf.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, height);
        }

        #endregion Transform

        /// <summary>
        /// 绑定可拖拽面板
        /// </summary>
        public static void BindDragPanel(this RectTransform rectTf)
        {
            UIDragPanel.Bind(rectTf);
        }
    }
}
