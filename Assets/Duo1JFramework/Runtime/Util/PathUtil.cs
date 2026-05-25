using System.IO;

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
            return string.IsNullOrEmpty(path) ? path : path.Replace("\\", "/");
        }

        /// <summary>
        /// 组合路径并统一分隔符
        /// </summary>
        public static string Combine(params string[] paths)
        {
            if (paths == null || paths.Length == 0)
            {
                return string.Empty;
            }

            return SplitUnify(Path.Combine(paths));
        }

        /// <summary>
        /// 确保路径以统一分隔符结尾
        /// </summary>
        public static string EnsureTrailingSlash(string path)
        {
            path = SplitUnify(path);
            if (string.IsNullOrEmpty(path) || path.EndsWith("/"))
            {
                return path;
            }

            return $"{path}/";
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
            string fileName = ignoreSuffix ? assetBundleName : $"{assetBundleName}{Def.Path.ASSET_BUNDLE_SUFFIX}";
            return Combine(GetAssetBundleRoot(), fileName);
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
            return EnsureTrailingSlash(Combine(Def.Path.DataPath, "..", Def.Path.ASSET_BUNDLE_BUILD_FOLDER, Def.Path.ASSET_BUNDLE_MAIN_NAME));
        }

        /// <summary>
        /// 获取AssetBundle在运行时的存放根文件夹
        /// </summary>
        public static string GetAssetBundleRuntimeRoot()
        {
            return EnsureTrailingSlash(Combine(Def.Path.Streaming, Def.Path.ASSET_BUNDLE_MAIN_NAME));
        }

        /// <summary>
        /// 获取AssetBundle在运行时的存放根文件夹的meta文件
        /// </summary>
        public static string GetAssetBundleRuntimeRootMeta()
        {
            return Combine(Def.Path.Streaming, $"{Def.Path.ASSET_BUNDLE_MAIN_NAME}{Def.Path.META_SUFFIX}");
        }

        /// <summary>
        /// 获取AB资源映射文件配置位置
        /// </summary>
        public static string GetABMapDataPath()
        {
            return Combine(GetAssetBundleRoot(), Def.Path.ASSET_BUNDLE_MAP_DATA_NAME);
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
