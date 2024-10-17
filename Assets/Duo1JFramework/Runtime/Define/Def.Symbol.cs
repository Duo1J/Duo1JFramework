namespace Duo1JFramework
{
    public partial class Def
    {
        /// <summary>
        /// 程序符号定义
        /// </summary>
        public class Symbol
        {
            /// <summary>
            /// 启用Profile分析
            /// </summary>
            public const string ENABLE_PROFILE = "ENABLE_PROFILE";

            /// <summary>
            /// 不加密ABMapData
            /// </summary>
            public const string NOT_ENCRYPT_AB_MAP_DATA = "NOT_ENCRYPT_AB_MAP_DATA";

            /// <summary>
            /// 不构建AssetBundle的CRC校验
            /// </summary>
            public const string NOT_BUILD_AB_CRC = "NOT_BUILD_AB_CRC";

            private Symbol()
            {
            }
        }
    }
}
