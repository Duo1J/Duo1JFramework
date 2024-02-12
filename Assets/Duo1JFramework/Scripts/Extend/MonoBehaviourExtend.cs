using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// MonoBehaviour扩展方法
    /// </summary>
    public static class MonoBehaviourExtend
    {
        /// <summary>
        /// 获取或添加MB组件
        /// </summary>
        public static T GetOrAddComponent<T>(this MonoBehaviour mb) where T : Component
        {
            return mb.gameObject.GetOrAddComponent<T>();
        }
    }
}