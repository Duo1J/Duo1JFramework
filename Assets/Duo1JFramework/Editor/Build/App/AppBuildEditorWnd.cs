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
            DrawBuildInfo();
            DrawBottomButton();
        }

        private void DrawBuildInfo()
        {
            ED.Scroll(ref scrollPos, () =>
            {
                ED.Vertical(() =>
                {
                    AppBuildStrategyData data = strategy.Data;

                    data.buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("构建目标", data.buildTarget);
                    data.buildOptions = (BuildOptions)EditorGUILayout.EnumFlagsField("构建选项", data.buildOptions);

                    GUILayout.Space(10);
                    data.buildAsset = EditorGUILayout.Toggle("构建资源", data.buildAsset);
                    data.assetLoaderType = (EAssetLoaderType)EditorGUILayout.EnumPopup("资源加载器类型", data.assetLoaderType);

                    data.copyAsset = EditorGUILayout.Toggle("拷贝资源到运行时目录", data.copyAsset);
                    if (data.assetLoaderType == EAssetLoaderType.AssetBundle)
                    {
                        data.deleteManifest = EditorGUILayout.Toggle("删除Manifest文件", data.deleteManifest);
                    }
                });
            });
        }

        private void DrawBottomButton()
        {
            ED.Vertical(() =>
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("打开资源构建面板"))
                {
                    OpenAssetBuildWnd(strategy.Data.assetLoaderType);
                }

                if (strategy.Data.assetLoaderType == EAssetLoaderType.AssetBundle && GUILayout.Button("清理Manifest文件"))
                {
                    if (EditorUtility.DisplayDialog("", "是否执行清理清理Manifest文件", "确认", "取消"))
                    {
                        AssetBundleBuilder.DeleteAllManifestCopy();
                    }
                }

                if (GUILayout.Button("定位到构建策略文件"))
                {
                    AppBuildStrategy.Instance.SelectAsset();
                }

                ED.SurrondColor(ES.GreenL, () =>
                {
                    if (GUILayout.Button("构建App"))
                    {
                        if (!EditorUtil.CheckPlatformChgAndAsk(AppBuildStrategy.Instance.Data.buildTarget))
                        {
                            return;
                        }

                        string tarPath = EditorUtility.SaveFolderPanel("选择App构建目标路径", "", "");
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
