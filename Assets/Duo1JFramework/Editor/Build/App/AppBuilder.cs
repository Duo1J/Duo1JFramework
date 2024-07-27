using Duo1JFramework.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// App构建器
    /// </summary>
    public static class AppBuilder
    {
        /// <summary>
        /// App构建目标文件夹名
        /// </summary>
        public static string BuildTarFolderName => $"{Application.productName}_Out";

        /// <summary>
        /// 以AppBuildStrategy参数构建App
        /// </summary>
        public static void BuildApp(string tarPath)
        {
            Assert.NotNull(tarPath, "App构建目标路径不可为空");
            FileUtil.CheckDir(tarPath);

            AppBuildStrategyData data = AppBuildStrategy.Instance.Data;

            if (data.buildAsset)
            {
                if (!BuildAsset(data.buildTarget, data.assetLoaderType))
                {
                    return;
                }
            }

            if (data.copyAsset)
            {
                if (!CopyAsset(data.assetLoaderType))
                {
                    return;
                }
            }

            BuildPlayer(data, tarPath);
        }

        /// <summary>
        /// 构建Player
        /// </summary>
        private static void BuildPlayer(AppBuildStrategyData data, string tarPath)
        {
            try
            {
                Assert.NotNull(tarPath, "Player构建目标路径不可为空");
                FileUtil.CheckDir(tarPath);
                string tarPlayerFolder = tarPath + $"/{Application.productName}/";
                if (FileUtil.DeleteDir(tarPlayerFolder))
                {
                    Log.EditorInfo($"Player构建时，删除已存在文件夹: {tarPlayerFolder}");
                }

                string tarPlayerPath = tarPlayerFolder + $"{Application.productName}.exe";
                List<EditorBuildSettingsScene> buildSettingSceneList = GetBuildSettingSceneList();
                BuildPipeline.BuildPlayer(buildSettingSceneList.ToArray(), tarPlayerPath, data.buildTarget, data.buildOptions);

                ProjectViewUtil.OpenExplorer(tarPath);
                Log.EditorInfo($"Player构建成功: {tarPath}");
            }
            catch (Exception e)
            {
                Log.EditorError($"Player构建失败");
                Assert.ExceptHandle(e);
            }
        }

        /// <summary>
        /// 根据目标和类型构建资源
        /// </summary>
        public static bool BuildAsset(BuildTarget buildTarget, EAssetLoaderType assetLoaderType)
        {
            try
            {
                switch (assetLoaderType)
                {
                    case EAssetLoaderType.AssetDatabase:
                        {
                            Log.EditorInfo($"资源构建时，`{assetLoaderType.GetName()}`加载器类型无需构建");
                            return true;
                        }
                    case EAssetLoaderType.AssetBundle:
                        {
                            AssetBundleBuilder.BuildAllAssetBundle(buildTarget);
                            return true;
                        }
                    case EAssetLoaderType.Addressables:
                        {
                            Log.EditorError($"资源构建时，Addressables加载器类型未实现");
                            return false;
                        }
                    default:
                        {
                            Log.EditorError($"资源构建时，加载器类型错误: {assetLoaderType}");
                            return false;
                        }
                }
            }
            catch (Exception e)
            {
                Log.EditorError($"资源构建时，构建加载器类型`{assetLoaderType.GetName()}`的资源异常");
                Assert.ExceptHandle(e);
                return false;
            }
        }

        /// <summary>
        /// 根据类型拷贝资源到运行时目录
        /// </summary>
        public static bool CopyAsset(EAssetLoaderType assetLoaderType)
        {
            try
            {
                switch (assetLoaderType)
                {
                    case EAssetLoaderType.AssetDatabase:
                        {
                            Log.EditorInfo($"资源拷贝时，`{assetLoaderType.GetName()}`加载器类型无需拷贝");
                            return true;
                        }
                    case EAssetLoaderType.AssetBundle:
                        {
                            return AssetBundleBuilder.CopyAllAssetBundleBuild();
                        }
                    case EAssetLoaderType.Addressables:
                        {
                            Log.EditorError($"资源拷贝时，Addressables加载器类型未实现");
                            return false;
                        }
                    default:
                        {
                            Log.EditorError($"资源拷贝时，加载器类型错误: {assetLoaderType}");
                            return false;
                        }
                }
            }
            catch (Exception e)
            {
                Log.EditorError($"资源拷贝时，拷贝加载器类型`{assetLoaderType.GetName()}`的资源异常");
                Assert.ExceptHandle(e);
                return false;
            }
        }

        /// <summary>
        /// 命令行构建App
        /// </summary>
        public static void CommandBuildApp()
        {
        }

        #region Tool

        /// <summary>
        /// 获取设置中需构建的设置场景列表
        /// </summary>
        public static List<EditorBuildSettingsScene> GetBuildSettingSceneList()
        {
            IEnumerable<EditorBuildSettingsScene> buildSettingSceneList = EditorBuildSettings.scenes.Where(scene => scene.enabled);
            return new List<EditorBuildSettingsScene>(buildSettingSceneList);
        }

        /// <summary>
        /// 获取设置中需构建的场景列表
        /// </summary>
        public static List<Scene> GetBuildSceneList()
        {
            List<EditorBuildSettingsScene> buildSettingSceneList = GetBuildSettingSceneList();
            List<Scene> buildSceneList = new List<Scene>();

            foreach (EditorBuildSettingsScene buildSettingScene in buildSettingSceneList)
            {
                Scene scene = SceneManager.GetSceneByPath(buildSettingScene.path);
                if (scene != null)
                {
                    buildSceneList.Add(scene);
                }
            }

            return buildSceneList;
        }

        #endregion Tool
    }
}
