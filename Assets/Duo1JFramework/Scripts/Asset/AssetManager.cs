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
        /// 通过加载方式加载
        /// </summary>
        public void LoadByType<T>(EAssetLoadType loadType, string assetPath, Action<T> callback) where T : UObject
        {
            switch (loadType)
            {
                case EAssetLoadType.AssetBundle:
                    {
                        Load<T>(assetPath, callback);
                        return;
                    }
                case EAssetLoadType.Resources:
                    {
                        LoadResource<T>(assetPath, callback);
                        return;
                    }
                default:
                    {
                        Log.ErrorForce($"LoadByType 未处理的加载方式: `{loadType}`");
                        callback?.Invoke(null);
                        return;
                    }
            }
        }

        /// <summary>
        /// 通过加载方式加载实例
        /// </summary>
        public void LoadInsByType<T>(EAssetLoadType loadType, string assetPath, Action<T> callback) where T : UObject
        {
            switch (loadType)
            {
                case EAssetLoadType.AssetBundle:
                    {
                        LoadIns<T>(assetPath, callback);
                        return;
                    }
                case EAssetLoadType.Resources:
                    {
                        LoadResourceIns<T>(assetPath, callback);
                        return;
                    }
                default:
                    {
                        Log.ErrorForce($"LoadInsByType 未处理的加载方式: `{loadType}`");
                        callback?.Invoke(null);
                        return;
                    }
            }
        }

        /// <summary>
        /// 通过加载方式同步加载
        /// </summary>
        public T LoadByTypeSync<T>(EAssetLoadType loadType, string assetPath) where T : UObject
        {
            switch (loadType)
            {
                case EAssetLoadType.AssetBundle:
                    {
                        return LoadSync<T>(assetPath);
                    }
                case EAssetLoadType.Resources:
                    {
                        return LoadResourceSync<T>(assetPath);
                    }
                default:
                    {
                        Log.ErrorForce($"LoadByTypeSync 未处理的加载方式: `{loadType}`");
                        return null;
                    }
            }
        }

        /// <summary>
        /// 通过加载方式同步加载
        /// </summary>
        public T LoadInsByTypeSync<T>(EAssetLoadType loadType, string assetPath) where T : UObject
        {
            switch (loadType)
            {
                case EAssetLoadType.AssetBundle:
                    {
                        return LoadInsSync<T>(assetPath);
                    }
                case EAssetLoadType.Resources:
                    {
                        return LoadResourceInsSync<T>(assetPath);
                    }
                default:
                    {
                        Log.ErrorForce($"LoadInsByTypeSync 未处理的加载方式: `{loadType}`");
                        return null;
                    }
            }
        }

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
        /// 修正资源路径，添加前缀
        /// </summary>
        private string ModifyAssetPath(string assetPath)
        {
            return $"{Path.ASSET_PATH_PREFIX}{assetPath}";
        }

        protected override void OnInit()
        {
            CreateAssetLoader();
        }

        private void CreateAssetLoader()
        {
            void SetAssetLoader(EAssetLoaderType assetLoaderType)
            {
#if UNITY_EDITOR
                GameConfig.Instance.editorAssetLoaderType = assetLoaderType;
#else
                GameConfig.Instance.runtimeAssetLoaderType = assetLoaderType;
#endif
                switch (assetLoaderType)
                {
                    case EAssetLoaderType.AssetDatabase:
                        Log.Info("使用EditorAssetLoader资源加载器");
                        loader = new EditorAssetLoader();
                        break;
                    case EAssetLoaderType.AssetBundle:
                        Log.Info("使用ABAssetLoader资源加载器");
                        loader = new ABAssetLoader();
                        break;
                    case EAssetLoaderType.Addressables:
                        Log.Info("使用ABAssetLoader资源加载器");
                        loader = new ABAssetLoader();
                        break;
                }
            }

            if (loader == null)
            {
                try
                {
#if UNITY_EDITOR
                    switch (GameConfig.Instance.editorAssetLoaderType)
#else
                switch (GameConfig.Instance.runtimeAssetLoaderType)
#endif
                    {
                        case EAssetLoaderType.AssetDatabase:
#if UNITY_EDITOR
                            SetAssetLoader(EAssetLoaderType.AssetDatabase);
                            break;
#else
                        throw CommonException.Create("运行时不可使用AssetDatabase类型资源加载器");
#endif
                        case EAssetLoaderType.AssetBundle:
                            SetAssetLoader(EAssetLoaderType.AssetBundle);
                            break;
                        case EAssetLoaderType.Addressables:
                            throw CommonException.Create("Addressables资源加载器未实现");
                        default:
#if UNITY_EDITOR
                            throw CommonException.Create($"GameConfig.editorAssetLoaderType类型错误: {GameConfig.Instance.editorAssetLoaderType}");
#else
                        throw CommonException.Create($"GameConfig.runtimeAssetLoaderType类型错误: {GameConfig.Instance.runtimeAssetLoaderType}");
#endif
                    }
                }
                catch (Exception e)
                {
                    Assert.ExceptHandle(e, "创建资源加载器异常, 使用对应环境默认资源加载器");
#if UNITY_EDITOR
                    SetAssetLoader(EAssetLoaderType.AssetDatabase);
#else
                    SetAssetLoader(eAssetLoaderType.AssetBundle);
#endif
                }
            }
        }

        protected override void OnDispose()
        {
            loader?.Dispose();
            loader = null;
        }
    }
}