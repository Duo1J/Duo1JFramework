using Duo1JFramework.Asset;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// App构建编辑器
    /// </summary>
    public class AppBuildEditorWnd : BaseEditorWindow<AppBuildEditorWnd>
    {
        private Vector2 scrollPos;

        private AppBuildStrategy strategy;

        private void OnGUI()
        {
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
                ED.Vertical(() =>
                {
                    strategy.buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("构建目标", strategy.buildTarget);
                    strategy.buildOptions = (BuildOptions)EditorGUILayout.EnumFlagsField("构建选项", strategy.buildOptions);

                    GUILayout.Space(10);
                    strategy.buildAsset = EditorGUILayout.Toggle("构建资源", strategy.buildAsset);
                    strategy.assetLoaderType = (EAssetLoaderType)EditorGUILayout.EnumPopup("资源加载器类型", strategy.assetLoaderType);

                    if (strategy.assetLoaderType == EAssetLoaderType.AssetBundle)
                    {
                        strategy.ABData.copyAsset = EditorGUILayout.Toggle("拷贝资源到运行时目录", strategy.ABData.copyAsset);
                        if (strategy.ABData.copyAsset)
                        {
                            if (strategy.assetLoaderType == EAssetLoaderType.AssetBundle)
                            {
                                strategy.ABData.deleteManifest = EditorGUILayout.Toggle("删除Manifest文件", strategy.ABData.deleteManifest);
                            }
                        }
                    }
                });
            });
        }

        private void DrawBottomButton()
        {
            ED.Vertical(() =>
            {
                GUILayout.FlexibleSpace();

                if (strategy.assetLoaderType == EAssetLoaderType.AssetBundle && GUILayout.Button("清理Manifest文件"))
                {
                    if (EditorUtility.DisplayDialog("", "是否执行清理清理Manifest文件", "确认", "取消"))
                    {
                        AssetBundleBuilder.DeleteAllManifestCopy();
                    }
                }

                if (GUILayout.Button("打开资源构建面板"))
                {
                    OpenAssetBuildWnd(strategy.assetLoaderType);
                }

                if (GUILayout.Button("定位到构建策略文件"))
                {
                    AppBuildStrategy.Instance.SelectAsset();
                }

                ED.Color(ES.GreenL, () =>
                {
                    if (GUILayout.Button("构建App"))
                    {
                        if (!EditorUtil.CheckPlatformChgAndAsk(AppBuildStrategy.Instance.buildTarget))
                        {
                            return;
                        }

                        string tarPath = EditorUtility.SaveFolderPanel("选择App构建目标路径", null, "");
                        if (string.IsNullOrEmpty(tarPath))
                        {
                            Log.EditorError("App构建目标路径不可为空");
                        }
                        else
                        {
                            tarPath = $"{tarPath}/{AppBuilder.BuildTarFolderName}";
                            if (EditorUtility.DisplayDialog("", $"是否执行构建App到 {tarPath}", "确认", "取消"))
                            {
                                AppBuilder.BuildApp(tarPath);
                            }
                        }
                    }
                });
            });
        }

        protected override void LoadData()
        {
            base.LoadData();
            strategy = AppBuildStrategy.Instance;
        }

        protected override void SaveData()
        {
            base.SaveData();
            if (strategy != null)
            {
                EditorUtility.SetDirty(strategy);
                EditorUtil.SaveAndRefresh("AppBuildEditorWnd::SaveData");
            }
        }

        /// <summary>
        /// 打开资源构建面板
        /// </summary>
        private void OpenAssetBuildWnd(EAssetLoaderType assetLoaderType)
        {
            switch (assetLoaderType)
            {
                case EAssetLoaderType.AssetDatabase:
                {
                    Log.EditorError($"AssetDatabase无资源构建面板");
                    break;
                }
                case EAssetLoaderType.AssetBundle:
                {
                    AssetBundleBuildEditorWnd.Open();
                    break;
                }
                case EAssetLoaderType.Addressables:
                {
                    Log.EditorError($"Addressables暂未实现资源构建面板");
                    break;
                }
                default:
                {
                    Log.EditorError($"资源加载器类型错误: {assetLoaderType}");
                    break;
                }
            }
        }
    }
}