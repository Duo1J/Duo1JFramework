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
            /// 资源路径前缀
            /// </summary>
            public const string ASSET_PATH_PREFIX = "Assets/Res/";

            /// <summary>
            /// 资源全路径前缀
            /// </summary>
            public static string ASSET_FULL_PATH_PREFIX = $"{DATA_PATH}/Res/";

            /// <summary>
            /// Resources资源路径前缀
            /// </summary>
            public const string RES_PATH_PREFIX = Def.FRAME_WORK_NAME + "/";

            /// <summary>
            /// AssetBundle主包名称
            /// </summary>
            public const string ASSET_BUNDLE_MAIN_NAME = "Bundle";

            /// <summary>
            /// AssetBundle映射文件名
            /// </summary>
            public const string ASSET_BUNDLE_MAP_DATA_NAME = "ABMapData.json";
        }
    }
}