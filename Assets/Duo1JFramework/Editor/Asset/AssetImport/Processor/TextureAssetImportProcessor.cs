using System;
using UnityEditor;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 贴图资源导入处理器
    /// </summary>
    public class TextureAssetImportProcessor : BaseAssetImportProcessor
    {
        public override Type ImporterType => typeof(TextureImporter);

        protected override void OnProcess(AssetImporter importer, string assetPath)
        {
            base.OnProcess(importer, assetPath);
        }
    }
}
