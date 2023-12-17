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