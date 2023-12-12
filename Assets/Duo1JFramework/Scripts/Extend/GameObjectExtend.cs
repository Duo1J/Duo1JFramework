using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// GameObject扩展方法
    /// </summary>
    public static class GameObjectExtend
    {
        /// <summary>
        /// 重置旋转、缩放、坐标
        /// </summary>
        public static void ResetSRT(this GameObject go)
        {
            go.transform.ResetSRT();
        }

        public static T GetAndAssertComponent<T>(this GameObject go, string msg = null)
        {
            T ret = go.GetComponent<T>();
            Assert.NotNull(ret, msg);
            return ret;
        }
    }
}