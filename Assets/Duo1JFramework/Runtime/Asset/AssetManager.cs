using Duo1JFramework.Config;
using System;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    /// <see cref="AssetCollection"/>
    public class AssetManager : MonoSingleton<AssetManager>, IAssetLoadable
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
                    switch (GameOption.AssetLoaderType)
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
        /// 通过加载方式异步加载
        /// </summary>
        public void LoadByType<T>(string assetPath, Action<IAssetHandle<T>> callback, EAssetLoadType loadType = EAssetLoadType.Bundle) where T : UObject
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
        /// 通过加载方式同步加载
        /// </summary>
        public IAssetHandle<T> LoadByTypeSync<T>(string assetPath, EAssetLoadType loadType = EAssetLoadType.Bundle) where T : UObject
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
        public void Load<T>(string assetPath, Action<IAssetHandle<T>> callback) where T : UObject
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            assetPath = ModifyAssetPath(assetPath);

            loader.Load<T>(assetPath, callback);
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public IAssetHandle<T> LoadSync<T>(string assetPath) where T : UObject
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            assetPath = ModifyAssetPath(assetPath);

            return loader.LoadSync<T>(assetPath);
        }

        /// <summary>
        /// 异步加载Resources资源
        /// </summary>
        public void LoadResource<T>(string assetPath, Action<IAssetHandle<T>> callback) where T : UObject
        {
            loader.LoadResource<T>(assetPath, callback);
        }

        /// <summary>
        /// 同步加载Resources资源
        /// </summary>
        public IAssetHandle<T> LoadResourceSync<T>(string assetPath) where T : UObject
        {
            return loader.LoadResourceSync<T>(assetPath);
        }

        /// <summary>
        /// 资源垃圾清理
        /// </summary>
        public void GC()
        {
            if (loader != null)
            {
                if (loader is ABAssetLoader)
                {
                    ABManager.Instance.GC();
                }
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
            if (loader == null)
            {
                try
                {
                    switch (GameOption.AssetLoaderType)
                    {
                        case EAssetLoaderType.AssetDatabase:
#if UNITY_EDITOR
                            SetAssetLoader(EAssetLoaderType.AssetDatabase);
                            break;
#else
                        throw Except.Create("运行时不可使用AssetDatabase类型资源加载器");
#endif
                        case EAssetLoaderType.AssetBundle:
                            SetAssetLoader(EAssetLoaderType.AssetBundle);
                            break;
                        case EAssetLoaderType.Addressables:
                            throw Except.Create("Addressables资源加载器未实现");
                        default:
#if UNITY_EDITOR
                            throw Except.Create($"GameOption.editor.assetLoaderType类型错误: {GameOption.AssetLoaderType}");
#else
                            throw Except.Create($"GameOption.runtime.assetLoaderType类型错误: {GameOption.AssetLoaderType}");
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

        private void SetAssetLoader(EAssetLoaderType assetLoaderType)
        {
            GameOption.AssetLoaderType = assetLoaderType;
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

        protected override void OnDispose()
        {
            loader?.Dispose();
            loader = null;
        }
    }
}
