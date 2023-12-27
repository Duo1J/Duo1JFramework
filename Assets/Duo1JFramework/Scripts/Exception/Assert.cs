using System;

namespace Duo1JFramework
{
    public static class Assert
    {
        /// <summary>
        /// 断言不为空
        /// </summary>
        public static void NotNull(object o, string message)
        {
            if (o == null)
            {
                if (message == null)
                {
                    throw new CommonException("<空指针异常>");
                }
                else
                {
                    throw new CommonException($"<空指针异常>: {message}");
                }
            }
        }

        /// <summary>
        /// 判断字符串是否不为null以及""，否则抛出异常
        /// </summary>
        public static void NotNullOrEmpty(string str, string message)
        {
            if (string.IsNullOrEmpty(str))
            {
                if (message == null)
                {
                    throw new CommonException("<字符串异常>");
                }
                else
                {
                    throw new CommonException($"<字符串异常>: {message}");
                }
            }
        }

        /// <summary>
        /// 判断是否为真，否则抛出异常
        /// </summary>
        /// <param name="b"></param>
        /// <param name="message"></param>
        public static void Guard(bool b, string message)
        {
            if (!b)
            {
                if (message == null)
                {
                    throw new CommonException("<Guard异常>");
                }
                else
                {
                    throw new CommonException($"<Guard异常>: {message}");
                }
            }
        }

        /// <summary>
        /// 通用异常处理
        /// </summary>
        public static void ExceptHandle(Exception e, params object[] msg)
        {
            Log.Exception(e, msg);
        }
    }
}