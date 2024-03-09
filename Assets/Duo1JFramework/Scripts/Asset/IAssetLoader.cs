using System;

using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源加载器
    /// </summary>
    public interface IAssetLoader
    {
        /// <summary>
        /// 异步加载
        /// </summary>
        public void Load<T>(string assetPath, Action<T> callback) where T : UObject;

        /// <summary>
        /// 同步加载
        /// </summary>
        public T LoadSync<T>(string assetPath) where T : UObject;

        /// <summary>
        /// 异步加载实例
        /// </summary>
        public void LoadIns<T>(string assetPath, Action<T> callback) where T : UObject;

        /// <summary>
        /// 同步加载实例
        /// </summary>
        public T LoadInsSync<T>(string assetPath) where T : UObject;

        /// <summary>
        /// 异步加载Resources资源
        /// </summary>
        public void LoadResource<T>(string assetPath, Action<T> callback) where T : UObject;

        /// <summary>
        /// 同步加载Resources资源
        /// </summary>
        public T LoadResourceSync<T>(string assetPath) where T : UObject;

        /// <summary>
        /// 异步加载Resources实例
        /// </summary>
        public void LoadResourceIns<T>(string assetPath, Action<T> callback) where T : UObject;

        /// <summary>
        /// /同步加载Resource实例
        /// </summary>
        public T LoadResourceInsSync<T>(string assetPath) where T : UObject;
    }
}
