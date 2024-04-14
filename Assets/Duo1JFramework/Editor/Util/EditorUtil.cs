using System;
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
        /// 获取选中的Obj
        /// </summary>
        public static bool GetActiveObj(out UnityEngine.Object obj, bool nullWarn = true)
        {
            obj = Selection.activeObject;
            if (obj == null)
            {
                if (nullWarn) Log.Error("未选中任何Object");
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

        #region 杂项

        /// <summary>
        /// 打开资源管理器
        /// </summary>
        public static void OpenExplore(string path)
        {
            Log.EditorInfo($"打开文件夹: {path}");
            System.Diagnostics.Process.Start("explorer.exe", path.Replace("/", "\\"));
        }

        /// <summary>
        /// 复制
        /// </summary>
        public static void CopyText(string text)
        {
            TextEditor editor = new TextEditor();
            editor.text = text;
            editor.SelectAll();
            editor.Copy();
            Log.EditorInfo($"已拷贝`{text}`到粘贴板");
        }

        /// <summary>
        /// 编辑器资源编辑
        /// </summary>
        public static void AssetEditing(Action callback)
        {
            try
            {
                Log.EditorInfo("编辑器开始资源编辑");
                AssetDatabase.StartAssetEditing();
                callback?.Invoke();
            }
            finally
            {
                Log.EditorInfo("编辑器结束资源编辑");
                AssetDatabase.StopAssetEditing();
            }
        }

        /// <summary>
        /// 编辑器保存并刷新
        /// </summary>
        public static void SaveAndRefresh()
        {
            Log.EditorInfo("编辑器保存并刷新");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion 杂项 

        #region 窗口

        /// <summary>
        /// 打开编辑器窗口
        /// </summary>
        /// <param name="wndName">窗口名，不填则使用配置的名称</param>
        public static T OpenEditorWnd<T>(string _wndName = null) where T : EditorWindow
        {
            string wndName = string.IsNullOrEmpty(_wndName) ? EditorDef.GetEditorWndName(typeof(T)) : _wndName;

            T wnd = EditorWindow.GetWindow<T>();
            if (!string.IsNullOrEmpty(wndName))
            {
                wnd.titleContent = new GUIContent(wndName);
            }
            wnd.Show();
            return wnd;
        }

        #endregion

        #region ScriptableObject

        /// <summary>
        /// 获取或创建ScriptableObject
        /// </summary>
        public static T GetOrCreateSO<T>(string path) where T : ScriptableObject
        {
            T so = AssetDatabase.LoadAssetAtPath<T>(path);
            if (so == null)
            {
                so = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(so, path);
            }
            return so;
        }

        /// <summary>
        /// 获取或创建编辑器配置ScriptableObject
        /// </summary>
        public static T GetOrCreateEditorCfgSO<T>() where T : ScriptableObject
        {
            string path = GetEditorCfgSOPath<T>();
            return GetOrCreateSO<T>(path);
        }

        /// <summary>
        /// 获取编辑器配置ScriptableObject默认路径
        /// </summary>
        public static string GetEditorCfgSOPath<T>()
        {
            return $"{EditorDef.EDITOR_CONFIG_PATH}/{typeof(T).Name}.asset";
        }

        #endregion ScriptableObject

        #region Build

        /// <summary>
        /// 获取当前构建目标平台
        /// </summary>
        public static BuildTarget GetCurBuildTarget()
        {
            return EditorUserBuildSettings.activeBuildTarget;
        }

        /// <summary>
        /// 获取AB构建选项
        /// </summary>
        public static BuildAssetBundleOptions GetABBuildOptions()
        {
            return BuildAssetBundleOptions.ChunkBasedCompression;
        }

        #endregion Build
    }
}