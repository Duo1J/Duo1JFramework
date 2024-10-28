using System;
using System.Collections.Generic;
using Duo1JFramework.Asset;
using Object = UnityEngine.Object;

namespace Duo1JFramework
{
    /// <summary>
    /// 资源集合
    /// </summary>
    public class AssetCollection : IAssetLoadable, IDispose
    {
        /// <summary>
        /// 资源句柄集合
        /// </summary>
        private HashSet<IDispose> assetHandleSet;

        /// <summary>
        /// 通过加载方式异步加载
        /// </summary>
        public void LoadByType<T>(string assetPath, Action<IAssetHandle<T>> callback, EAssetLoadType loadType = EAssetLoadType.Bundle) where T : Object
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            Assert.NotNullArg(callback, "callback");

            if (assetHandleSet == null)
            {
                assetHandleSet = new HashSet<IDispose>();
            }

            AssetManager.Instance.LoadByType<T>(assetPath, (handle) =>
            {
                if (handle != null)
                {
                    assetHandleSet.Add(handle);
                }

                callback(handle);
            }, loadType);
        }

        /// <summary>
        /// 通过加载方式同步加载
        /// </summary>
        public IAssetHandle<T> LoadByTypeSync<T>(string assetPath, EAssetLoadType loadType = EAssetLoadType.Bundle) where T : Object
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");

            if (assetHandleSet == null)
            {
                assetHandleSet = new HashSet<IDispose>();
            }

            IAssetHandle<T> handle = AssetManager.Instance.LoadByTypeSync<T>(assetPath, loadType);
            if (handle != null)
            {
                assetHandleSet.Add(handle);
            }

            return handle;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public void Load<T>(string assetPath, Action<IAssetHandle<T>> callback) where T : Object
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            Assert.NotNullArg(callback, "callback");

            if (assetHandleSet == null)
            {
                assetHandleSet = new HashSet<IDispose>();
            }

            AssetManager.Instance.Load<T>(assetPath, (handle) =>
            {
                if (handle != null)
                {
                    assetHandleSet.Add(handle);
                }

                callback(handle);
            });
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public IAssetHandle<T> LoadSync<T>(string assetPath) where T : Object
        {
            Assert.NotNullOrEmpty(assetPath, "callback");

            if (assetHandleSet == null)
            {
                assetHandleSet = new HashSet<IDispose>();
            }

            IAssetHandle<T> handle = AssetManager.Instance.LoadSync<T>(assetPath);
            if (handle != null)
            {
                assetHandleSet.Add(handle);
            }

            return handle;
        }

        /// <summary>
        /// 异步加载Resources资源
        /// </summary>
        public void LoadResource<T>(string assetPath, Action<IAssetHandle<T>> callback) where T : Object
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            Assert.NotNullArg(callback, "callback");

            if (assetHandleSet == null)
            {
                assetHandleSet = new HashSet<IDispose>();
            }

            AssetManager.Instance.LoadResource<T>(assetPath, (handle) =>
            {
                if (handle != null)
                {
                    assetHandleSet.Add(handle);
                }

                callback(handle);
            });
        }

        /// <summary>
        /// 同步加载Resources资源
        /// </summary>
        public IAssetHandle<T> LoadResourceSync<T>(string assetPath) where T : Object
        {
            Assert.NotNullOrEmpty(assetPath, "callback");

            if (assetHandleSet == null)
            {
                assetHandleSet = new HashSet<IDispose>();
            }

            IAssetHandle<T> handle = AssetManager.Instance.LoadResourceSync<T>(assetPath);
            if (handle != null)
            {
                assetHandleSet.Add(handle);
            }

            return handle;
        }

        public void Dispose()
        {
            if (assetHandleSet != null)
            {
                foreach (IDispose handler in assetHandleSet)
                {
                    handler.Dispose();
                }

                assetHandleSet = null;
            }
        }
    }
}
