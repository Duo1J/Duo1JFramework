using Duo1JFramework.Build;

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
            /// 是否加密ABMapData
            /// </summary>
            public static bool EncryptABMapData = false;
#else
            /// <summary>
            /// 是否加密ABMapData
            /// </summary>
            public static bool EncryptABMapData = true;
#endif

#if NOT_BUILD_AB_CRC
            /// <summary>
            /// 是否构建AssetBundle的CRC校验
            /// </summary>
            public static bool BuildABCRC = false;
#else
            /// <summary>
            /// 是否构建AssetBundle的CRC校验
            /// </summary>
            public static bool BuildABCRC = true;
#endif

            /// <summary>
            /// AssetBundle文件命名方式
            /// </summary>
            public static EABNameType ABNameType => abNameType;

            private const EABNameType abNameType = EABNameType.MD5;
        }
    }
}
