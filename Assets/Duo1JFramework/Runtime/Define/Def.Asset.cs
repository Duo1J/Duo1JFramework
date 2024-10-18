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

            /// <summary>
            /// 是否加密ABMapData
            /// </summary>
            public static bool EncryptABMapData = true;

            /// <summary>
            /// 是否构建AssetBundle的CRC校验
            /// </summary>
            public static bool BuildABCRC = true;

            /// <summary>
            /// AssetBundle文件命名方式
            /// </summary>
            public static EABNameType ABNameType { get; set; } = EABNameType.MD5;
        }
    }
}
