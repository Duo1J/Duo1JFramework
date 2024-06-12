using Duo1JFramework.Asset;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// String扩展
    /// </summary>
    public static class StringExtend
    {
        /// <summary>
        /// 检查文件是否创建，未创建则创建
        /// </summary>
        public static string CheckFile(this string str)
        {
            return FileUtil.CheckFile(str);
        }

        /// <summary>
        /// 检查文件夹是否创建，未创建则创建
        /// </summary>
        public static string CheckDir(this string str)
        {
            return FileUtil.CheckDir(str);
        }
    }
}