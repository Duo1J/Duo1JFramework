using Duo1JFramework.Asset;
using System.IO;
using System.Text;
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

        #region ScriptableObject 20

        [MenuItem(EditorDef.Menu.Assets.SCRIPTABLE_OBJECT_PREFIX + "导出为JSON", priority = 20)]
        public static void ExportScriptableObjectsToJson()
        {
            Object[] selected = Selection.objects;
            if (selected == null || selected.Length == 0)
            {
                Log.Warn("未选中任何ScriptableObject");
                return;
            }

            string dir = EditorUtility.OpenFolderPanel("选择导出目录", Application.dataPath, "");
            if (string.IsNullOrEmpty(dir))
            {
                Log.Error("选中导出目录为空");
                return;
            }

            int successCount = 0;
            UTF8Encoding utf8NoBom = new UTF8Encoding(false);
            foreach (Object obj in selected)
            {
                if (!(obj is ScriptableObject so))
                {
                    Log.Warn($"跳过非ScriptableObject对象: {obj.name}");
                    continue;
                }

                string json = JsonUtility.ToJson(so, true);
                string filePath = Path.Combine(dir, so.name + ".json");
                File.WriteAllText(filePath, json, utf8NoBom);
                successCount++;
            }

            Log.Info($"导出ScriptableObject为JSON完成，成功{successCount}/{selected.Length}个，目录：{dir}");
        }

        [MenuItem(EditorDef.Menu.Assets.SCRIPTABLE_OBJECT_PREFIX + "导出为JSON", true)]
        public static bool ExportScriptableObjectsToJsonValidate()
        {
            Object[] selected = Selection.objects;
            if (selected == null || selected.Length == 0)
            {
                return false;
            }

            foreach (Object obj in selected)
            {
                if (obj is ScriptableObject)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion ScriptableObject 20
    }
}
