using Duo1JFramework.Scheduling;
using System;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源加载器基类
    /// </summary>
    public abstract class BaseAssetLoader : IAssetLoader
    {
        /// <summary>
        /// 异步加载
        /// </summary>
        public abstract void Load<T>(string assetPath, Action<IAssetHandle<T>> callback) where T : UObject;

        /// <summary>
        /// 同步加载
        /// </summary>
        public abstract IAssetHandle<T> LoadSync<T>(string assetPath) where T : UObject;

        /// <summary>
        /// 异步加载Resources资源
        /// </summary>
        public virtual void LoadResource<T>(string assetPath, Action<IAssetHandle<T>> callback) where T : UObject
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            Assert.NotNullArg(callback, "callback");

            ResourceRequest request = Resources.LoadAsync<T>(assetPath);
            UpdateManager.Instance.RegisterAsyncRequest(request, (req) =>
            {
                ResourceRequest _request = req as ResourceRequest;
                UObject asset = _request.asset;

                if (asset == null)
                {
                    Log.Error($"无法加载到Resources资源: `{assetPath}`");
                    callback(null);
                    return;
                }

                ResAssetHandle<T> handle = ResAssetHandle<T>.Create(asset.Convert<T>());
                callback(handle);
            });
        }

        /// <summary>
        /// 同步加载Resources资源
        /// </summary>
        public virtual IAssetHandle<T> LoadResourceSync<T>(string assetPath) where T : UObject
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");

            T asset = Resources.Load<T>(assetPath);
            if (asset == null)
            {
                Log.Error($"无法加载到Resources资源: `{assetPath}`");
                return null;
            }

            ResAssetHandle<T> handle = ResAssetHandle<T>.Create(asset);
            return handle;
        }

        public virtual void Dispose()
        {
        }
    }
}
