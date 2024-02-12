using System;
using System.Diagnostics;
using System.Reflection;
using UnityEditor;

using UObject = UnityEngine.Object;

namespace Duo1JFramework
{
    /// <summary>
    /// Project视图工具
    /// </summary>
    public static class ProjectViewUtil
    {
        /// <summary>
        /// 聚焦到Project窗口
        /// </summary>
        public static void FocusProjectView()
        {
            Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));
            Type projectBrowserType = editorAssembly.GetType("UnityEditor.ProjectBrowser");
            EditorWindow.GetWindow(projectBrowserType);
        }

        /// <summary>
        /// 选中Project资源
        /// </summary>
        public static void SelectProjectAsset(string path)
        {
            Assert.NotNullOrEmpty(path, "待选中Project资源路径为空");
            FocusProjectView();
            UObject obj = AssetDatabase.LoadAssetAtPath<UObject>(path);
            if (obj != null)
            {
                EditorGUIUtility.PingObject(obj);
                Selection.activeObject = obj;
            }
            else
            {
                Log.EditorError($"未找到路径`{path}`的资源");
            }
        }

        /// <summary>
        /// 打开系统资源管理器
        /// </summary>
        public static void OpenExplorer(string path)
        {
            Assert.NotNullOrEmpty(path, "待打开系统资源管理器路径为空");
            System.Diagnostics.Process.Start("explorer.exe", path);
        }
    }
}