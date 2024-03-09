using Duo1JFramework.TimerUpdate;
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
        public abstract void Load<T>(string assetPath, Action<T> callback) where T : UObject;

        /// <summary>
        /// 同步加载
        /// </summary>
        public abstract T LoadSync<T>(string assetPath) where T : UObject;

        /// <summary>
        /// 异步加载实例
        /// </summary>
        public virtual void LoadIns<T>(string assetPath, Action<T> callback) where T : UObject
        {
            Load<T>(assetPath, (asset) =>
            {
                if (asset == null)
                {
                    callback(null);
                    return;
                }
                T ins = UObject.Instantiate(asset);
                if (ins == null)
                {
                    Log.Error($"实例化资源失败: `{assetPath}`");
                    callback(null);
                    return;
                }
                callback(ins);
            });
        }

        /// <summary>
        /// 同步加载实例
        /// </summary>
        public virtual T LoadInsSync<T>(string assetPath) where T : UObject
        {
            T asset = LoadSync<T>(assetPath);
            if (asset == null)
            {
                return null;
            }
            T ins = UObject.Instantiate(asset);
            return ins;
        }

        /// <summary>
        /// 异步加载Resources资源
        /// </summary>
        public virtual void LoadResource<T>(string assetPath, Action<T> callback) where T : UObject
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            Assert.NotNull(callback, "回调不可为空");

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
                callback(asset.Convert<T>());
            });
        }

        /// <summary>
        /// 同步加载Resources资源
        /// </summary>
        public virtual T LoadResourceSync<T>(string assetPath) where T : UObject
        {
            Assert.NotNull(assetPath, "资源路径不可为空");

            T asset = Resources.Load<T>(assetPath);
            if (asset == null)
            {
                Log.Error($"无法加载到Resources资源: `{assetPath}`");
                return null;
            }
            return asset;
        }

        /// <summary>
        /// 异步加载Resources实例
        /// </summary>
        public virtual void LoadResourceIns<T>(string assetPath, Action<T> callback) where T : UObject
        {
            LoadResource<T>(assetPath, (asset) =>
            {
                if (asset == null)
                {
                    callback(null);
                    return;
                }
                T ins = UObject.Instantiate(asset);
                if (ins == null)
                {
                    Log.Error($"实例化Resources资源失败: `{assetPath}`");
                    callback(null);
                    return;
                }
                callback(ins);
            });
        }

        /// <summary>
        /// 同步加载Resource实例
        /// </summary>
        public virtual T LoadResourceInsSync<T>(string assetPath) where T : UObject
        {
            T asset = LoadResourceSync<T>(assetPath);
            if (asset == null)
            {
                return null;
            }
            T ins = UObject.Instantiate(asset);
            return ins;
        }
    }
}
