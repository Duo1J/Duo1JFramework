using Duo1JFramework.Pattern.Pipeline;
using System.Collections.Generic;
using System.Linq;

namespace Duo1JFramework.Build
{
    public partial class ABBuiltinTask
    {
        /// <summary>
        /// 构建AssetBundle与资产映射
        /// </summary>
        public class BuildAB2AssetMap : ITask
        {
            public bool Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.BUILD_DATAS, out ABBuildData[] buildDatas))
                    {
                        return false;
                    }

                    Dictionary<string, List<ABMapAssetData>> ab2AssetMap = new Dictionary<string, List<ABMapAssetData>>();
                    foreach (ABBuildData buildData in buildDatas)
                    {
                        if (buildData.IsEmpty())
                        {
                            continue;
                        }

                        List<ABMapAssetData> assetDataList = null;
                        if (ab2AssetMap.ContainsKey(buildData.ABName))
                        {
                            Log.EditorError($"AssetBundle包名重复: {buildData.ABName}");
                            assetDataList = ab2AssetMap[buildData.ABName];
                        }
                        else
                        {
                            assetDataList = new List<ABMapAssetData>();
                            ab2AssetMap.Add(buildData.ABName, assetDataList);
                        }

                        foreach (string assetPath in buildData.AssetPathList)
                        {
                            ABMapAssetData assetData = new ABMapAssetData(assetPath, buildData.ABName);
                            assetDataList.Add(assetData);
                        }
                    }

                    Dictionary<string, List<ABMapAssetData>> ab2AssetMapDistinct = new Dictionary<string, List<ABMapAssetData>>();
                    foreach (KeyValuePair<string, List<ABMapAssetData>> kv in ab2AssetMap)
                    {
                        ab2AssetMapDistinct.Add(kv.Key, kv.Value.Distinct().ToList());
                    }

                    context.Set<Dictionary<string, List<ABMapAssetData>>>(ABBuiltinPipeline.ContextKey.AB_TO_ASSET_MAP, ab2AssetMapDistinct);

                    return true;
                });
            }
        }
    }
}

