using System;

namespace Duo1JFramework
{
    /// <summary>
    /// 枚举工具类
    /// </summary>
    public static class EnumUtil
    {
        /// <summary>
        /// 获取枚举名
        /// </summary>
        public static string GetName(Enum e)
        {
            return Enum.GetName(e.GetType(), e);
        }
    }
}
