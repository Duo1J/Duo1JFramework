using UnityEditor;

namespace Duo1JFramework
{
    /// <summary>
    /// 路径工具类
    /// </summary>
    public static class PathUtil
    {
        /// <summary>
        /// 统一路径分隔符
        /// </summary>
        public static string UnifySplit(string path)
        {
            return path.Replace("\\", "/");
        }

        /// <summary>
        /// 移除文件类型后缀
        /// </summary>
        public static string RemoveTypeSuffix(string path)
        {
            int idx = path.LastIndexOf('.');
            return path.Substring(0, idx);
        }

        /// <summary>
        /// 获取加载AssetBundle的文件路径
        /// </summary>
        public static string GetAssetBundlePath(string assetBundleName)
        {
            return $"{GetAssetBundleRoot()}{assetBundleName}";
        }

        /// <summary>
        /// 获取加载AssetBundle的根文件夹
        /// </summary>
        public static string GetAssetBundleRoot()
        {
#if UNITY_EDITOR
            return GetAssetBundleEditorRoot();
#else
            return GetAssetBundleRuntimeRoot();
#endif
        }

        /// <summary>
        /// 获取AssetBundle在编辑器下的构建根文件夹
        /// </summary>
        public static string GetAssetBundleEditorRoot()
        {
            return $"{Def.Path.DATA_PATH}/../{Def.Path.ASSET_BUNDLE_BUILD_FOLDER}/{Def.Path.ASSET_BUNDLE_MAIN_NAME}/";
        }

        /// <summary>
        /// 获取AssetBundle在运行时的存放根文件夹
        /// </summary>
        public static string GetAssetBundleRuntimeRoot()
        {
            return $"{Def.Path.STREAMING}/{Def.Path.ASSET_BUNDLE_MAIN_NAME}/";
        }

        /// <summary>
        /// 获取AssetBundle在运行时的存放根文件夹的meta文件
        /// </summary>
        public static string GetAssetBundleRuntimeRootMeta()
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