using Duo1JFramework.Asset;
using UnityEngine;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// 运行时Inspector和Hierarchy调试窗口
    /// </summary>
    public class RuntimeInspectorWnd : GenericWindow<RuntimeInspectorWnd>
    {
        protected override UIData CreateUIConfig()
        {
            return new UIData($"{Def.Path.RES_PATH_PREFIX}UI/_RuntimeInspectorWnd")
                .SetLoadType(EAssetLoadType.Resources)
                .SetLayer(EUILayer.Const);
        }

        protected override void OnInit()
        {
            GameObject rumtimeHierarchyGo = GetGo("RuntimeHierarchy");
            rumtimeHierarchyGo.RectTF()?.BindDragPanel();

            GameObject rumtimeInspectorGo = GetGo("RuntimeInspector");
            rumtimeInspectorGo.RectTF()?.BindDragPanel();
        }
    }
}
