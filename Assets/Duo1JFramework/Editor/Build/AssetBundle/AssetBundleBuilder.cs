using System;
using System.Collections.Generic;
using UnityEditor;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// AssetBundle构建器
    /// </summary>
    public static class AssetBundleBuilder
    {
        /// <summary>
        /// 使用ABBuildStrategy配置的目标构建所有AssetBundle
        /// </summary>
        /// <see cref="ABBuildStrategy"/>
        public static void BuildAllAssetBundle()
        {
            BuildAllAssetBundle(ABBuildStrategy.Instance.BuildTarget);
        }

        /// <summary>
        /// 构建所有AssetBundle
        /// </summary>
        public static void BuildAllAssetBundle(BuildTarget buildTarget)
        {
            ClearAllAssetBundle();

            ABBuildStrategyData[] strategyDatas = ABBuildStrategy.Instance.Data;

            if (strategyDatas == null || strategyDatas.Length == 0)
            {
                Log.EditorError($"AB构建策略配置为空: `{ABBuildStrategy.AssetPath}`");
                ABBuildStrategy.Instance.SelectAsset();
                return;
            }

            ABBuildData[] buildDatas = StrategyToBuildData(strategyDatas);

            if (buildDatas == null || buildDatas.Length == 0)
            {
                Log.EditorError($"AB构建数据为空，请检查策略配置: `{ABBuildStrategy.AssetPath}`");
                ABBuildStrategy.Instance.SelectAsset();
                return;
            }

            List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();
            Dictionary<string, List<string>> ab2AssetMap = new Dictionary<string, List<string>>();

            foreach (ABBuildData buildData in buildDatas)
            {
                if (ab2AssetMap.ContainsKey(buildData.abName))
                {
                    Log.EditorError($"AssetBundle包名重复: {buildData.abName}");
                    continue;
                }
                ab2AssetMap.Add(buildData.abName, buildData.assetPathList);

                buildList.Add(buildData.ToAssetBundleBuild());
            }

            try
            {
                EditorUtility.DisplayProgressBar("构建AssetBndle", "正在构建AssetBundle...", 0.3f);

                BuildPipeline.BuildAssetBundles(
                    PathUtil.GetAssetBundleRoot().CheckDir(),
                    buildList.ToArray(),
                    ABBuildStrategy.Instance.BuildOptions,
                    buildTarget
                );

                ABMapData.Save(ab2AssetMap);
                Log.EditorInfo($"构建{buildTarget.GetName()}平台的AssetBndle成功");
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, "AssetBundle构建异常");
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                EditorUtil.SaveAndRefresh();
            }
        }

        /// <summary>
        /// 清理所有构建的AssetBundle
        /// </summary>
        public static void ClearAllAssetBundle()
        {
            FileUtil.DeleteDir(PathUtil.GetAssetBundleRoot());
            FileUtil.DeleteFile(PathUtil.GetAssetBundleRootMeta());
            EditorUtil.SaveAndRefresh();
        }

        /// <summary>
        /// 命令行构建所有AssetBundle
        /// </summary>
        public static void CommandBuildAllAssetBundle()
        {

        }

        /// <summary>
        /// 将策略数据转为构建数据
        /// </summary>
        public static ABBuildData[] StrategyToBuildData(ABBuildStrategyData[] strategyDatas)
        {
            List<ABBuildData> ret = new List<ABBuildData>();

            if (strategyDatas == null || strategyDatas.Length == 0)
            {
                return ret.ToArray();
            }

            string pathPrefix = Def.Path.ASSET_FULL_PATH_PREFIX;
            foreach (ABBuildStrategyData strategyData in strategyDatas)
            {
                if (!strategyData.CheckValiad())
                {
                    Log.EditorError($"AB策略配置项无效，abName: {strategyData.abName}");
                    continue;
                }

                ABBuildData buildData = new ABBuildData(strategyData.abName);

                foreach (string path in strategyData.pathList)
                {
                    string path_ = PathUtil.CorrectPath(path);
                    List<string> resultList = FileUtil.GetFileInDir(pathPrefix + path_, (p) =>
                    {
                        if (p.EndsWith(Def.Path.META_SUFFIX))
                        {
                            return null;
                        }

                        return p.Replace(pathPrefix, Def.Path.ASSET_PATH_PREFIX);
                    });

                    if (buildData.assetPathList == null || buildData.assetPathList.Count == 0)
                    {
                        buildData.assetPathList = resultList;
                    }
                    else
                    {
                        List<string> assetPathList = buildData.assetPathList;
                        foreach (string item in resultList)
                        {
                            assetPathList.Add(item);
                        }
                    }
                }

                ret.Add(buildData);
            }

            return ret.ToArray();
        }
    }
}
