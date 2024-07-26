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

                    data.buildAsset = EditorGUILayout.Toggle("构建资源", data.buildAsset);
                    data.assetLoaderType = (EAssetLoaderType)EditorGUILayout.EnumPopup("资源加载器类型", data.assetLoaderType);
                });
            });
        }

        private void DrawBottomButton()
        {
            ED.Vertical(() =>
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("定位到构建策略文件"))
                {
                    AppBuildStrategy.Instance.SelectAsset();
                }

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
                EditorUtil.SaveAndRefresh();
            }
        }
    }
}
