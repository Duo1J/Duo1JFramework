using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器工具类
    /// </summary>
    public static class EditorUtil
    {
        #region 选中

        /// <summary>
        /// 获取选中的Go
        /// </summary>
        public static bool GetActiveGo(out GameObject go, bool nullWarn = true)
        {
            go = Selection.activeGameObject;
            if (go == null)
            {
                if (nullWarn) Log.Error("未选中任何GameObject");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 设置选中Go为父节点
        /// </summary>
        public static void SetParentToActiveGo(GameObject go, bool worldPositionStays = false)
        {
            if (GetActiveGo(out GameObject parGo, false))
            {
                go.transform.SetParent(parGo.transform, worldPositionStays);
            }
        }

        /// <summary>
        /// 设置为选中物体
        /// </summary>
        public static void SetActiveGo(GameObject go)
        {
            Selection.activeObject = go;
        }

        #endregion 选中
    }
}