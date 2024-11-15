using System.IO;
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

            if (CheckCompiling(true, true))
            {
                return;
            }

            DrawBuildInfo();
            DrawBottomButton();
        }

        private void DrawBuildInfo()
        {
            ED.Scroll(ref scrollPos, () =>
            {
                strategy.PipelineType = (EABPipelineType)EditorGUILayout.EnumPopup("管线类型", strategy.PipelineType);
                strategy.BuildTarget = (BuildTarget)EditorGUILayout.EnumPopup("构建目标", strategy.BuildTarget);
                strategy.BuildOptions = (BuildAssetBundleOptions)EditorGUILayout.EnumFlagsField("构建选项", strategy.BuildOptions);

                strategy.ABNameType = (EABNameType)EditorGUILayout.EnumPopup("AB包命名方式", strategy.ABNameType);
                strategy.BuildABCRC = EditorGUILayout.Toggle("构建CRC校验", strategy.BuildABCRC);

                GUILayout.Space(10);

                GUILayout.Label("构建策略");
                foreach (ABBuildStrategyData data in strategy.Data)
                {
                    GUILayout.Label($"<color={ES.GreenS}>{data.abName}包:</color>");
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

                if (GUILayout.Button($"解密 {Def.Path.ASSET_BUNDLE_MAP_DATA_NAME} (ABMapData)"))
                {
                    string srcPath = EditorUtility.OpenFilePanel($"请选择 {Def.Path.ASSET_BUNDLE_MAP_DATA_NAME} (ABMapData) 文件", Def.Path.DataPath, "*");
                    if (string.IsNullOrEmpty(srcPath))
                    {
                        return;
                    }

                    string tarPath = EditorUtility.SaveFilePanel($"请选择解密保存位置", null, "ABMapDataDecode.json", "json");
                    if (string.IsNullOrEmpty(tarPath))
                    {
                        return;
                    }

                    bool success = ABMapData.DecodeToFile(srcPath, tarPath);
                    if (success)
                    {
                        Log.Info($"解密ABMapData成功, 保存到: `{tarPath}`");
                    }
                    else
                    {
                        Log.ErrorForce($"解密ABMapData失败: `{srcPath}`");
                    }
                }

                ED.Horizontal(() =>
                {
                    if (GUILayout.Button("打开构建目标文件夹"))
                    {
                        string abRoot = PathUtil.GetAssetBundleRoot();
                        if (!string.IsNullOrEmpty(abRoot) && Directory.Exists(abRoot))
                        {
                            ProjectUtil.OpenExplorer(abRoot);
                        }
                        else
                        {
                            Log.EditorError("AssetBundle未构建，无法打开目标文件夹");
                        }
                    }

                    if (GUILayout.Button("定位到构建策略文件"))
                    {
                        ABBuildStrategy.Instance.SelectAsset();
                    }
                });

                if (GUILayout.Button("清理构建的AssetBundle"))
                {
                    if (EditorUtility.DisplayDialog("", "是否执行清理构建的AssetBundle", "确认", "取消"))
                    {
                        AssetBundleBuilder.ClearAllAssetBundleBuild();
                    }
                }

                ED.Color(ES.GreenL, () =>
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
