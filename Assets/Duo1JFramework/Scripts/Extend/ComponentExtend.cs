using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// Component扩展方法
    /// </summary>
    public static class ComponentExtend
    {
        /// <summary>
        /// 获取或添加组件
        /// </summary>
        public static T GetOrAddComponent<T>(this Component com) where T : Component
        {
            return com.gameObject.GetOrAddComponent<T>();
        }

        public static T GetAndAssertComponent<T>(this Component com, string msg = null) where T : Component
        {
            return com.gameObject.GetAndAssertComponent<T>(msg);
        }

        public static void SetActive(this Component com, bool active)
        {
            com.gameObject.SetActive(active);
        }
    }
}