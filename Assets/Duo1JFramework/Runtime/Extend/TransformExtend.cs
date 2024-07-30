using Duo1JFramework.UI;
using System;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// Transform扩展方法
    /// </summary>
    public static class TransformExtend
    {
        /// <summary>
        /// 重置旋转、缩放、坐标
        /// </summary>
        public static void ResetSRT(this Transform tf)
        {
            tf.transform.localScale = Vector3.one;
            tf.transform.localEulerAngles = Vector3.zero;
            tf.transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// 设置子物体数量
        /// </summary>
        public static void SetChildCnt(this Transform tf, int cnt, Action<GameObject> foreachAction = null)
        {
            tf.gameObject.SetChildCnt(cnt, foreachAction);
        }

        /// <summary>
        /// 遍历子节点
        /// </summary>
        public static void ChildForeach(this Transform tf, Action<GameObject> foreachAction)
        {
            Assert.NotNull(foreachAction, "迭代函数不可为空");

            for (int i = 0; i < tf.childCount; ++i)
            {
                foreachAction(tf.GetChild(i).gameObject);
            }
        }
    }

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