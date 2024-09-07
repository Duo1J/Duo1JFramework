using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// Unity GameObject 相关扩展
    /// </summary>
    public static class GameObjectExtend
    {
        /// <summary>
        /// 设置显隐
        /// </summary>
        public static void SetActive(this Component com, bool active)
        {
            com.gameObject.SetActive(active);
        }
    }
}