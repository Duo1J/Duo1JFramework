using System;

namespace Duo1JFramework
{
    /// <summary>
    /// 枚举工具类
    /// </summary>
    public static class EnumUtil
    {
        public static string GetName(Enum e)
        {
            return Enum.GetName(e.GetType(), e);
        }
    }

    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtend
    {
        public static string GetName(this Enum e)
        {
            return EnumUtil.GetName(e);
        }
    }
}