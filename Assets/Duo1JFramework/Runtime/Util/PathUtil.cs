namespace Duo1JFramework
{
    /// <summary>
    /// 路径工具类
    /// </summary>
    public static class PathUtil
    {
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

        /// <summary>
        /// 获取AssetBundle的文件路径
        /// </summary>
        public static string GetAssetBundlePath(string assetBundleName)
        {
            return $"{GetAssetBundleRoot()}{assetBundleName}";
        }

        /// <summary>
        /// 获取AssetBundle的根文件夹
        /// </summary>
        public static string GetAssetBundleRoot()
        {
            return $"{Def.Path.STREAMING}/{Def.Path.ASSET_BUNDLE_MAIN_NAME}/";
        }

        /// <summary>
        /// 获取AssetBundle的根文件夹meta文件
        /// </summary>
        public static string GetAssetBundleRootMeta()
        {
            return $"{Def.Path.STREAMING}/{Def.Path.ASSET_BUNDLE_MAIN_NAME}{Def.Path.META_SUFFIX}";
        }

        /// <summary>
        /// 获取AB资源映射文件配置位置
        /// </summary>
        public static string GetABMapDataPath()
        {
            return $"{GetAssetBundleRoot()}/{Def.Path.ASSET_BUNDLE_MAP_DATA_NAME}";
        }
    }
}