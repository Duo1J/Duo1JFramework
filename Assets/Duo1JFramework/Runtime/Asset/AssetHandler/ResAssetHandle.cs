using UnityEngine;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// Resource资源句柄
    /// </summary>
    public class ResAssetHandle<T> : AssetHandle<T> where T : Object
    {
        public static ResAssetHandle<T> Create(T asset)
        {
            return new ResAssetHandle<T>(asset);
        }

        public ResAssetHandle(T asset) : base(asset)
        {
        }
    }
}
