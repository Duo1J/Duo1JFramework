using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器工具类
    /// </summary>
    public static class EditorUtil
    {
        /// <summary>
        /// 获取选中的Go
        /// </summary>
        public static GameObject GetActiveGo()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null)
            {
                Log.Error("未选中任何GameObject");
            }
            return go;
        }
    }
}