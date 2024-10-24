using System.Collections.Generic;
using Duo1JFramework.Pattern.Pipeline;
using UnityEditor;

namespace Duo1JFramework.Build
{
    public partial class ABBuiltinTask
    {
        /// <summary>
        /// 将构建数据转化为AssetBundle构建输入数据列表
        /// </summary>
        public class BuildDataToBuildInputList : ITask
        {
            public bool Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.BUILD_DATAS, out ABBuildData[] buildDatas))
                    {
                        return false;
                    }

                    List<AssetBundleBuild> buildInputList = new List<AssetBundleBuild>();
                    Dictionary<string, List<string>> ab2AssetMap = new Dictionary<string, List<string>>();

                    foreach (ABBuildData buildData in buildDatas)
                    {
                        if (ab2AssetMap.ContainsKey(buildData.ABName))
                        {
                            Log.EditorError($"AssetBundle包名重复: {buildData.ABName}");
                            continue;
                        }

                        ab2AssetMap.Add(buildData.ABName, buildData.AssetPathList);

                        if (buildData.IsEmpty())
                        {
                            continue;
                        }

                        buildInputList.Add(buildData.ToAssetBundleBuild());
                    }

                    context.Set<Dictionary<string, List<string>>>(ABBuiltinPipeline.ContextKey.AB_TO_ASSET_MAP, ab2AssetMap);
                    context.Set<List<AssetBundleBuild>>(ABBuiltinPipeline.ContextKey.BUILD_INPUT_LIST, buildInputList);

                    return true;
                });
            }
        }
    }
}
