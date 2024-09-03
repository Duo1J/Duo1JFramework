using UnityEngine;

namespace Duo1JFramework
{
    public static partial class Def
    {
        /// <summary>
        /// 路径定义
        /// </summary>
        public static partial class Path
        {
            public static string STREAMING => Application.streamingAssetsPath;

            public static string PERSISTENT => Application.persistentDataPath;

            public static string DATA_PATH => Application.dataPath;

            /// <summary>
            /// meta文件后缀
            /// </summary>
            public const string META_SUFFIX = ".meta";

            /// <summary>
            /// Manifest文件后缀
            /// </summary>
            public const string MANIFEST_SUFFIX = ".manifest";

            /// <summary>
            /// 资源根文件夹名
            /// </summary>
            public const string ASSET_ROOT_FOLDER = "Res";

            /// <summary>
            /// 资源路径前缀
            /// </summary>
            public static readonly string ASSET_PATH_PREFIX = $"Assets/{ASSET_ROOT_FOLDER}/";

            /// <summary>
            /// 资源全路径前缀
            /// </summary>
            public static readonly string ASSET_FULL_PATH_PREFIX = $"{DATA_PATH}/{ASSET_ROOT_FOLDER}/";

            /// <summary>
            /// 内部Resources资源路径前缀
            /// </summary>
            public const string RES_PATH_PREFIX = FRAME_WORK_NAME + "/";

            /// <summary>
            /// AssetBundle主包、根文件夹名称
            /// </summary>
            public const string ASSET_BUNDLE_MAIN_NAME = "Bundle";

            /// <summary>
            /// AssetBundle构建文件夹名
            /// </summary>
            public const string ASSET_BUNDLE_BUILD_FOLDER = "AssetBundleBuild";

            /// <summary>
            /// AssetBundle映射文件名
            /// </summary>
            public const string ASSET_BUNDLE_MAP_DATA_NAME = "Data.dat";
        }
    }
}