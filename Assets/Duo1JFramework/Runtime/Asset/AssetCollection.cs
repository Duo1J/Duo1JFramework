using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源管理集合
    /// </summary>
    /// <see cref="AssetManager"/>
    public class AssetCollection : IAssetLoadable, IDispose
    {
        /// <summary>
        /// 资源句柄集合
        /// </summary>
        private HashSet<IDispose> assetHandleSet;

        /// <summary>
        /// 是否已释放
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// 通过加载方式异步加载
        /// </summary>
        public void LoadByType<T>(string assetPath, Action<IAssetHandle<T>> callback, EAssetLoadType loadType = EAssetLoadType.Bundle) where T : Object
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            Assert.NotNullArg(callback, "callback");

            if (CheckDisposed())
            {
                return;
            }

            AssetManager.Instance.LoadByType<T>(assetPath, (handle) =>
            {
                if (!AddHandle(handle))
                {
                    return;
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

            if (CheckDisposed())
            {
                return null;
            }

            IAssetHandle<T> handle = AssetManager.Instance.LoadByTypeSync<T>(assetPath, loadType);
            AddHandle(handle);
            return handle;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public void Load<T>(string assetPath, Action<IAssetHandle<T>> callback) where T : Object
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            Assert.NotNullArg(callback, "callback");

            if (CheckDisposed())
            {
                return;
            }

            AssetManager.Instance.Load<T>(assetPath, (handle) =>
            {
                if (!AddHandle(handle))
                {
                    return;
                }

                callback(handle);
            });
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public IAssetHandle<T> LoadSync<T>(string assetPath) where T : Object
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");

            if (CheckDisposed())
            {
                return null;
            }

            IAssetHandle<T> handle = AssetManager.Instance.LoadSync<T>(assetPath);
            AddHandle(handle);
            return handle;
        }

        /// <summary>
        /// 异步加载Resources资源
        /// </summary>
        public void LoadResource<T>(string assetPath, Action<IAssetHandle<T>> callback) where T : Object
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            Assert.NotNullArg(callback, "callback");

            if (CheckDisposed())
            {
                return;
            }

            AssetManager.Instance.LoadResource<T>(assetPath, (handle) =>
            {
                if (!AddHandle(handle))
                {
                    return;
                }

                callback(handle);
            });
        }

        /// <summary>
        /// 同步加载Resources资源
        /// </summary>
        public IAssetHandle<T> LoadResourceSync<T>(string assetPath) where T : Object
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");

            if (CheckDisposed())
            {
                return null;
            }

            IAssetHandle<T> handle = AssetManager.Instance.LoadResourceSync<T>(assetPath);
            AddHandle(handle);
            return handle;
        }

        private bool AddHandle(IDispose handle)
        {
            if (handle == null)
            {
                return true;
            }

            if (Disposed)
            {
                handle.Dispose();
                return false;
            }

            if (assetHandleSet == null)
            {
                assetHandleSet = new HashSet<IDispose>();
            }

            assetHandleSet.Add(handle);
            return true;
        }

        private bool CheckDisposed()
        {
            if (!Disposed)
            {
                return false;
            }

            Log.Warn("AssetCollection Disposed");
            return true;
        }

        public void Dispose()
        {
            if (Disposed)
            {
                return;
            }

            Disposed = true;

            if (assetHandleSet != null)
            {
                List<IDispose> assetHandleList = new List<IDispose>(assetHandleSet);
                foreach (IDispose handler in assetHandleList)
                {
                    handler?.Dispose();
                }

                assetHandleSet.Clear();
                assetHandleSet = null;
            }
        }
    }
}
