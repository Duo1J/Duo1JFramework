using Duo1JFramework.Config;
using System;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public class AssetManager : MonoSingleton<AssetManager>, IAssetLoader
    {
        private IAssetLoader loader;

        /// <summary>
        /// 异步加载
        /// </summary>
        public void Load<T>(string assetPath, Action<T> callback) where T : UObject
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            assetPath = ModifyAssetPath(assetPath);

            loader.Load<T>(assetPath, callback);
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public T LoadSync<T>(string assetPath) where T : UObject
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            assetPath = ModifyAssetPath(assetPath);

            return loader.LoadSync<T>(assetPath);
        }

        /// <summary>
        /// 异步加载实例
        /// </summary>
        public void LoadIns<T>(string assetPath, Action<T> callback) where T : UObject
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            assetPath = ModifyAssetPath(assetPath);

            loader.LoadIns<T>(assetPath, callback);
        }

        /// <summary>
        /// 同步加载实例
        /// </summary>
        public T LoadInsSync<T>(string assetPath) where T : UObject
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            assetPath = ModifyAssetPath(assetPath);

            return loader.LoadInsSync<T>(assetPath);
        }

        /// <summary>
        /// 异步加载Resources资源
        /// </summary>
        public void LoadResource<T>(string assetPath, Action<T> callback) where T : UObject
        {
            loader.LoadResource<T>(assetPath, callback);
        }

        /// <summary>
        /// 同步加载Resources资源
        /// </summary>
        public T LoadResourceSync<T>(string assetPath) where T : UObject
        {
            return loader.LoadResourceSync<T>(assetPath);
        }

        /// <summary>
        /// 异步加载Resources实例
        /// </summary>
        public void LoadResourceIns<T>(string assetPath, Action<T> callback) where T : UObject
        {
            loader.LoadResourceIns<T>(assetPath, callback);
        }

        /// <summary>
        /// /同步加载Resource实例
        /// </summary>
        public T LoadResourceInsSync<T>(string assetPath) where T : UObject
        {
            return loader.LoadResourceInsSync<T>(assetPath);
        }

        public void GC()
        {
            if (!Game.IsEditor)
            {
                ABManager.Instance.GC();
            }
            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 设置自定义加载器
        /// </summary>
        public void SetCustomLoader(IAssetLoader loader)
        {
            this.loader = loader;
        }

        /// <summary>
        /// 修正资源路径，添加前缀
        /// </summary>
        private string ModifyAssetPath(string assetPath)
        {
            return $"{Path.ASSET_PATH_PREFIX}{assetPath}";
        }

        protected override void OnInit()
        {
            if (loader == null)
            {
                if (GameConfig.Instance.EditorUseAB)
                {
                    SetCustomLoader(new ABAssetLoader());
                }
                else if (Game.IsEditor)
                {
                    SetCustomLoader(new EditorAssetLoader());
                }
                else
                {
                    SetCustomLoader(new ABAssetLoader());
                }
            }
        }

        protected override void OnDispose()
        {
        }
    }
}