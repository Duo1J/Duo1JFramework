using System;

namespace Duo1JFramework
{
    public class CommonException : Exception
    {
        public CommonException(string message) : base(message) { }
    }
}