using UnityEngine;

namespace Duo1JFramework
{
    public static class Assert
    {
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
    }
}