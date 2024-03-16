using Duo1JFramework.Asset;
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
        /// 构建所有AssetBundle
        /// </summary>
        public static void BuildAllAssetBundle()
        {
            ABBuildStrategyData[] strategyDatas = ABBuildStrategy.Instance.Data;

            if (strategyDatas == null || strategyDatas.Length == 0)
            {
                Log.EditorError($"AB构建策略配置为空: `{ABBuildStrategy.AssetPath}`");
                ABBuildStrategy.SelectAsset();
                return;
            }

            ABBuildData[] buildDatas = StrategyToBuildData(strategyDatas);

            if (buildDatas == null || buildDatas.Length == 0)
            {
                Log.EditorError($"AB构建数据为空，请检查策略配置: `{ABBuildStrategy.AssetPath}`");
                ABBuildStrategy.SelectAsset();
                return;
            }

            List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();

            foreach (ABBuildData buildData in buildDatas)
            {
                buildList.Add(buildData.ToAssetBundleBuild());
            }

            //try
            //{
            //    EditorUtility.DisplayProgressBar("构建AssetBndle", "正在构建AssetBundle...", 0.3f);

            //    BuildPipeline.BuildAssetBundles(
            //        Path.GetAssetBundleRoot(),
            //        buildList.ToArray(),
            //        EditorUtil.GetABBuildOptions(),
            //        EditorUtil.GetCurBuildTarget()
            //    );
            //}
            //catch (Exception e)
            //{
            //    EditorUtility.ClearProgressBar();
            //    Assert.ExceptHandle(e, "AssetBundle构建异常");
            //}
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

            List<string> allPathList = GetAllResPathList();

            //todo hlj

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
                    string path_ = Path.CorrectPath(path);
                    List<string> resultList = allPathList.FindAll((p) => p.StartsWith(path_));

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

        /// <summary>
        /// 获取所有Res下资源路径列表
        /// </summary>
        public static List<string> GetAllResPathList()
        {
            string prefix = Path.ASSET_PATH_PREFIX;
            string[] allGUID = AssetDatabase.FindAssets("t:Object", new[] { prefix });
            List<string> allPathList = new List<string>();
            foreach (string guid in allGUID)
            {
                allPathList.Add(AssetDatabase.GUIDToAssetPath(guid));
            }

            List<string> ret = new List<string>();
            foreach (string path in allPathList)
            {
                ret.Add(path.Replace(prefix, ""));
            }

            return ret;
        }
    }
}
