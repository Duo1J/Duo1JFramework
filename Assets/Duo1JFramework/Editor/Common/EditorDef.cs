using Duo1JFramework.Build;
using System;
using System.Collections.Generic;

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器定义
    /// </summary>
    public class EditorDef
    {
        /// <summary>
        /// 工具栏路径前缀
        /// </summary>
        public const string TOOL_PREFIX = Def.FRAME_WORK_NAME + "/";

        #region 工具路径定义

        /// <summary>
        /// UI工具栏路径前缀
        /// </summary>
        public const string TOOL_UI_PREFIX = TOOL_PREFIX + "UI/";

        /// <summary>
        /// Actor工具栏路径前缀
        /// </summary>
        public const string TOOL_ACTOR_PREFIX = TOOL_PREFIX + "Actor/";

        /// <summary>
        /// 路径、名称工具栏路径前缀
        /// </summary>
        public const string TOOL_PATH_PREFIX = TOOL_PREFIX + "Path/";

        /// <summary>
        /// 编辑器样式表
        /// </summary>
        public const string TOOL_EDITOR_STYLE_PREFIX = TOOL_PREFIX + "EditorStyle/";

        /// <summary>
        /// 数据监视器
        /// </summary>
        public const string TOOL_EDITOR_MONITOR_PREFIX = TOOL_PREFIX + "Monitor/";

        /// <summary>
        /// 构建
        /// </summary>
        public const string TOOL_EDITOR_BUILD_PREFIX = TOOL_PREFIX + "Build/";

        #endregion 工具路径定义

        /// <summary>
        /// 编辑器配置路径
        /// </summary>
        public const string EDITOR_CONFIG_PATH = "Assets/" + Def.FRAME_WORK_NAME + "/EditorConfig/";

        #region 编辑器窗口

        /// <summary>
        /// 获取编辑器窗口名称
        /// </summary>
        public static string GetEditorWndName(Type t)
        {
            if (editorWndNameMap == null)
            {
                Log.EditorError("`EditorDef.editorWndNameMap`为空");
                return t.Name;
            }
            if (editorWndNameMap.TryGetValue(t, out string ret))
            {
                return ret;
            }
            Log.EditorWarn($"无法在`EditorDef.editorWndNameMap`中找到`{t.Name}`的标题配置，显示其类型名");
            return t.Name;
        }

        /// <summary>
        /// 编辑器窗口名称映射配置
        /// </summary>
        private static readonly Dictionary<Type, string> editorWndNameMap = new Dictionary<Type, string>()
        {
            [typeof(FolderFastJumpEditor)] = "文件夹快速选中",
            [typeof(GUIIconViewer)] = "内置图标列表",
            [typeof(GUIStyleViewer)] = "内置样式列表",
            [typeof(TimerMonitor)] = "计时器监视",
            [typeof(AssetBundleBuildEditor)] = "构建AssetBundle",
        };

        #endregion 编辑器窗口
    }
}