using Duo1JFramework.AnimationAPI;
using Duo1JFramework.Asset;
using Duo1JFramework.Build;
using Duo1JFramework.Net;
using Duo1JFramework.Scheduling;
using Duo1JFramework.PhysicsAPI;
using Duo1JFramework.World;
using System;
using System.Collections.Generic;

namespace Duo1JFramework
{
    public partial class EditorDef
    {
        /// <summary>
        /// 编辑器菜单工具栏定义
        /// </summary>
        public class Menu
        {
            #region 工具栏路径前缀

            #region Framework

            /// <summary>
            /// 框架工具栏
            /// </summary>
            public class Framework
            {
                /// <summary>
                /// 框架工具栏路径前缀
                /// </summary>
                public const string PREFIX = Def.FRAME_WORK_NAME + "/";

                /// <summary>
                /// UI
                /// </summary>
                public const string UI_PREFIX = PREFIX + "UI/";

                /// <summary>
                /// Actor
                /// </summary>
                public const string ACTOR_PREFIX = PREFIX + "Actor/";

                /// <summary>
                /// 路径、名称
                /// </summary>
                public const string PATH_PREFIX = PREFIX + "Path/";

                /// <summary>
                /// 编辑器样式表
                /// </summary>
                public const string EDITOR_STYLE_PREFIX = PREFIX + "EditorStyle/";

                /// <summary>
                /// 数据监视器
                /// </summary>
                public const string MONITOR_PREFIX = PREFIX + "Monitor/";

                /// <summary>
                /// 构建
                /// </summary>
                public const string BUILD_PREFIX = PREFIX + "Build/";

                /// <summary>
                /// 烘焙
                /// </summary>
                public const string BAKE_PREFIX = PREFIX + "Bake/";

                /// <summary>
                /// 动画
                /// </summary>
                public const string ANIMATION_PREFIX = PREFIX + "Animation/";
            }

            #endregion Framework

            #region Assets

            /// <summary>
            /// Assets工具栏路径前缀
            /// </summary>
            public class Assets
            {
                /// <summary>
                /// Assets工具栏路径前缀
                /// </summary>
                public const string PREFIX = "Assets/" + Def.FRAME_WORK_NAME + "/";

                /// <summary>
                /// 引用
                /// </summary>
                public const string REF_PREFIX = PREFIX + "Reference/";

                /// <summary>
                /// 资产检查
                /// </summary>
                public const string ASSET_CHECK = PREFIX + "资产检查";
            }

            #endregion Assets

            #endregion 工具栏路径前缀

            #region 工具窗口名称

            /// <summary>
            /// 获取工具窗口名称
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

            public const string FOLDER_FAST_JUMP_EDITOR_WND = "文件夹快速选中";

            public const string GUI_ICON_VIEWER = "内置图标列表";
            public const string GUI_STYLE_VIEWER = "内置样式列表";
            public const string GUI_COLOR_VIEWER = "GUI颜色调整器";

            public const string TIMER_MONITOR = "计时器监视";
            public const string ASSET_BUNDLE_MONITOR = "AssetBundle监视";
            public const string COLLISION_MONITOR = "碰撞、触发监视";
            public const string WORLD_MONITOR = "世界监视";
            public const string NET_MONITOR = "网络消息监视";

            public const string APP_BUILD_EDITOR_WND = "构建App";
            public const string ASSET_BUNDLE_BUILD_EDITOR_WND = "构建AssetBundle";
            public const string ADDRESSABLES_BUILD_ALL = "全量构建Addressables";

            public const string BAKE_ALL = "全量烘焙";
            public const string LOC_BAKE_CONF = "定位烘焙配置";

            public const string FOOT_IK_CURVE_GENERATOR = "足部IK曲线生成器";

            /// <summary>
            /// 工具窗口名称映射配置
            /// </summary>
            private static readonly Dictionary<Type, string> editorWndNameMap = new Dictionary<Type, string>()
            {
                [typeof(FolderFastJumpEditorWnd)] = FOLDER_FAST_JUMP_EDITOR_WND,

                [typeof(EditorIconViewer)] = GUI_ICON_VIEWER,
                [typeof(EditorStyleViewer)] = GUI_STYLE_VIEWER,
                [typeof(EditorColorViewer)] = GUI_COLOR_VIEWER,

                [typeof(TimerMonitor)] = TIMER_MONITOR,
                [typeof(AssetBundleMonitor)] = ASSET_BUNDLE_MONITOR,
                [typeof(CollisionMonitor)] = COLLISION_MONITOR,
                [typeof(WorldMonitor)] = WORLD_MONITOR,
                [typeof(NetMonitor)] = NET_MONITOR,

                [typeof(AppBuildEditorWnd)] = APP_BUILD_EDITOR_WND,
                [typeof(AssetBundleBuildEditorWnd)] = ASSET_BUNDLE_BUILD_EDITOR_WND,

                [typeof(FootIKCurveGenerator)] = FOOT_IK_CURVE_GENERATOR,
            };

            #endregion 工具窗口名称

            private Menu()
            {
            }
        }
    }
}
