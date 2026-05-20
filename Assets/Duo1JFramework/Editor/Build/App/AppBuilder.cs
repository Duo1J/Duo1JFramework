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
    public class AppBuilder
    {
        /// <summary>
        /// App构建目标文件夹名
        /// </summary>
        public static readonly string BuildTarFolderName = $"{Application.productName}_Out";

        /// <summary>
        /// 以AppBuildStrategy参数构建App
        /// </summary>
        /// <param name="tarPath">目标文件夹, 需要在外层拼接上`/{AppBuilder.BuildTarFolderName}`</param>
        public static bool BuildApp(string tarPath)
        {
            Assert.NotNullArg(tarPath, "tarPath");
            FileUtil.CheckDir(tarPath);

            AppBuildStrategy strategy = AppBuildStrategy.Instance;

            if (strategy.buildAsset)
            {
                if (!BuildAsset(strategy.buildTarget, strategy.assetLoaderType))
                {
                    return false;
                }
            }

            if (strategy.ABData.copyAsset)
            {
                if (!CopyAsset(strategy.assetLoaderType))
                {
                    return false;
                }

                if (strategy.assetLoaderType == EAssetLoaderType.AssetBundle && strategy.ABData.deleteManifest)
                {
                    AssetBundleBuilder.DeleteAllManifestCopy();
                }
            }

            return BuildPlayer(strategy, tarPath);
        }

        /// <summary>
        /// 构建Player
        /// </summary>
        private static bool BuildPlayer(AppBuildStrategy strategy, string tarPath)
        {
            try
            {
                Assert.NotNullArg(tarPath, "tarPath");
                FileUtil.CheckDir(tarPath);

                string tarPlayerFolder = $"{tarPath}/{Application.productName}/";
                if (FileUtil.DeleteDir(tarPlayerFolder))
                {
                    Log.EditorInfo($"Player构建时，删除已存在的文件夹: `{tarPlayerFolder}`");
                }

                string tarPlayerPath = tarPlayerFolder + $"{Application.productName}.exe";
                List<EditorBuildSettingsScene> buildSettingSceneList = GetBuildSettingSceneList();
                BuildPipeline.BuildPlayer(buildSettingSceneList.ToArray(), tarPlayerPath, strategy.buildTarget, strategy.buildOptions);

                ProjectUtil.OpenExplorer(tarPath);
                Log.EditorInfo($"Player构建成功: `{tarPath}`");

                return true;
            }
            catch (Exception e)
            {
                Log.EditorError($"Player构建失败");
                Assert.ExceptHandle(e);
                return false;
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
                    case EAssetLoaderType.AssetBundle:
                        {
                            AssetBundleBuilder.BuildAllAssetBundle(buildTarget, ABBuildStrategy.Instance.PipelineType);
                            return true;
                        }
                    case EAssetLoaderType.Addressables:
                        {
                            return AddressablesBuilder.BuildAllAddressables();
                        }
                    default:
                        {
                            Log.EditorError($"资源构建时，加载器类型错误: `{assetLoaderType}`");
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
                    case EAssetLoaderType.AssetBundle:
                        {
                            return AssetBundleBuilder.CopyAllAssetBundleBuild();
                        }
                    case EAssetLoaderType.Addressables:
                        {
                            return AddressablesBuilder.CopyAllAddressablesBuild();
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

        private AppBuilder()
        {
        }
    }
}
