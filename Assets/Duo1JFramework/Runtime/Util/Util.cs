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
            try
            {
                action?.Invoke();
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
        public static bool TryCatch(Func<bool> action)
        {
            try
            {
                return action == null || action.Invoke();
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e);
                return false;
            }
        }

        private Util()
        {
        }
    }
}
