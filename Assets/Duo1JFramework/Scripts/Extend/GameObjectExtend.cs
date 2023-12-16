using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// GameObject扩展方法
    /// </summary>
    public static class GameObjectExtend
    {
        /// <summary>
        /// 重置旋转、缩放、坐标
        /// </summary>
        public static void ResetSRT(this GameObject go)
        {
            go.transform.ResetSRT();
        }

        public static T GetAndAssertComponent<T>(this GameObject go, string msg = null)
        {
            T ret = go.GetComponent<T>();
            Assert.NotNull(ret, msg);
            return ret;
        }

        /// <summary>
        /// 获取或添加MB组件
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject go) where T : MonoBehaviour
        {
            T com = go.GetComponent<T>();
            if (com == null) com = go.AddComponent<T>();
            return com;
        }

        #region Editor

        #endregion Editor
    }
}