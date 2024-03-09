using ParadoxNotion.Design;
using UnityEditor;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源路径管理
    /// </summary>
    public static class Path
    {
        /// <summary>
        /// 资源路径前缀
        /// </summary>
        public const string ASSET_PATH_PREFIX = "Assets/Res/";

        /// <summary>
        /// Resources资源路径前缀
        /// </summary>
        public const string RES_PATH_PREFIX = Def.FRAME_WORK_NAME + "/";

        /// <summary>
        /// Resources-UI资源路径前缀
        /// </summary>
        public const string RES_PATH_UI_PREFIX = RES_PATH_PREFIX + "UI/";

        /// <summary>
        /// 移除文件类型
        /// </summary>
        public static string RemoveFileType(string path)
        {
            int idx = path.LastIndexOf('.');
            return path.Substring(0, idx);
        }

        /// <summary>
        /// 获取AssetBundle的文件路径
        /// </summary>
        public static string GetAssetBundlePath(string assetBundleName)
        {
            return assetBundleName;
        }
    }
}