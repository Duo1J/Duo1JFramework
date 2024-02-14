using UnityEngine;

using UObject = UnityEngine.Object;

namespace Duo1JFramework
{
    /// <summary>
    /// Unity Object扩展方法
    /// </summary>
    public static class ObjectExtend
    {
        /// <summary>
        /// 立即销毁
        /// </summary>
        public static void DestroyImmediate(this UObject obj, bool allowDestroyingAssets = false)
        {
            UObject.DestroyImmediate(obj, allowDestroyingAssets);
        }

        public static void Destroy(this UObject obj, float t = 0)
        {
            UObject.Destroy(obj, t);
        }
    }
}