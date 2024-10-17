namespace Duo1JFramework
{
    public static partial class Def
    {
        /// <summary>
        /// 资源相关定义
        /// </summary>
        public static partial class Asset
        {
            /// <summary>
            /// 最大AssetBundle卸载空闲等待时间
            /// </summary>
            public const float MAX_AB_FREE_TIME = 5;

#if NOT_ENCRYPT_AB_MAP_DATA
            /// <summary>
            /// 加密ABMapData
            /// </summary>
            public static bool EncryptABMapData = false;
#else
            /// <summary>
            /// 加密ABMapData
            /// </summary>
            public static bool EncryptABMapData = false;
#endif

#if NOT_BUILD_AB_CRC
            /// <summary>
            /// 构建AssetBundle的CRC校验
            /// </summary>
            public static bool BuildABCRC = false;
#else
            /// <summary>
            /// 构建AssetBundle的CRC校验
            /// </summary>
            public static bool BuildABCRC = true;
#endif
        }
    }
}
