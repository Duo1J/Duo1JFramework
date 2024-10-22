using Duo1JFramework.ObjectPool;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Duo1JFramework
{
    /// <summary>
    /// 加密、编码工具类
    /// </summary>
    public class CryptoUtil
    {
        #region AES

        /// <summary>
        /// AES加密, 输入流
        /// </summary>
        public static byte[] AesEncrypt(ICryptoTransform cryptoTransform, Stream inputStream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    byte[] buffer = new byte[1024];
                    int len;
                    while ((len = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        cryptoStream.Write(buffer, 0, len);
                    }

                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }

        /// <summary>
        /// AES加密, 输入流
        /// </summary>
        /// <param name="iv">初始向量，若为空则会随机生成，并放在头部</param>
        public static byte[] AesEncrypt(Stream inputStream, byte[] key, byte[] iv = null)
        {
            using (Aes aes = Aes.Create())
            {
                if (iv == null)
                {
                    iv = new byte[aes.BlockSize / 8];
                    using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                    {
                        rng.GetBytes(iv);
                    }
                }

                aes.Key = key;
                aes.IV = iv;

                byte[] encrypted = AesEncrypt(aes.CreateEncryptor(), inputStream);
                byte[] result = new byte[iv.Length + encrypted.Length];
                Array.Copy(iv, 0, result, 0, iv.Length);
                Array.Copy(encrypted, 0, result, iv.Length, encrypted.Length);

                return result;
            }
        }

        /// <summary>
        /// AES加密, 输入文件路径
        /// </summary>
        /// <param name="iv">初始向量，若为空则会随机生成，并放在头部</param>
        public static byte[] AesEncrypt(string filePath, byte[] key, byte[] iv = null)
        {
            filePath.GuardFile();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return AesEncrypt(fs, key, iv);
            }
        }

        /// <summary>
        /// AES加密, 输入byte[]
        /// </summary>
        /// <param name="iv">初始向量，若为空则会随机生成，并放在头部</param>
        public static byte[] AesEncrypt(byte[] bytes, byte[] key, byte[] iv = null)
        {
            Assert.NotNullArg(bytes, "bytes");

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return AesEncrypt(ms, key, iv);
            }
        }

        /// <summary>
        /// AES解密, 输入流
        /// </summary>
        public static byte[] AesDecrypt(ICryptoTransform cryptoTransform, Stream inputStream)
        {
            using (CryptoStream cryptoStream = new CryptoStream(inputStream, cryptoTransform, CryptoStreamMode.Read))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    cryptoStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        /// <summary>
        /// AES解密, 输入流
        /// </summary>
        /// <param name="iv">初始向量，若为空则会从头部读取</param>
        public static byte[] AesDecrypt(Stream inputStream, byte[] key, byte[] iv = null)
        {
            using (Aes aes = Aes.Create())
            {
                if (iv == null)
                {
                    iv = new byte[aes.BlockSize / 8];
                    if (inputStream.Read(iv, 0, iv.Length) != iv.Length)
                    {
                        throw Except.Create("读取IV失败");
                    }
                }

                aes.Key = key;
                aes.IV = iv;

                return AesDecrypt(aes.CreateDecryptor(), inputStream);
            }
        }

        /// <summary>
        /// AES解密, 输入文件路径
        /// </summary>
        /// <param name="iv">初始向量，若为空则会从头部读取</param>
        public static byte[] AesDecrypt(string filePath, byte[] key, byte[] iv = null)
        {
            filePath.GuardFile();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return AesDecrypt(fs, key, iv);
            }
        }

        /// <summary>
        /// AES解密, 输入byte[]
        /// </summary>
        /// <param name="iv">初始向量，若为空则会从头部读取</param>
        public static byte[] AesDecrypt(byte[] bytes, byte[] key, byte[] iv = null)
        {
            Assert.NotNullArg(bytes, "bytes");

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return AesDecrypt(ms, key, iv);
            }
        }

        #endregion AES

        #region MD5

        public static readonly MD5 MD5 = MD5.Create();

        public static byte[] MD5ComputeHash(byte[] bytes)
        {
            return MD5.ComputeHash(bytes);
        }

        public static byte[] MD5ComputeHash(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return MD5ComputeHash(bytes);
        }

        public static byte[] MD5ComputeHash(Stream stream)
        {
            return MD5.ComputeHash(stream);
        }

        public static string MD5ComputeHashStr(byte[] bytes)
        {
            byte[] md5Hash = MD5ComputeHash(bytes);
            return MD5HashToString(md5Hash);
        }

        public static string MD5ComputeHashStr(string str)
        {
            byte[] md5Hash = MD5ComputeHash(str);
            return MD5HashToString(md5Hash);
        }

        public static string MD5ComputeHashStr(Stream stream)
        {
            byte[] md5Hash = MD5ComputeHash(stream);
            return MD5HashToString(md5Hash);
        }

        public static string MD5HashToString(byte[] md5Hash)
        {
            return BitConverter.ToString(md5Hash).Replace("-", "").ToLowerInvariant();

            //string ret = null;

            //Pool.StringBuilderPool.Using((sb) =>
            //{
            //    foreach (byte b in md5Hash)
            //    {
            //        sb.Append(b.ToHexStr());
            //    }

            //    ret = sb.ToString();
            //});

            //return ret;
        }

        #endregion MD5

        #region Base64

        /// <summary>
        /// Base64编码
        /// </summary>
        public static string Base64Encode(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        public static string Base64Decode(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(bytes);
        }

        #endregion Base64

        private CryptoUtil()
        {
        }
    }
}
