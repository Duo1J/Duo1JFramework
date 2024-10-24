using System;

namespace Duo1JFramework
{
    /// <summary>
    /// 常用工具类
    /// </summary>
    public class Util
    {
        /// <summary>
        /// 安全执行
        /// </summary>
        public static bool SafeExecute(Action action)
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

        private Util()
        {
        }
    }
}
