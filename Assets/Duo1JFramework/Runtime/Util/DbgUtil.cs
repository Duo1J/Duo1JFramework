using Duo1JFramework.ObjectPool;
using Duo1JFramework.PlatformAPI;
using System.Diagnostics;

namespace Duo1JFramework
{
    /// <summary>
    /// Debug工具类
    /// </summary>
    public static class DbgUtil
    {
        /// <summary>
        /// 获取当前调用栈
        /// </summary>
        public static string GetStackTrace()
        {
            string ret = null;

            StackTrace strackTrace = new StackTrace();
            Pool.StringBuilderPool.Using((sb) =>
            {
                foreach (StackFrame frame in strackTrace.GetFrames())
                {
                    sb.AppendLine($"{frame.GetMethod()} - {frame.GetFileColumnNumber()}");
                }
                ret = sb.ToString();
            });

            return ret ?? string.Empty;
        }

        /// <summary>
        /// 获取当前内存信息
        /// </summary>
        public static string GetMemoryInfoStr()
        {
            return $"Heap: {Platform.Current.GetUsedHeapSize().B2MB().Limit(2)}/{Platform.Current.GetTotalHeapSize().B2MB().Limit(2)} MB\n" +
                   $"Reserved: {Platform.Current.GetTotalReservedMemory().B2MB().Limit(2)} MB\n" +
                   $"Total: {Platform.Current.GetTotalMemory().MB2GB().Limit(2)} GB\n";
        }
    }
}
