using System;
using System.IO;
using System.Security.Cryptography;

namespace Duo1JFramework
{
    /// <summary>
    /// 加密工具类
    /// </summary>
    public static class CryptoUtil
    {
        #region AES

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

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] buffer = new byte[1024];
                        int len;
                        while ((len = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            cryptoStream.Write(buffer, 0, len);
                        }

                        cryptoStream.FlushFinalBlock();

                        byte[] encrypted = memoryStream.ToArray();
                        byte[] result = new byte[iv.Length + encrypted.Length];
                        Array.Copy(iv, 0, result, 0, iv.Length);
                        Array.Copy(encrypted, 0, result, iv.Length, encrypted.Length);

                        return result;
                    }
                }
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
            Assert.NotNull(bytes);

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return AesEncrypt(ms, key, iv);
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
                        Assert.Throw("读取IV失败");
                    }
                }

                aes.Key = key;
                aes.IV = iv;

                using (CryptoStream cryptoStream = new CryptoStream(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        cryptoStream.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
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
            Assert.NotNull(bytes);

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return AesDecrypt(ms, key, iv);
            }
        }

        #endregion AES
    }
}