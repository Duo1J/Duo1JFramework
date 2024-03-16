using UnityEngine;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源路径管理
    /// </summary>
    public static class Path
    {
        public static string Streaming => Application.streamingAssetsPath;

        public static string Persistent => Application.persistentDataPath;

        public static string DataPath => Application.dataPath;

        /// <summary>
        /// 资源路径前缀
        /// </summary>
        public const string ASSET_PATH_PREFIX = "Assets/Res/";

        /// <summary>
        /// 资源全路径前缀
        /// </summary>
        public static string ASSET_FULL_PATH_PREFIX = $"{DataPath}/Res/";

        /// <summary>
        /// Resources资源路径前缀
        /// </summary>
        public const string RES_PATH_PREFIX = Def.FRAME_WORK_NAME + "/";

        /// <summary>
        /// Resources-UI资源路径前缀
        /// </summary>
        public const string RES_PATH_UI_PREFIX = RES_PATH_PREFIX + "UI/";

        #region Util

        /// <summary>
        /// 矫正文件路径
        /// </summary>
        public static string CorrectPath(string path)
        {
            return path.Replace("\\", "/");
        }

        /// <summary>
        /// 移除文件类型
        /// </summary>
        public static string RemoveFileType(string path)
        {
            int idx = path.LastIndexOf('.');
            return path.Substring(0, idx);
        }

        #endregion Util

        #region AssetBundle

        /// <summary>
        /// 获取AssetBundle的文件路径
        /// </summary>
        public static string GetAssetBundlePath(string assetBundleName)
        {
            return $"{GetAssetBundleRoot()}{assetBundleName}.assetbundle";
        }

        /// <summary>
        /// 获取AssetBundle的根文件夹
        /// </summary>
        public static string GetAssetBundleRoot()
        {
            return $"{Streaming}/Bundle/";
        }

        /// <summary>
        /// 获取AB资源映射文件配置位置
        /// </summary>
        public static string GetABMapDataPath()
        {
            return $"{GetAssetBundleRoot()}/ABMapData.json";
        }

        #endregion AssetBundle
    }
}