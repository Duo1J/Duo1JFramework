using Duo1JFramework.Config;
using System;
using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 运行时AssetBundle加载器
    /// </summary>
    public class ABAssetLoader : BaseAssetLoader
    {
        /// <summary>
        /// 异步加载
        /// </summary>
        public override void Load<T>(string assetPath, Action<IAssetHandle<T>> callback)
        {
#if UNITY_EDITOR
            if (!CheckEditorAssetLoaderType())
            {
                callback(null);
                return;
            }
#endif

            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            Assert.NotNullArg(callback, "callback");

            ABData abData = ABManager.Instance.GetABDataByAsset(assetPath);
            if (abData == null)
            {
                Log.ErrorForce($"加载资源`{assetPath}`时，无法获取其对应的ABData");
                callback(null);
                return;
            }

            abData.Load<T>(assetPath, (asset) =>
            {
                ABAssetHandle<T> handle = ABAssetHandle<T>.Create(asset, abData, assetPath);
                callback(handle);
            });
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public override IAssetHandle<T> LoadSync<T>(string assetPath)
        {
#if UNITY_EDITOR
            if (!CheckEditorAssetLoaderType())
            {
                return null;
            }
#endif

            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");

            ABData abData = ABManager.Instance.GetABDataByAsset(assetPath);
            if (abData == null)
            {
                Log.ErrorForce($"加载资源`{assetPath}`时，无法获取其对应的ABData");
                return null;
            }

            T asset = abData.LoadSync<T>(assetPath);
            ABAssetHandle<T> handle = ABAssetHandle<T>.Create(asset, abData, assetPath);
            return handle;
        }

        /// <summary>
        /// 检查编辑器下资源加载器的设置类型
        /// </summary>
        private bool CheckEditorAssetLoaderType()
        {
#if UNITY_EDITOR
            if (GameOption.AssetLoaderType != EAssetLoaderType.AssetBundle)
            {
                Log.EditorError("编辑器下使用ABAssetLoader请设置GameOption.Editor.assetLoaderType为AssetBundle类型");
                return false;
            }

            return true;
#else
            return true;
#endif
        }
    }
}