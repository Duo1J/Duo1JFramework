using UnityEngine;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源句柄接口
    /// </summary>
    public interface IAssetHandle<T> : IDispose where T : Object
    {
        /// <summary>
        /// 资源引用
        /// </summary>
        T Asset { get; }

        /// <summary>
        /// 资源实例化
        /// </summary>
        /// <returns></returns>
        T Instantiate();

        /// <summary>
        /// 检查是否异常
        /// </summary>
        bool Error();

        /// <summary>
        /// 释放资源句柄
        /// </summary>
        void Release();
    }

    public static class AssetHandleExtend
    {
        public static void BindMono<T>(this IAssetHandle<T> handle, GameObject go) where T : Object
        {
            MonoAssetHandle monoHandle = go.GetOrAddComponent<MonoAssetHandle>();
            monoHandle.AddHandle(handle);
        }

        public static void BindMono<T>(this IAssetHandle<T> handle, Transform tf) where T : Object
        {
            handle.BindMono(tf.gameObject);
        }
    }
}