using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// Addressables资源句柄
    /// </summary>
    public class ADAssetHandle<T> : AssetHandle<T> where T : Object
    {
        /// <summary>
        /// Addressables操作句柄
        /// </summary>
        public AsyncOperationHandle<T> OperationHandle { get; private set; }

        /// <summary>
        /// 是否持有Addressables操作句柄
        /// </summary>
        private bool hasOperationHandle;

        /// <summary>
        /// 释放句柄
        /// </summary>
        public override void Release()
        {
            if (Released)
            {
                return;
            }

            AsyncOperationHandle<T> operationHandle = OperationHandle;
            bool needRelease = hasOperationHandle;
            hasOperationHandle = false;

            base.Release();

            if (needRelease && operationHandle.IsValid())
            {
                Addressables.Release(operationHandle);
            }
        }

        /// <summary>
        /// 创建资源句柄
        /// </summary>
        public static ADAssetHandle<T> Create(T asset)
        {
            return new ADAssetHandle<T>(asset);
        }

        /// <summary>
        /// 创建资源句柄
        /// </summary>
        public static ADAssetHandle<T> Create(AsyncOperationHandle<T> operationHandle)
        {
            ADAssetHandle<T> handle = new ADAssetHandle<T>(operationHandle.Result);
            handle.OperationHandle = operationHandle;
            handle.hasOperationHandle = true;
            return handle;
        }

        public ADAssetHandle(T asset) : base(asset)
        {
        }
    }
}
