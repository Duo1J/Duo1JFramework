using Duo1JFramework.UI;
using UnityEditor;
using UnityEngine;
using Duo1JFramework.Asset;
using Duo1JFramework.Build;
using Duo1JFramework.TimerUpdate;

using UObject = UnityEngine.Object;

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器工具栏菜单配置
    /// </summary>
    public static class ToolMenuConfig
    {
        #region Path 10

        [MenuItem(EditorDef.TOOL_PATH_PREFIX + "UI节点快速命名 &1", priority = 10)]
        public static void UINodeFastName()
        {
            if (EditorUtil.GetActiveGo(out GameObject go))
            {
                if (!go.name.StartsWith(UIController.NodePrefix))
                {
                    go.RecordObject("UI node rename").name = UIController.NodePrefix + go.name;
                }
            }
        }

        [MenuItem(EditorDef.TOOL_PATH_PREFIX + "复制文件路径(无后缀) &2", priority = 11)]
        public static void CopyProjectFilePath()
        {
            if (EditorUtil.GetActiveObj(out UObject go))
            {
                string path = AssetDatabase.GetAssetPath(go);
                EditorUtil.CopyText(Path.RemoveFileType(path));
            }
        }

        [MenuItem(EditorDef.TOOL_PATH_PREFIX + "复制文件路径(带后缀) &3", priority = 12)]
        public static void CopyProjectFilePathWithSuffix()
        {
            if (EditorUtil.GetActiveObj(out UObject go))
            {
                string path = AssetDatabase.GetAssetPath(go);
                EditorUtil.CopyText(path);
            }
        }

        [MenuItem(EditorDef.TOOL_PATH_PREFIX + "文件夹快速选中", priority = 13)]
        public static FolderFastJumpEditor OpenFolderFastJumpEditor()
        {
            return FolderFastJumpEditor.Open();
        }

        #endregion Path

        #region EditorStyle 20

        [MenuItem(EditorDef.TOOL_EDITOR_STYLE_PREFIX + "内置图标列表", priority = 20)]
        private static GUIIconViewer OpenGUIIconViewer()
        {
            return GUIIconViewer.Open();
        }

        [MenuItem(EditorDef.TOOL_EDITOR_STYLE_PREFIX + "内置样式列表", priority = 21)]
        public static GUIStyleViewer OpenGUIStyleViewer()
        {
            return GUIStyleViewer.Open();
        }

        #endregion EditorStyle

        #region Monitor 30

        [MenuItem(EditorDef.TOOL_EDITOR_MONITOR_PREFIX + "计时器监视", priority = 30)]
        public static TimerMonitor OpenTimerMonitor()
        {
            return TimerMonitor.Open();
        }

        [MenuItem(EditorDef.TOOL_EDITOR_MONITOR_PREFIX + "AssetBundle监视", priority = 31)]
        public static AssetBundleMonitor OpenAssetBundleMonitor()
        {
            return AssetBundleMonitor.Open();
        }

        #endregion Monitor

        #region Build 40

        [MenuItem(EditorDef.TOOL_EDITOR_BUILD_PREFIX + "构建AssetBundle", priority = 40)]
        public static AssetBundleBuildEditor OpenAssetBundleBuildEditor()
        {
            return AssetBundleBuildEditor.Open();
        }

        #endregion Build
    }
}