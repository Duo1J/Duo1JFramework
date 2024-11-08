using System.Collections.Generic;
using Duo1JFramework.Pattern.Pipeline;

namespace Duo1JFramework.Build
{
    public partial class ABBuiltinTask
    {
        /// <summary>
        /// 构建MD5映射
        /// </summary>
        public class BuildAB2MD5Map : ITask
        {
            public EPipelineState Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.BUILD_DATAS, out ABBuildData[] buildDatas))
                    {
                        return EPipelineState.Fail;
                    }

                    Dictionary<string, string> ab2MD5Map = AssetBundleBuilder.BuildAB2MD5Map(buildDatas);

                    context.Set<Dictionary<string, string>>(ABBuiltinPipeline.ContextKey.AB_TO_MD5_MAP, ab2MD5Map);

                    return EPipelineState.Success;
                });
            }
        }
    }
}
