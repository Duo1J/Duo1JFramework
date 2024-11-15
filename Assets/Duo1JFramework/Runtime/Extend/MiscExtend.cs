using Duo1JFramework.Asset;
using System;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace Duo1JFramework
{
    /// <summary>
    /// 杂项扩展
    /// </summary>
    public static class MiscExtend
    {
        #region Enum

        /// <summary>
        /// 获取枚举名
        /// </summary>
        public static string GetName(this Enum e)
        {
            return EnumUtil.GetName(e);
        }

        #endregion Enum

        #region AssetHandle

        /// <summary>
        /// 将资源句柄绑定到Mono生命周期
        /// </summary>
        public static void BindMono<T>(this IAssetHandle<T> handle, GameObject go) where T : UObject
        {
            MonoAssetHandle monoHandle = go.GetOrAddComponent<MonoAssetHandle>();
            monoHandle.AddHandle(handle);
        }

        /// <summary>
        /// 将资源句柄绑定到Mono生命周期
        /// </summary>
        public static void BindMono<T>(this IAssetHandle<T> handle, Transform tf) where T : UObject
        {
            handle.BindMono(tf.gameObject);
        }

        #endregion AssetHandle
    }
}
