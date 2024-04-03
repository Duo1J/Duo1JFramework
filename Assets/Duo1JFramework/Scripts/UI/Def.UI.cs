using Duo1JFramework.Asset;
using Duo1JFramework.Config;

namespace Duo1JFramework
{
    /// <summary>
    /// UI定义
    /// </summary>
    public static partial class Def
    {
        /// <summary>
        /// UIRoot路径
        /// </summary>
        public const string UI_ROOT_PATH = Path.RES_PATH_PREFIX + "UI/UIRoot";

        /// <summary>
        /// 每个UI界面的层级差
        /// </summary>
        public const int UI_STEP_LAYER = 50;

        /// <summary>
        /// UI相机深度
        /// </summary>
        public const int UI_CAMERA_DEPTH = 100;

        /// <summary>
        /// UI裁剪遮罩
        /// </summary>
        public const int UI_CULLING_MASK = 1 << LayerDef.UI;
    }
}