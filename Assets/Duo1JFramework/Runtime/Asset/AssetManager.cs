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
        /// <summary>
        /// 资源加载器
        /// </summary>
        private IAssetLoader loader;

        /// <summary>
        /// 当前设置的资源包加载类型
        /// </summary>
        public EAssetLoadType BundleLoadType
        {
            get
            {
                if (bundleLoadType == EAssetLoadType.Bundle)
                {
#if UNITY_EDITOR
                    switch (GameOption.Editor.assetLoaderType)
#else
                    switch (GameOption.Runtime.assetLoaderType)
#endif
                    {
                        case EAssetLoaderType.AssetDatabase:
                        case EAssetLoaderType.AssetBundle:
                            bundleLoadType = EAssetLoadType.AssetBundle;
                            break;
                        case EAssetLoaderType.Addressables:
                            bundleLoadType = EAssetLoadType.Addressables;
                            break;
                    }
                }
                return bundleLoadType;
            }
        }
        private EAssetLoadType bundleLoadType = EAssetLoadType.Bundle;

        /// <summary>
        /// 通过加载方式加载
        /// </summary>
        public void LoadByType<T>(EAssetLoadType loadType, string assetPath, Action<T> callback) where T : UObject
        {
            PreprocessLoadType(ref loadType);
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
            PreprocessLoadType(ref loadType);
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
            PreprocessLoadType(ref loadType);
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
            PreprocessLoadType(ref loadType);
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
        /// 资源加载方式预处理
        /// </summary>
        public void PreprocessLoadType(ref EAssetLoadType loadType)
        {
            if (loadType == EAssetLoadType.Bundle)
            {
                loadType = BundleLoadType;
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
            return $"{Def.Path.ASSET_PATH_PREFIX}{assetPath}";
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
                GameOption.Editor.assetLoaderType = assetLoaderType;
#else
                GameOption.Runtime.assetLoaderType = assetLoaderType;
#endif
                switch (assetLoaderType)
                {
                    case EAssetLoaderType.AssetDatabase:
                        Log.Info("使用`EditorAssetLoader`资源加载器");
                        loader = new EditorAssetLoader();
                        break;
                    case EAssetLoaderType.AssetBundle:
                        Log.Info("使用`ABAssetLoader`资源加载器");
                        loader = new ABAssetLoader();
                        break;
                    case EAssetLoaderType.Addressables:
                        Log.Info("使用`ABAssetLoader`资源加载器");
                        loader = new ABAssetLoader();
                        break;
                }
            }

            if (loader == null)
            {
                try
                {
#if UNITY_EDITOR
                    switch (GameOption.Editor.assetLoaderType)
#else
                    switch (GameOption.Runtime.assetLoaderType)
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
                            throw CommonException.Create($"GameOption.editor.assetLoaderType类型错误: {GameOption.Editor.assetLoaderType}");
#else
                        throw CommonException.Create($"GameOption.runtime.assetLoaderType类型错误: {GameOption.Runtime.assetLoaderType}");
#endif
                    }
                }
                catch (Exception e)
                {
                    Assert.ExceptHandle(e, "创建资源加载器异常, 使用对应环境默认资源加载器");
#if UNITY_EDITOR
                    SetAssetLoader(EAssetLoaderType.AssetDatabase);
#else
                    SetAssetLoader(EAssetLoaderType.AssetBundle);
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