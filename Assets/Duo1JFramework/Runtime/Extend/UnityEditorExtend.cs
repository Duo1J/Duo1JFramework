using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Duo1JFramework
{
    /// <summary>
    /// Unity Editor 相关扩展
    /// </summary>
    public static class UnityEditorExtend
    {
        /// <summary>
        /// 记录物体撤销
        /// </summary>
        public static GameObject RecordObject(this GameObject go, string msg)
        {
#if UNITY_EDITOR
            Undo.RecordObject(go, msg);
#else
            Log.ErrorForce($"非编辑器下不可调用GameObject:RecordObject(), {msg}");
#endif
            return go;
        }
    }
}
