using System;

namespace Duo1JFramework
{
    /// <summary>
    /// 一般异常
    /// </summary>
    public class Except : Exception
    {
        public Except(string message) : base(message)
        {
        }

        /// <summary>
        /// 创建
        /// </summary>
        public static Except Create(string message)
        {
            return new Except(message);
        }
    }
}
