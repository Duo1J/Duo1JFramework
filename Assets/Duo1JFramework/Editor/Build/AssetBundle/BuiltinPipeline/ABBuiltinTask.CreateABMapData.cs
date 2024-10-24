using System.Collections.Generic;
using Duo1JFramework.Pattern.Pipeline;

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
                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.AB_TO_ASSET_MAP, out Dictionary<string, List<string>> ab2AssetMap))
                    {
                        return false;
                    }

                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.AB_TO_HASH_MAP, out Dictionary<string, string> ab2HashMap))
                    {
                        return false;
                    }

                    context.TryGet(ABBuiltinPipeline.ContextKey.AB_TO_CRC_MAP, out Dictionary<string, uint> ab2CrcMap);

                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.AB_TO_MD5_MAP, out Dictionary<string, string> ab2MD5Map))
                    {
                        return false;
                    }

                    ABMapData abMapData = ABMapData.Create(ab2AssetMap, ab2HashMap, ab2CrcMap, ab2MD5Map);
                    abMapData.SaveToFile(Def.Asset.EncryptABMapData);

                    return true;
                });
            }
        }
    }
}
