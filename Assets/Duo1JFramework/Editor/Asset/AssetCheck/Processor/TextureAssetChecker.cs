using System;
using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 贴图资源检查器
    /// </summary>
    public class TextureAssetChecker : BaseAssetChecker
    {
        public override Type AssetType => typeof(Texture2D);

        protected override void OnCheck(UObject asset, string assetPath, AssetCheckResult result)
        {
        }
    }
}
