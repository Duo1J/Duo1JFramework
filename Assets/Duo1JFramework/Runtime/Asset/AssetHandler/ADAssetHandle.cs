using UnityEngine;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// AssetDatabase资源句柄
    /// </summary>
    public class ADAssetHandle<T> : AssetHandle<T> where T : Object
    {
        public static ADAssetHandle<T> Create(T asset)
        {
            return new ADAssetHandle<T>(asset);
        }

        public ADAssetHandle(T asset) : base(asset)
        {
        }
    }
}