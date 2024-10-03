using UnityEngine;

namespace Duo1JFramework
{
    public static partial class Def
    {
        /// <summary>
        /// UI定义
        /// </summary>
        public static partial class UI
        {
            /// <summary>
            /// UIRoot的Resources路径
            /// </summary>
            public const string ROOT_PATH = Path.RES_PATH_PREFIX + "UI/_UIRoot";

            /// <summary>
            /// UIRoot默认位置
            /// </summary>
            public static readonly Vector3 ROOT_DEFAULT_POS = new Vector3(0, -1000, 0);

            /// <summary>
            /// UI移动到远处的坐标，用以处理全屏策略
            /// </summary>
            public static readonly Vector2 FAR_POS = new Vector2(-10000, -10000);

            /// <summary>
            /// 每个UI界面的层级差
            /// </summary>
            public const int STEP_LAYER = 50;

            /// <summary>
            /// UI相机深度
            /// </summary>
            public const int CAMERA_DEPTH = 100;

            /// <summary>
            /// UI裁剪遮罩
            /// </summary>
            public const int CULLING_MASK = Def.LayerMask.UI;

            /// <summary>
            /// UI节点标记前缀
            /// </summary>
            public const string NODE_PREFIX = "@_";
        }
    }
}
