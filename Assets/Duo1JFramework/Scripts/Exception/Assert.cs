using System;
using UnityEngine;

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
                    throw new CommonException("空指针异常");
                }
                else
                {
                    throw new CommonException($"空指针异常: {message}");
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