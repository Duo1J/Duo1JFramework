using UnityEngine;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源句柄接口
    /// </summary>
    /// <see cref="AssetCollection"/>
    public interface IAssetHandle<T> : IDispose where T : Object
    {
        /// <summary>
        /// 资源引用
        /// </summary>
        T Asset { get; }

        /// <summary>
        /// 是否已释放
        /// </summary>
        bool Released { get; }

        /// <summary>
        /// 资源实例化
        /// </summary>
        /// <returns></returns>
        T Instantiate();

        /// <summary>
        /// 检查是否异常
        /// </summary>
        bool Error();

        /// <summary>
        /// 释放资源句柄
        /// </summary>
        void Release();
    }
}
