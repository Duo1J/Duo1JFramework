using Duo1JFramework.Config;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// Addressables加载器
    /// </summary>
    public class ADAssetLoader : BaseAssetLoader
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

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetPath);
            handle.Completed += (operationHandle) =>
            {
                if (operationHandle.Status != AsyncOperationStatus.Succeeded || operationHandle.Result == null)
                {
                    Log.ErrorForce($"异步加载Addressables资源失败: `{assetPath}`");
                    if (operationHandle.IsValid())
                    {
                        Addressables.Release(operationHandle);
                    }

                    callback(null);
                    return;
                }

                callback(ADAssetHandle<T>.Create(operationHandle));
            };
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

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetPath);
            T asset = handle.WaitForCompletion();
            if (handle.Status != AsyncOperationStatus.Succeeded || asset == null)
            {
                Log.ErrorForce($"同步加载Addressables资源失败: `{assetPath}`");
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }

                return null;
            }

            return ADAssetHandle<T>.Create(handle);
        }

        /// <summary>
        /// 检查编辑器下资源加载器的设置类型
        /// </summary>
        private bool CheckEditorAssetLoaderType()
        {
#if UNITY_EDITOR
            if (GameOption.AssetLoaderType != EAssetLoaderType.Addressables)
            {
                Log.EditorError("编辑器下使用ADAssetLoader请设置GameOption.Editor.assetLoaderType为Addressables类型");
                return false;
            }

            return true;
#else
            return true;
#endif
        }
    }
}
