using System.Collections.Generic;
using Duo1JFramework.Pattern.Pipeline;

namespace Duo1JFramework.Build
{
    public partial class ABBuiltinTask
    {
        /// <summary>
        /// 重命名所有的AssetBundle
        /// </summary>
        public class RenameAllAssetBundle : ITask
        {
            public EPipelineState Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.BUILD_DATAS, out ABBuildData[] buildDatas))
                    {
                        return EPipelineState.Fail;
                    }

                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.STRATEGY_DATA, out ABBuildStrategy strategy))
                    {
                        return EPipelineState.Fail;
                    }

                    bool success = false;

                    switch (strategy.ABNameType)
                    {
                        case EABNameType.Hash:
                            if (!context.TryGet(ABBuiltinPipeline.ContextKey.AB_TO_HASH_MAP, out Dictionary<string, string> ab2HashMap))
                            {
                                return EPipelineState.Fail;
                            }

                            success = AssetBundleBuilder.RenameAllAssetBundle(buildDatas, ab2HashMap);
                            break;
                        case EABNameType.MD5:
                            if (!context.TryGet(ABBuiltinPipeline.ContextKey.AB_TO_MD5_MAP, out Dictionary<string, string> ab2MD5Map))
                            {
                                return EPipelineState.Fail;
                            }

                            success = AssetBundleBuilder.RenameAllAssetBundle(buildDatas, ab2MD5Map);
                            break;
                        default:
                            success = AssetBundleBuilder.RenameAllAssetBundle(buildDatas, null);
                            break;
                    }

                    return success ? EPipelineState.Success : EPipelineState.Fail;
                });
            }
        }
    }
}
