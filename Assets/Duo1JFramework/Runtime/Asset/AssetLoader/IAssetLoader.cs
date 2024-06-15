using System;

using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源加载器
    /// </summary>
    public interface IAssetLoader : IDispose
    {
        /// <summary>
        /// 异步加载
        /// </summary>
        void Load<T>(string assetPath, Action<T> callback) where T : UObject;

        /// <summary>
        /// 同步加载
        /// </summary>
        T LoadSync<T>(string assetPath) where T : UObject;

        /// <summary>
        /// 异步加载实例
        /// </summary>
        void LoadIns<T>(string assetPath, Action<T> callback) where T : UObject;

        /// <summary>
        /// 同步加载实例
        /// </summary>
        T LoadInsSync<T>(string assetPath) where T : UObject;

        /// <summary>
        /// 异步加载Resources资源
        /// </summary>
        void LoadResource<T>(string assetPath, Action<T> callback) where T : UObject;

        /// <summary>
        /// 同步加载Resources资源
        /// </summary>
        T LoadResourceSync<T>(string assetPath) where T : UObject;

        /// <summary>
        /// 异步加载Resources实例
        /// </summary>
        void LoadResourceIns<T>(string assetPath, Action<T> callback) where T : UObject;

        /// <summary>
        /// /同步加载Resource实例
        /// </summary>
        T LoadResourceInsSync<T>(string assetPath) where T : UObject;
    }
}
