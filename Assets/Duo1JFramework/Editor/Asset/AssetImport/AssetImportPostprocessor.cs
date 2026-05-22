using UnityEditor;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源导入后处理入口
    /// </summary>
    public class AssetImportPostprocessor : AssetPostprocessor
    {
        private void OnPreprocessAsset()
        {
            AssetImportManager.Process(assetImporter, assetPath);
        }
    }
}
