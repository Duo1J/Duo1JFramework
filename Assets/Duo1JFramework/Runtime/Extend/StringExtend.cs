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
            FileUtil.CheckFile(str);
            return str;
        }

        /// <summary>
        /// 检查文件夹是否创建，未创建则创建
        /// </summary>
        public static string CheckDir(this string str)
        {
            FileUtil.CheckDir(str);
            return str;
        }
    }
}