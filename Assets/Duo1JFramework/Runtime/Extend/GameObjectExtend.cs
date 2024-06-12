using System;
using UnityEditor;
using UnityEngine;

using UObject = UnityEngine.Object;

namespace Duo1JFramework
{
    /// <summary>
    /// GameObject扩展方法
    /// </summary>
    public static class GameObjectExtend
    {
        /// <summary>
        /// 获取或添加组件
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T com = go.GetComponent<T>();
            if (com == null) com = go.AddComponent<T>();
            return com;
        }

        /// <summary>
        /// 获取并断言组件
        /// </summary>
        public static T GetAndAssertComponent<T>(this GameObject go, string msg = null)
        {
            if (msg == null)
            {
                msg = $"`{go.name}-{go.GetInstanceID()}` 未持有组件 `{typeof(T).FullName}`";
            }

            T ret = go.GetComponent<T>();
            Assert.NotNull(ret, msg);
            return ret;
        }

        #region Transform

        /// <summary>
        /// 重置旋转、缩放、坐标
        /// </summary>
        public static void ResetSRT(this GameObject go)
        {
            go.transform.ResetSRT();
        }

        /// <summary>
        /// 设置父节点
        /// </summary>
        public static void SetParent(this GameObject go, Transform parent)
        {
            go.transform.SetParent(parent);
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
        /// 设置子物体数量
        /// </summary>
        public static void SetChildCnt(this GameObject go, int cnt, Action<GameObject> foreachAction = null)
        {
            int childCnt = go.transform.childCount;

            if (childCnt == 0)
            {
                Log.ErrorForce($"`{go.name}` 下未找到任何子节点");
                return;
            }

            Transform childTemplate = go.transform.GetChild(0);
            if (childCnt < cnt)
            {
                for (int i = 0; i < cnt - childCnt; i++)
                {
                    UObject.Instantiate(childTemplate, go.transform, true);
                }
            }

            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform child = go.transform.GetChild(i);
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

        #endregion Transform

        #region UI

        /// <summary>
        /// 设置CanvasGroup的Alpha值
        /// </summary>
        public static void SetCanvasGroupAlpha(this GameObject go, float alpha)
        {
            go.GetOrAddComponent<CanvasGroup>().alpha = alpha;
        }

        #endregion UI

        #region Editor

        /// <summary>
        /// 记录物体撤销
        /// </summary>
        /// <param name="go"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static GameObject RecordObject(this GameObject go, string msg)
        {
#if UNITY_EDITOR
            Undo.RecordObject(go, msg);
#else
            Log.ErrorForce($"非编辑器下不可调用GameObject:RecordObject(), {msg}");
#endif
            return go;
        }

        #endregion Editor
    }
}