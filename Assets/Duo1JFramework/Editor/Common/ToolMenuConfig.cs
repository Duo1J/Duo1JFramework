using Duo1JFramework.UI;
using UnityEditor;
using UnityEngine;
using Duo1JFramework.Asset;
using Duo1JFramework.Build;
using Duo1JFramework.TimerUpdate;

using UObject = UnityEngine.Object;
using Duo1JFramework.AnimationAPI;

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器工具栏菜单配置
    /// </summary>
    public static class ToolMenuConfig
    {
        #region Path 10

        [MenuItem(EditorDef.Menu.PATH_PREFIX + "UI节点快速命名 &1", priority = 10)]
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

        [MenuItem(EditorDef.Menu.PATH_PREFIX + "复制文件路径(无后缀) &2", priority = 11)]
        public static void CopyProjectFilePath()
        {
            if (EditorUtil.GetActiveObj(out UObject go))
            {
                string path = AssetDatabase.GetAssetPath(go);
                EditorUtil.CopyText(PathUtil.RemoveFileType(path));
            }
        }

        [MenuItem(EditorDef.Menu.PATH_PREFIX + "复制文件路径(带后缀) &3", priority = 12)]
        public static void CopyProjectFilePathWithSuffix()
        {
            if (EditorUtil.GetActiveObj(out UObject go))
            {
                string path = AssetDatabase.GetAssetPath(go);
                EditorUtil.CopyText(path);
            }
        }

        [MenuItem(EditorDef.Menu.PATH_PREFIX + EditorDef.Menu.FOLDER_FAST_JUMP_EDITOR_WND, priority = 13)]
        public static FolderFastJumpEditorWnd OpenFolderFastJumpEditor()
        {
            return FolderFastJumpEditorWnd.Open();
        }

        #endregion Path 10

        #region EditorStyle 20

        [MenuItem(EditorDef.Menu.EDITOR_STYLE_PREFIX + EditorDef.Menu.GUI_ICON_VIEWER, priority = 20)]
        private static GUIIconViewer OpenGUIIconViewer()
        {
            return GUIIconViewer.Open();
        }

        [MenuItem(EditorDef.Menu.EDITOR_STYLE_PREFIX + EditorDef.Menu.GUI_STYLE_VIEWER, priority = 21)]
        public static GUIStyleViewer OpenGUIStyleViewer()
        {
            return GUIStyleViewer.Open();
        }

        [MenuItem(EditorDef.Menu.EDITOR_STYLE_PREFIX + EditorDef.Menu.GUI_COLOR_VIEWER, priority = 22)]
        public static GUIColorViewer OpenGUIColorViewer()
        {
            return GUIColorViewer.Open();
        }

        #endregion EditorStyle 20

        #region Monitor 30

        [MenuItem(EditorDef.Menu.MONITOR_PREFIX + EditorDef.Menu.TIMER_MONITOR, priority = 30)]
        public static TimerMonitor OpenTimerMonitor()
        {
            return TimerMonitor.Open();
        }

        [MenuItem(EditorDef.Menu.MONITOR_PREFIX + EditorDef.Menu.ASSET_BUNDLE_MONITOR, priority = 31)]
        public static AssetBundleMonitor OpenAssetBundleMonitor()
        {
            return AssetBundleMonitor.Open();
        }

        [MenuItem(EditorDef.Menu.MONITOR_PREFIX + EditorDef.Menu.COLLISION_MONITOR, priority = 32)]
        public static CollisionMonitor OpenCollisionMonitor()
        {
            return CollisionMonitor.Open();
        }

        #endregion Monitor 40

        #region Build 40

        [MenuItem(EditorDef.Menu.BUILD_PREFIX + EditorDef.Menu.ASSET_BUNDLE_BUILD_EDITOR_WND, priority = 40)]
        public static AssetBundleBuildEditorWnd OpenAssetBundleBuildEditor()
        {
            return AssetBundleBuildEditorWnd.Open();
        }

        #endregion Build 40

        #region Animation 50

        [MenuItem(EditorDef.Menu.ANIMATION_PREFIX + EditorDef.Menu.FOOT_IK_CURVE_GENERATOR, priority = 50)]
        public static FootIKCurveGenerator OpenFootIKCurveGenerator()
        {
            return FootIKCurveGenerator.Open();
        }

        #endregion Animation 50
    }
}