using System.Collections.Generic;
using Duo1JFramework.Pattern.Pipeline;
using Newtonsoft.Json;

namespace Duo1JFramework.Build
{
    public partial class ABBuiltinTask
    {
        /// <summary>
        /// 创建AssetBundle映射数据
        /// </summary>
        public class CreateABMapData : ITask
        {
            public bool Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.AB_TO_ASSET_MAP, out Dictionary<string, List<ABMapAssetData>> ab2AssetMap))
                    {
                        return false;
                    }

                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.AB_TO_HASH_MAP, out Dictionary<string, string> ab2HashMap))
                    {
                        return false;
                    }

                    context.TryGet(ABBuiltinPipeline.ContextKey.AB_TO_CRC_MAP, out Dictionary<string, uint> ab2CrcMap, true);

                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.AB_TO_MD5_MAP, out Dictionary<string, string> ab2MD5Map))
                    {
                        return false;
                    }

                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.STRATEGY_DATA, out ABBuildStrategy strategy))
                    {
                        return false;
                    }

                    ABMapData abMapData = ABMapData.Create(ab2AssetMap);
                    strategy.SetToABMapData(abMapData);
                    abMapData
                        .SetAB2HashMap(ab2HashMap)
                        .SetAB2CRCMap(ab2CrcMap)
                        .SetAB2MD5Map(ab2MD5Map);

                    abMapData.SaveToFile(Def.Asset.EncryptABMapData, null, Formatting.None);

                    return true;
                });
            }
        }
    }
}
