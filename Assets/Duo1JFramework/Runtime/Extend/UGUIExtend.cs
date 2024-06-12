using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duo1JFramework
{
    /// <summary>
    /// UGUI接口扩展
    /// </summary>
    public static class UGUIExtend
    {
        #region Button

        public static void OnClick(this GameObject go, UnityAction action)
        {
            go.GetAndAssertComponent<Button>().OnClick(action);
        }

        public static void OnClick(this Button button, UnityAction action)
        {
            Assert.NotNull(action, "`Button::OnClick()`参数`action`为空");
            button.onClick.AddListener(action);
        }

        public static void RemoveOnClick(this GameObject go, UnityAction action)
        {
            go.GetAndAssertComponent<Button>().RemoveOnClick(action);
        }

        public static void RemoveOnClick(this Button button, UnityAction action)
        {
            Assert.NotNull(action, "`Button::RemoveOnClick()`参数`action`为空");
            button.onClick.RemoveListener(action);
        }

        public static void RemoveAllOnclick(this GameObject go)
        {
            go.GetAndAssertComponent<Button>().RemoveAllOnclick();
        }

        public static void RemoveAllOnclick(this Button button)
        {
            button.onClick.RemoveAllListeners();
        }

        #endregion Button
    }
}
