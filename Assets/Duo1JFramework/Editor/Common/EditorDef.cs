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

        #endregion 工具路径定义

        /// <summary>
        /// 编辑器配置路径
        /// </summary>
        public const string EDITOR_CONFIG_PATH = "Assets/" + Def.FRAME_WORK_NAME + "/EditorConfig/";
    }
}