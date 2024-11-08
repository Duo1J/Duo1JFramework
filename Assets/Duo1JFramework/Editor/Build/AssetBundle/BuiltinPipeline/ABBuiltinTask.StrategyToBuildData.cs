using Duo1JFramework.Pattern.Pipeline;

namespace Duo1JFramework.Build
{
    public partial class ABBuiltinTask
    {
        /// <summary>
        /// 将策略数据转化为构建数据
        /// </summary>
        public class StrategyToBuildData : ITask
        {
            public EPipelineState Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.STRATEGY_DATA, out ABBuildStrategy strategy))
                    {
                        return EPipelineState.Fail;
                    }

                    ABBuildData[] buildDatas = AssetBundleBuilder.StrategyToBuildData(strategy.Data);

                    if (buildDatas == null || buildDatas.Length == 0)
                    {
                        Log.EditorError($"AB构建数据为空，请检查策略配置: `{ABBuildStrategy.AssetPath}`");
                        ABBuildStrategy.Instance.SelectAsset();
                        return EPipelineState.Fail;
                    }

                    context.Set<ABBuildData[]>(ABBuiltinPipeline.ContextKey.BUILD_DATAS, buildDatas);

                    return EPipelineState.Success;
                });
            }
        }
    }
}
