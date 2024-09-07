using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// Unity Component 相关扩展
    /// </summary>
    public static class ComponentExtend
    {
        /// <summary>
        /// 获取或添加组件
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T com = go.GetComponent<T>();

            if (com == null)
            {
                com = go.AddComponent<T>();
            }

            return com;
        }

        /// <summary>
        /// 获取或添加组件
        /// </summary>
        public static T GetOrAddComponent<T>(this Component com) where T : Component
        {
            return com.gameObject.GetOrAddComponent<T>();
        }

        /// <summary>
        /// 获取并断言组件
        /// </summary>
        public static T GetAndAssertComponent<T>(this GameObject go, string msg = null)
        {
            if (msg == null)
            {
                msg = $"{go.GetNameInsID()} 未持有组件 `{typeof(T).FullName}`";
            }

            T ret = go.GetComponent<T>();
            Assert.NotNull(ret, msg);

            return ret;
        }

        /// <summary>
        /// 获取并断言组件
        /// </summary>
        public static T GetAndAssertComponent<T>(this Component com, string msg = null) where T : Component
        {
            return com.gameObject.GetAndAssertComponent<T>(msg);
        }
    }
}