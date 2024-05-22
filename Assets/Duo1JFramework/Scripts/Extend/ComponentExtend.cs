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

        /// <summary>
        /// 获取并断言组件
        /// </summary>
        public static T GetAndAssertComponent<T>(this Component com, string msg = null) where T : Component
        {
            return com.gameObject.GetAndAssertComponent<T>(msg);
        }

        /// <summary>
        /// 设置显隐
        /// </summary>
        public static void SetActive(this Component com, bool active)
        {
            com.gameObject.SetActive(active);
        }

        #region UI

        /// <summary>
        /// 设置CanvasGroup的Alpha值
        /// </summary>
        public static void SetCanvasGroupAlpha(this Component component, float alpha)
        {
            component.gameObject.SetCanvasGroupAlpha(alpha);
        }

        #endregion UI
    }
}