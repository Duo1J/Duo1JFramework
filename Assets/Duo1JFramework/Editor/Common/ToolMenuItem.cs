using Duo1JFramework.UI;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

using UObject = UnityEngine.Object;
using Duo1JFramework.Asset;

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器工具栏菜单
    /// </summary>
    public static class ToolMenuItem
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
            FolderFastJumpEditor wnd = EditorWindow.GetWindow<FolderFastJumpEditor>();
            wnd.titleContent = new GUIContent("文件夹快速选中");
            wnd.Show();
            return wnd;
        }

        #endregion Path

        #region EditorStyle 20

        [MenuItem(EditorDef.TOOL_EDITOR_STYLE_PREFIX + "内置图标列表", priority = 20)]
        private static GUIIconViewer OpenGUIIconViewer()
        {
            GUIIconViewer wnd = EditorWindow.GetWindow<GUIIconViewer>("内置图标列表");
            wnd.Show();
            return wnd;
        }

        [MenuItem(EditorDef.TOOL_EDITOR_STYLE_PREFIX + "内置样式列表", priority = 21)]
        public static GUIStyleViewer OpenGUIStyleViewer()
        {
            GUIStyleViewer wnd = EditorWindow.GetWindow<GUIStyleViewer>("内置样式列表");
            wnd.Show();
            return wnd;
        }

        #endregion EditorStyle
    }
}