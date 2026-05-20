using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// Addressables构建器
    /// </summary>
    public class AddressablesBuilder
    {
        /// <summary>
        /// 构建所有Addressables资源
        /// </summary>
        public static bool BuildAllAddressables()
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Log.EditorError("Addressables设置不存在，请先创建Addressables Settings");
                return false;
            }

            AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
            if (!string.IsNullOrEmpty(result.Error))
            {
                Log.EditorError($"Addressables资源构建失败: {result.Error}");
                return false;
            }

            EditorUtil.SaveAndRefresh("AddressablesBuilder::BuildAllAddressables");
            Log.EditorInfo("Addressables资源构建成功");

            return true;
        }

        /// <summary>
        /// 清理Addressables构建缓存
        /// </summary>
        public static bool ClearAllAddressablesBuild()
        {
            AddressableAssetSettings.CleanPlayerContent();

            EditorUtil.SaveAndRefresh("AddressablesBuilder::ClearAllAddressablesBuild");
            Log.EditorInfo("Addressables构建缓存已清理");

            return true;
        }

        /// <summary>
        /// 拷贝Addressables资源
        /// </summary>
        public static bool CopyAllAddressablesBuild()
        {
            Log.EditorInfo("Addressables资源由Addressables构建系统管理，无需额外拷贝");
            return true;
        }

        private AddressablesBuilder()
        {
        }
    }
}
