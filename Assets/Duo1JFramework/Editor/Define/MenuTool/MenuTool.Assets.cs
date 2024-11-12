using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// Assets菜单工具栏
    /// </summary>
    public partial class MenuTool
    {
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
