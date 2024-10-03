using System;
using UnityEngine;

using UObject = UnityEngine.Object;

namespace Duo1JFramework
{
    /// <summary>
    /// Unity Transform 相关扩展
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
        /// 重置旋转、缩放、坐标
        /// </summary>
        public static void ResetSRT(this GameObject go)
        {
            go.transform.ResetSRT();
        }

        /// <summary>
        /// 重置旋转、缩放、坐标
        /// </summary>
        public static void ResetSRT(this RectTransform rectTf)
        {
            rectTf.transform.ResetSRT();
            rectTf.anchoredPosition = Vector3.zero;
        }

        /// <summary>
        /// 设置父节点
        /// </summary>
        public static void SetParent(this GameObject go, GameObject parent)
        {
            if (parent == null)
            {
                return;
            }

            go.SetParent(parent.transform);
        }

        /// <summary>
        /// 设置父节点
        /// </summary>
        public static void SetParent(this GameObject go, Transform parent)
        {
            if (parent == null)
            {
                return;
            }

            go.transform.SetParent(parent);
        }

        #region Child

        /// <summary>
        /// 设置子物体数量
        /// </summary>
        public static void SetChildCnt(this Transform tf, int cnt, Action<GameObject> foreachAction = null)
        {
            int childCnt = tf.childCount;

            if (childCnt == 0)
            {
                Log.ErrorForce($"{tf.GetNameInsID()} 下未找到任何子节点");
                return;
            }

            Transform childTemplate = tf.GetChild(0);
            if (childCnt < cnt)
            {
                for (int i = 0; i < cnt - childCnt; i++)
                {
                    UObject.Instantiate(childTemplate, tf, true);
                }
            }

            for (int i = 0; i < tf.childCount; i++)
            {
                Transform child = tf.GetChild(i);
                if (i < cnt)
                {
                    child.SetActive(true);
                    foreachAction?.Invoke(child.gameObject);
                }
                else
                {
                    child.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 设置子物体数量
        /// </summary>
        public static void SetChildCnt(this GameObject go, int cnt, Action<GameObject> foreachAction = null)
        {
            go.transform.SetChildCnt(cnt, foreachAction);
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

        /// <summary>
        /// 遍历子节点
        /// </summary>
        public static void ChildForeach(this GameObject go, Action<GameObject> foreachAction)
        {
            go.transform.ChildForeach(foreachAction);
        }

        #endregion Child
    }
}
