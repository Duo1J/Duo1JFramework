using Duo1JFramework.AnimationAPI;
using Duo1JFramework.Asset;
using Duo1JFramework.Build;
using Duo1JFramework.Scheduling;
using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace Duo1JFramework
{
    /// <summary>
    /// 框架菜单工具栏
    /// </summary>
    public partial class MenuTool
    {
        #region Path 10

        [MenuItem(EditorDef.Menu.Framework.PATH_PREFIX + "UI节点快速命名 &1", priority = 10)]
        public static void UINodeFastName()
        {
            if (EditorUtil.GetActiveGo(out GameObject go))
            {
                if (!go.name.StartsWith(Def.UI.NODE_PREFIX))
                {
                    go.RecordObject("UI node rename").name = Def.UI.NODE_PREFIX + go.name;
                }
            }
        }

        [MenuItem(EditorDef.Menu.Framework.PATH_PREFIX + "复制文件路径(无后缀) &2", priority = 11)]
        public static void CopyProjectFilePath()
        {
            if (EditorUtil.GetActiveObj(out UObject go))
            {
                string path = AssetDatabase.GetAssetPath(go);
                EditorUtil.CopyText(PathUtil.RemoveTypeSuffix(path));
            }
        }

        [MenuItem(EditorDef.Menu.Framework.PATH_PREFIX + "复制文件路径(带后缀) &3", priority = 12)]
        public static void CopyProjectFilePathWithSuffix()
        {
            if (EditorUtil.GetActiveObj(out UObject go))
            {
                string path = AssetDatabase.GetAssetPath(go);
                EditorUtil.CopyText(path);
            }
        }

        [MenuItem(EditorDef.Menu.Framework.PATH_PREFIX + EditorDef.Menu.FOLDER_FAST_JUMP_EDITOR_WND, priority = 13)]
        public static FolderFastJumpEditorWnd OpenFolderFastJumpEditor()
        {
            return FolderFastJumpEditorWnd.Open();
        }

        #endregion Path 10

        #region EditorStyle 20

        [MenuItem(EditorDef.Menu.Framework.EDITOR_STYLE_PREFIX + EditorDef.Menu.GUI_ICON_VIEWER, priority = 20)]
        private static EditorIconViewer OpenGUIIconViewer()
        {
            return EditorIconViewer.Open();
        }

        [MenuItem(EditorDef.Menu.Framework.EDITOR_STYLE_PREFIX + EditorDef.Menu.GUI_STYLE_VIEWER, priority = 21)]
        public static EditorStyleViewer OpenGUIStyleViewer()
        {
            return EditorStyleViewer.Open();
        }

        [MenuItem(EditorDef.Menu.Framework.EDITOR_STYLE_PREFIX + EditorDef.Menu.GUI_COLOR_VIEWER, priority = 22)]
        public static EditorColorViewer OpenGUIColorViewer()
        {
            return EditorColorViewer.Open();
        }

        #endregion EditorStyle 20

        #region Monitor 30

        [MenuItem(EditorDef.Menu.Framework.MONITOR_PREFIX + EditorDef.Menu.TIMER_MONITOR, priority = 30)]
        public static TimerMonitor OpenTimerMonitor()
        {
            return TimerMonitor.Open();
        }

        [MenuItem(EditorDef.Menu.Framework.MONITOR_PREFIX + EditorDef.Menu.ASSET_BUNDLE_MONITOR, priority = 31)]
        public static AssetBundleMonitor OpenAssetBundleMonitor()
        {
            return AssetBundleMonitor.Open();
        }

        [MenuItem(EditorDef.Menu.Framework.MONITOR_PREFIX + EditorDef.Menu.COLLISION_MONITOR, priority = 32)]
        public static CollisionMonitor OpenCollisionMonitor()
        {
            return CollisionMonitor.Open();
        }

        #endregion Monitor 40

        #region Build 40

        [MenuItem(EditorDef.Menu.Framework.BUILD_PREFIX + EditorDef.Menu.APP_BUILD_EDITOR_WND, priority = 40)]
        public static AppBuildEditorWnd OpenAppBuildEditorWnd()
        {
            return AppBuildEditorWnd.Open();
        }

        [MenuItem(EditorDef.Menu.Framework.BUILD_PREFIX + EditorDef.Menu.ASSET_BUNDLE_BUILD_EDITOR_WND, priority = 41)]
        public static AssetBundleBuildEditorWnd OpenAssetBundleBuildEditorWnd()
        {
            return AssetBundleBuildEditorWnd.Open();
        }

        #endregion Build 40

        #region Animation 50

        [MenuItem(EditorDef.Menu.Framework.ANIMATION_PREFIX + EditorDef.Menu.FOOT_IK_CURVE_GENERATOR, priority = 50)]
        public static FootIKCurveGenerator OpenFootIKCurveGenerator()
        {
            return FootIKCurveGenerator.Open();
        }

        #endregion Animation 50

        private MenuTool()
        {
        }
    }
}
