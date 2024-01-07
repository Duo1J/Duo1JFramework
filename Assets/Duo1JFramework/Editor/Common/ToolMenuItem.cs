using Duo1JFramework.UI;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器工具栏菜单
    /// </summary>
    public static class ToolMenuItem
    {
        #region Path

        [MenuItem(EditorDef.TOOL_PATH_PREFIX + "UI节点快速命名 &1", priority = 10)]
        public static void UINodeFastName()
        {
            if (EditorUtil.GetActiveGo(out GameObject go))
            {
                if (!go.name.StartsWith(UIController.NodePrefix))
                {
                    go.RecordObject("UI node rename").name = UIController.NodePrefix + go.name;
                }
            }
        }

        #endregion Path
    }
}