using System.Text;

namespace Duo1JFramework
{
    /// <summary>
    /// 全局定义
    /// </summary>
    public static partial class Def
    {
        /// <summary>
        /// 框架名
        /// </summary>
        public const string FRAME_WORK_NAME = "Duo1JFramework";

        #region Crypto

        /// <summary>
        /// AES加密私钥
        /// </summary>
        public static string AesKey => AES_KEY;

        private const string AES_KEY = "Duo1JFrameworkAK";

        /// <summary>
        /// AES加密私钥byte数组
        /// </summary>
        public static byte[] AesKeyByte => AES_KEY_BYTE;

        private static readonly byte[] AES_KEY_BYTE = Encoding.UTF8.GetBytes(AesKey);

        #endregion Crypto
    }
}
