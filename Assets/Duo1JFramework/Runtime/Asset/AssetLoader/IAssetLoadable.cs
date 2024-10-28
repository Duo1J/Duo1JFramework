using System;
using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 可加载资源接口
    /// </summary>
    public interface IAssetLoadable
    {
        /// <summary>
        /// 异步加载
        /// </summary>
        void Load<T>(string assetPath, Action<IAssetHandle<T>> callback) where T : UObject;

        /// <summary>
        /// 同步加载
        /// </summary>
        IAssetHandle<T> LoadSync<T>(string assetPath) where T : UObject;

        /// <summary>
        /// 异步加载Resources资源
        /// </summary>
        void LoadResource<T>(string assetPath, Action<IAssetHandle<T>> callback) where T : UObject;

        /// <summary>
        /// 同步加载Resources资源
        /// </summary>
        IAssetHandle<T> LoadResourceSync<T>(string assetPath) where T : UObject;
    }
}
