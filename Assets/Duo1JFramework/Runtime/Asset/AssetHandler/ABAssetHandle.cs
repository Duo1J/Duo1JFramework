using UnityEngine;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// AssetBundle资源句柄
    /// </summary>
    public class ABAssetHandle<T> : AssetHandle<T> where T : Object
    {
        /// <summary>
        /// AssetBundle数据
        /// </summary>
        public ABData ABData { get; private set; }

        /// <summary>
        /// 资源路径
        /// </summary>
        public string AssetPath { get; private set; }

        /// <summary>
        /// 释放句柄
        /// </summary>
        public override void Release()
        {
            base.Release();

            if (ABData != null && AssetPath != null)
            {
                ABData.UnloadAsset(AssetPath);
            }

            ABData = null;
            AssetPath = null;
        }

        /// <summary>
        /// 创建资源句柄
        /// </summary>
        public static ABAssetHandle<T> Create(T asset, ABData abData, string assetPath)
        {
            ABAssetHandle<T> handle = new ABAssetHandle<T>(asset);
            handle.ABData = abData;
            handle.AssetPath = assetPath;
            return handle;
        }

        public ABAssetHandle(T asset) : base(asset)
        {
        }
    }
}
