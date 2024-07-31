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
        /// 获取RectTransform
        /// </summary>
        public static RectTransform RectTF(this Transform tf)
        {
            return tf as RectTransform;
        }

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
}