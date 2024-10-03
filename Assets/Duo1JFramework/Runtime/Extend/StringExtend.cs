using System.IO;

namespace Duo1JFramework
{
    /// <summary>
    /// String 相关扩展
    /// </summary>
    public static class StringExtend
    {
        #region FileSystem

        /// <summary>
        /// 统一路径分隔符
        /// </summary>
        public static string SplitUnify(this string str)
        {
            return PathUtil.SplitUnify(str);
        }

        /// <summary>
        /// 移除文件类型后缀
        /// </summary>
        public static string RemoveTypeSuffix(string str)
        {
            return PathUtil.RemoveTypeSuffix(str);
        }

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
        /// 删除文件
        /// </summary>
        public static bool DeleteFile(this string str)
        {
            return FileUtil.DeleteFile(str);
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

        /// <summary>
        /// 删除文件夹
        /// </summary>
        public static bool DeleteDir(this string str)
        {
            return FileUtil.DeleteDir(str);
        }

        #endregion FileSystem

        #region Encoding

        /// <summary>
        /// 转为Base64字符串
        /// </summary>
        public static string ToBase64(this string str)
        {
            return CryptoUtil.Base64Encode(str);
        }

        /// <summary>
        /// 从Base64字符串转为UTF-8字符串
        /// </summary>
        public static string FromBase64(this string str)
        {
            return CryptoUtil.Base64Decode(str);
        }

        #endregion Encoding

        /// <summary>
        /// 字符串分割并Trim
        /// </summary>
        public static string[] SplitTrim(this string str, params char[] separator)
        {
            return str.Split(separator).Trim();
        }

        /// <summary>
        /// 字符串数组Trim
        /// </summary>
        public static string[] Trim(this string[] strArr)
        {
            for (int i = 0; i < strArr.Length; i++)
            {
                strArr[i] = strArr[i].Trim();
            }

            return strArr;
        }

        #region Json

        /// <summary>
        /// 对象转Json
        /// </summary>
        public static string ToJson(this object o)
        {
            return JsonUtil.ToJson(o);
        }

        /// <summary>
        /// Json转泛型对象
        /// </summary>
        public static T FromJson<T>(this string str)
        {
            return JsonUtil.ToObject<T>(str);
        }

        /// <summary>
        /// Json转对象
        /// </summary>
        public static object FromJson(this string str)
        {
            return JsonUtil.ToObject(str);
        }

        #endregion Json
    }
}
