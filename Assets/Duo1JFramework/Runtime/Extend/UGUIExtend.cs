using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duo1JFramework
{
    /// <summary>
    /// UGUI接口扩展,
    /// 挂载组件逻辑在 `UIAddComponent` 中
    /// </summary>
    /// <see cref="UIAddComponent"/>
    public static class UGUIExtend
    {
        #region Button

        public static void OnUIClick(this GameObject go, UnityAction action)
        {
            go.GetOrAddComponent<Button>().OnUIClick(action);
        }

        public static void OnUIClick(this Button button, UnityAction action)
        {
            Assert.NotNull(action, "`Button::OnUIClick()` 参数 `action` 为空");
            button.onClick.AddListener(action);
        }

        public static void RemoveOnUIClick(this GameObject go, UnityAction action)
        {
            go.GetComponent<Button>()?.RemoveOnUIClick(action);
        }

        public static void RemoveOnUIClick(this Button button, UnityAction action)
        {
            Assert.NotNull(action, "`Button::RemoveOnUIClick()` 参数 `action`为空");
            button.onClick.RemoveListener(action);
        }

        public static void RemoveAllOnUIClick(this GameObject go)
        {
            go.GetComponent<Button>()?.RemoveAllOnUIClick();
        }

        public static void RemoveAllOnUIClick(this Button button)
        {
            button.onClick.RemoveAllListeners();
        }

        #endregion Button

        /// <summary>
        /// 设置CanvasGroup的Alpha值
        /// </summary>
        public static void SetCanvasGroupAlpha(this GameObject go, float alpha)
        {
            go.GetOrAddComponent<CanvasGroup>().alpha = alpha;
        }

        /// <summary>
        /// 设置CanvasGroup的Alpha值
        /// </summary>
        public static void SetCanvasGroupAlpha(this Component component, float alpha)
        {
            component.gameObject.SetCanvasGroupAlpha(alpha);
        }
    }
}
