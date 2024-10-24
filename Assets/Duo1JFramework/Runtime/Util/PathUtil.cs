namespace Duo1JFramework
{
    /// <summary>
    /// 路径工具类
    /// </summary>
    public class PathUtil
    {
        /// <summary>
        /// 统一路径分隔符
        /// </summary>
        /// <see cref="StringExtend.SplitUnify(string)"/>
        public static string SplitUnify(string path)
        {
            return path.Replace("\\", "/");
        }

        /// <summary>
        /// 移除文件类型后缀
        /// </summary>
        /// <see cref="StringExtend.RemoveTypeSuffix(string)"/>
        public static string RemoveTypeSuffix(string path)
        {
            int idx = path.LastIndexOf('.');
            return path.Substring(0, idx);
        }

        /// <summary>
        /// 获取加载AssetBundle的文件路径
        /// </summary>
        public static string GetAssetBundlePath(string assetBundleName, bool ignoreSuffix = false)
        {
            if (ignoreSuffix)
            {
                return $"{GetAssetBundleRoot()}{assetBundleName}";
            }
            else
            {
                return $"{GetAssetBundleRoot()}{assetBundleName}{Def.Path.ASSET_BUNDLE_SUFFIX}";
            }
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
            return $"{Def.Path.DataPath}/../{Def.Path.ASSET_BUNDLE_BUILD_FOLDER}/{Def.Path.ASSET_BUNDLE_MAIN_NAME}/";
        }

        /// <summary>
        /// 获取AssetBundle在运行时的存放根文件夹
        /// </summary>
        public static string GetAssetBundleRuntimeRoot()
        {
            return $"{Def.Path.Streaming}/{Def.Path.ASSET_BUNDLE_MAIN_NAME}/";
        }

        /// <summary>
        /// 获取AssetBundle在运行时的存放根文件夹的meta文件
        /// </summary>
        public static string GetAssetBundleRuntimeRootMeta()
        {
            return $"{Def.Path.Streaming}/{Def.Path.ASSET_BUNDLE_MAIN_NAME}{Def.Path.META_SUFFIX}";
        }

        /// <summary>
        /// 获取AB资源映射文件配置位置
        /// </summary>
        public static string GetABMapDataPath()
        {
            return $"{GetAssetBundleRoot()}/{Def.Path.ASSET_BUNDLE_MAP_DATA_NAME}";
        }

        /// <summary>
        /// 统一AssetBundle名为全小写
        /// </summary>
        public static string ABNameUnify(string abName)
        {
            if (string.IsNullOrEmpty(abName))
            {
                return "";
            }

            return abName.ToLower();
        }

        private PathUtil()
        {
        }
    }
}
