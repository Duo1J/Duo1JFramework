using Duo1JFramework.Asset;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// Assets菜单工具栏
    /// </summary>
    public partial class MenuTool
    {
        #region AssetCheck 1

        [MenuItem(EditorDef.Menu.Assets.ASSET_CHECK, priority = 1)]
        public static void CheckAsset()
        {
            if (!EditorUtil.GetActiveObj(out Object obj))
            {
                return;
            }

            string assetPath = AssetDatabase.GetAssetPath(obj);
            AssetCheckResult result = AssetCheckManager.Check(obj, assetPath);
            EditorUtility.DisplayDialog("资产检查", result.GetMessage(), "确定");
        }

        [MenuItem(EditorDef.Menu.Assets.ASSET_CHECK, true)]
        public static bool CheckAssetValidate()
        {
            return Selection.activeObject != null && !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(Selection.activeObject));
        }

        #endregion AssetCheck 1

        #region Reference 10

        [MenuItem(EditorDef.Menu.Assets.REF_PREFIX + "引用资源列表", priority = 10)]
        public static void ReferenceAssetList()
        {
            if (EditorUtil.GetActiveObj(out Object obj))
            {
                ReferenceUtil.OpenReferenceAssetList(obj, false);
            }
        }

        [MenuItem(EditorDef.Menu.Assets.REF_PREFIX + "引用资源列表(递归)", priority = 11)]
        public static void ReferenceAssetListRecursive()
        {
            if (EditorUtil.GetActiveObj(out Object obj))
            {
                ReferenceUtil.OpenReferenceAssetList(obj, true);
            }
        }

        #endregion Reference 10
    }
}
