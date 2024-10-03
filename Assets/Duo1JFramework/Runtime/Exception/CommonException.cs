using System;

namespace Duo1JFramework
{
    public class CommonException : Exception
    {
        public CommonException(string message) : base(message) { }

        public static CommonException Create(string message)
        {
            return new CommonException(message);
        }

        public static void Throw(string message)
        {
            throw Create(message);
        }
    }
}
