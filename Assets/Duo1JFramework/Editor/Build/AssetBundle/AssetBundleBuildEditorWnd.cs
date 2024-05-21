using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// AssetBundle包构建编辑器窗口
    /// </summary>
    public class AssetBundleBuildEditorWnd : EditorWindowBase<AssetBundleBuildEditorWnd>
    {
        private Vector2 scrollPos;

        private void OnGUI()
        {
            LU.Scroll(ref scrollPos, () =>
            {
                LU.Vertical(() =>
                {
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("构建AssetBundle"))
                    {
                        if (EditorUtility.DisplayDialog("", "是否执行构建AssetBundle", "确认", "取消"))
                        {
                            AssetBundleBuilder.BuildAllAssetBundle();
                        }
                    }
                });
            });
        }
    }
}