using System.Collections.Generic;
using Duo1JFramework.Pattern.Pipeline;

namespace Duo1JFramework.Build
{
    public partial class ABBuiltinTask
    {
        /// <summary>
        /// 构建CRC映射
        /// </summary>
        public class BuildAB2CRCMap : ITask
        {
            public bool Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.BUILD_DATAS, out ABBuildData[] buildDatas))
                    {
                        return false;
                    }

                    if (Def.Asset.BuildABCRC)
                    {
                        Dictionary<string, uint> ab2CrcMap = AssetBundleBuilder.BuildAB2CRCMap(buildDatas);
                        context.Set<Dictionary<string, uint>>(ABBuiltinPipeline.ContextKey.AB_TO_CRC_MAP, ab2CrcMap);
                    }

                    return true;
                });
            }
        }
    }
}
