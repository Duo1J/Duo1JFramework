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
    }

    /// <summary>
    /// RectTransform扩展方法
    /// </summary>
    public static class RectTransformExtend
    {
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
    }
}