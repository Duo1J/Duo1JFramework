using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// AssetBundle包构建编辑器窗口
    /// </summary>
    public class AssetBundleBuildEditorWnd : BaseEditorWindow<AssetBundleBuildEditorWnd>
    {
        private Vector2 scrollPos;

        private ABBuildStrategy strategy;

        private void OnGUI()
        {
            RichText = true;

            DrawBuildInfo();
            DrawBottomButton();
        }

        private void DrawBuildInfo()
        {
            ED.Scroll(ref scrollPos, () =>
            {
                strategy.BuildTarget = (BuildTarget)EditorGUILayout.EnumPopup("构建目标", strategy.BuildTarget);
                strategy.BuildOptions = (BuildAssetBundleOptions)EditorGUILayout.EnumFlagsField("构建选项", strategy.BuildOptions);

                GUILayout.Space(10);

                foreach (ABBuildStrategyData data in strategy.Data)
                {
                    GUILayout.Label($"<color={ES.GreenSL}>{data.abName}包:</color>");
                    foreach (string path in data.pathList)
                    {
                        GUILayout.Label($"{ED.S8}{path}");
                    }
                }
            });
        }

        private void DrawBottomButton()
        {
            ED.Vertical(() =>
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("定位到构建策略文件"))
                {
                    ABBuildStrategy.Instance.SelectAsset();
                }

                if (GUILayout.Button("清理构建的AssetBundle"))
                {
                    if (EditorUtility.DisplayDialog("", "是否执行清理构建的AssetBundle", "确认", "取消"))
                    {
                        AssetBundleBuilder.ClearAllAssetBundleBuild();
                    }
                }

                ED.SurrondColor(ES.GreenL, () =>
                {
                    if (GUILayout.Button("构建AssetBundle"))
                    {
                        if (EditorUtility.DisplayDialog("", "是否执行构建AssetBundle", "确认", "取消"))
                        {
                            if (!EditorUtil.CheckPlatformChgAndAsk(ABBuildStrategy.Instance.BuildTarget))
                            {
                                return;
                            }

                            AssetBundleBuilder.BuildAllAssetBundle();
                        }
                    }
                });
            });
        }

        protected override void LoadData()
        {
            base.LoadData();
            strategy = ABBuildStrategy.Instance;
        }

        protected override void SaveData()
        {
            base.SaveData();
            if (strategy != null)
            {
                EditorUtility.SetDirty(strategy);
                EditorUtil.SaveAndRefresh("AssetBundleBuildEditorWnd::SaveData");
            }
        }
    }
}