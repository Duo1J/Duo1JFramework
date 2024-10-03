using Duo1JFramework.UI;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// Unity RectTransform 相关扩展
    /// </summary>
    public static class RectTransformExtend
    {
        #region RectTransform

        /// <summary>
        /// 获取RectTransform
        /// </summary>
        public static RectTransform RectTF(this Transform tf)
        {
            return tf as RectTransform;
        }

        /// <summary>
        /// 获取RectTransform
        /// </summary>
        public static RectTransform RectTF(this GameObject go)
        {
            return go.transform.RectTF();
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

        #endregion RectTransform

        /// <summary>
        /// 绑定可拖拽面板
        /// </summary>
        public static void BindDragPanel(this RectTransform rectTf)
        {
            UIDragPanel.Bind(rectTf);
        }
    }
}
