using System.IO;

namespace Duo1JFramework
{
    /// <summary>
    /// String扩展
    /// </summary>
    public static class StringExtend
    {
        #region FileSystem

        /// <summary>
        /// 检查文件是否存在，不存在则创建
        /// </summary>
        public static string CheckFile(this string str)
        {
            FileUtil.CheckFile(str);
            return str;
        }

        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        public static bool ExistFile(this string str)
        {
            return File.Exists(str);
        }

        /// <summary>
        /// 确保文件存在
        /// </summary>
        public static void GuardFile(this string str, string msg = "")
        {
            Assert.Guard(str.ExistFile(), $"文件不存在: `{str}`  {msg}");
        }

        /// <summary>
        /// 检查文件夹是否存在，不存在则创建
        /// </summary>
        public static string CheckDir(this string str)
        {
            FileUtil.CheckDir(str);
            return str;
        }

        /// <summary>
        /// 检查文件夹是否存在
        /// </summary>
        public static bool ExistDir(this string str)
        {
            return Directory.Exists(str);
        }

        /// <summary>
        /// 确保文件夹存在
        /// </summary>
        public static void GuardDir(this string str, string msg = "")
        {
            Assert.Guard(str.ExistDir(), $"文件夹不存在: `{str}`  {msg}");
        }

        #endregion FileSystem
    }
}