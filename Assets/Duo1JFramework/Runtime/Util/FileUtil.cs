using Duo1JFramework.Asset;
using System;
using System.Collections.Generic;
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
        public static string ReadAllText(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Log.ErrorForce($"不存在文件: {filePath}");
                    return "";
                }

                return File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, $"ReadAllText(path=`{filePath}`)");
                return "";
            }
        }

        /// <summary>
        /// 写入所有字符串
        /// </summary>
        public static void WriteAllText(string filePath, string txt)
        {
            try
            {
                File.WriteAllText(filePath.CheckFile(), txt);
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, $"WriteAllText(path=`{filePath}`, txt)/ntxt=`{txt}`");
            }
        }

        /// <summary>
        /// 检查文件是否创建，未创建则创建
        /// </summary>
        public static bool CheckFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, $"检查并创建文件异常: {filePath}");
                return false;
            }
        }

        /// <summary>
        /// 检查文件夹是否创建，未创建则创建
        /// </summary>
        public static bool CheckDir(string dirPath)
        {
            try
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, $"检查并创建文件夹异常: {dirPath}");
                return false;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return false;
                }

                File.Delete(filePath);

                return true;
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, $"删除文件异常: {filePath}");
                return false;
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        public static bool DeleteDir(string dirPath)
        {
            try
            {
                if (!Directory.Exists(dirPath))
                {
                    return false;
                }

                Directory.Delete(dirPath, true);

                return true;
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, $"删除文件夹异常: {dirPath}");
                return false;
            }
        }

        /// <summary>
        /// 拷贝文件夹
        /// 若存在相同文件则跳过
        /// </summary>
        public static bool CopyDirectory(string srcDir, string tarDir)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(srcDir);
                if (!dirInfo.Exists)
                {
                    Log.ErrorForce($"未找到待拷贝的源文件夹: `{srcDir}`");
                    return false;
                }

                if (!CheckDir(tarDir))
                {
                    return false;
                }

                DirectoryInfo[] dirInfoList = dirInfo.GetDirectories();
                FileInfo[] fileInfoList = dirInfo.GetFiles();

                foreach (FileInfo fileInfo in fileInfoList)
                {
                    string tempPath = Path.Combine(tarDir, fileInfo.Name);
                    fileInfo.CopyTo(tempPath, false);
                }

                foreach (DirectoryInfo subDirInfo in dirInfoList)
                {
                    string tempPath = Path.Combine(tarDir, subDirInfo.Name);
                    if (!CopyDirectory(subDirInfo.FullName, tempPath))
                    {
                        throw CommonException.Create($"拷贝文件夹`{subDirInfo.FullName}`到`{tempPath}`失败");
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, $"拷贝文件夹`{srcDir}`到`{tarDir}`时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 获取某一文件夹下文件并过滤
        /// 默认获取所有层级的所有文件
        /// </summary>
        /// <param name="pathModifier">路径修改钩子，返回null表示跳过</param>
        public static List<string> GetFileInDir(string dirPath, Func<string, string> pathModifier = null, string searchPattern = "*", SearchOption option = SearchOption.AllDirectories)
        {
            List<string> ret = new List<string>();

            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            FileInfo[] fileInfos = dirInfo.GetFiles(searchPattern, option);
            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Exists)
                {
                    string path = fileInfo.FullName;
                    path = PathUtil.UnifySplit(path);

                    if (pathModifier != null)
                    {
                        path = pathModifier(path);
                    }

                    if (path != null)
                    {
                        ret.Add(path);
                    }
                }
            }

            return ret;
        }
    }
}