using System.Collections.Generic;
using Duo1JFramework.Pattern.Pipeline;

namespace Duo1JFramework.Build
{
    public partial class ABBuiltinTask
    {
        /// <summary>
        /// 构建Hash映射
        /// </summary>
        public class BuildAB2HashMap : ITask
        {
            public bool Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.BUILD_DATAS, out ABBuildData[] buildDatas))
                    {
                        return false;
                    }

                    Dictionary<string, string> ab2HashMap = AssetBundleBuilder.BuildAB2HashMap(buildDatas);

                    context.Set<Dictionary<string, string>>(ABBuiltinPipeline.ContextKey.AB_TO_HASH_MAP, ab2HashMap);

                    return true;
                });
            }
        }
    }
}
