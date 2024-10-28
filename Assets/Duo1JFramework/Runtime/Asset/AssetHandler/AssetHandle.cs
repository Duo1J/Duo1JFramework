using UnityEngine;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 默认资源句柄
    /// </summary>
    public abstract class AssetHandle<T> : IAssetHandle<T> where T : Object
    {
        /// <summary>
        /// 资源引用
        /// </summary>
        public T Asset { get; protected set; }

        /// <summary>
        /// 资源实例化
        /// </summary>
        public virtual T Instantiate()
        {
            if (Error())
            {
                Log.ErrorForce("资源为空, 无法实例化");
                return null;
            }

            return Object.Instantiate(Asset);
        }

        /// <summary>
        /// 检查是否异常
        /// </summary>
        public virtual bool Error()
        {
            return Asset == null;
        }

        /// <summary>
        /// 释放句柄
        /// </summary>
        public virtual void Release()
        {
            Asset = null;
        }

        protected AssetHandle(T asset)
        {
            Asset = asset;
        }

        public void Dispose() => Release();
    }
}
