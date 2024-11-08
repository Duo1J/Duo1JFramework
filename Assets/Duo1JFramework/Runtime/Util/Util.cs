using System;

namespace Duo1JFramework
{
    /// <summary>
    /// 常用工具类
    /// </summary>
    public class Util
    {
        /// <summary>
        /// TryCatch安全执行
        /// </summary>
        public static bool TryCatch(Action action)
        {
            Assert.NotNullArg(action, "action");

            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// TryCatch安全执行
        /// </summary>
        public static T TryCatch<T>(Func<T> action)
        {
            Assert.NotNullArg(action, "action");

            try
            {
                return action.Invoke();
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e);
                return default(T);
            }
        }

        private Util()
        {
        }
    }
}
