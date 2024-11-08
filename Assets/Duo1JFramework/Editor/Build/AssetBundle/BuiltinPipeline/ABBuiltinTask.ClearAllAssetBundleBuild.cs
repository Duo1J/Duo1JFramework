using Duo1JFramework.Pattern.Pipeline;

namespace Duo1JFramework.Build
{
    public partial class ABBuiltinTask
    {
        /// <summary>
        /// 清理所有的AssetBundle构建结果
        /// </summary>
        public class ClearAllAssetBundleBuild : ITask
        {
            public EPipelineState Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    AssetBundleBuilder.ClearAllAssetBundleBuild();

                    return EPipelineState.Success;
                });
            }
        }
    }
}
