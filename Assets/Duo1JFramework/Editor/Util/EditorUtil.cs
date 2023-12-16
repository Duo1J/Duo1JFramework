using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
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