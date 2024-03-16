using System;
using System.IO;

namespace Duo1JFramework
{
    /// <summary>
    /// 文件工具类
    /// </summary>
    public static class FileUtil
    {
        /// <summary>
        /// 读取所有字符串
        /// </summary>
        public static string ReadAllText(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, $"ReadAllText(path=`{path}`)");
                return "";
            }
        }

        /// <summary>
        /// 写入所有字符串
        /// </summary>
        public static void WriteAllText(string path, string txt)
        {
            try
            {
                File.WriteAllText(path, txt);
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, $"WriteAllText(path=`{path}`, txt)/ntxt=`{txt}`");
            }
        }

#if UNITY_EDITOR

        /// <summary>
        /// 获取某文件夹下所有资源路径
        /// </summary>
        public static string[] GetAllAssetsPath(string folder)
        {
            //todo hlj
            return null;
        }

#endif
    }
}