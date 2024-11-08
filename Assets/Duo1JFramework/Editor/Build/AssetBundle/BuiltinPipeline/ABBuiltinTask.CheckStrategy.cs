using Duo1JFramework.Pattern.Pipeline;

namespace Duo1JFramework.Build
{
    public partial class ABBuiltinTask
    {
        /// <summary>
        /// 检查构建策略
        /// </summary>
        public class CheckStrategy : ITask
        {
            public EPipelineState Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    ABBuildStrategy strategy = ABBuildStrategy.Instance;
                    ABBuildStrategyData[] strategyDatas = strategy.Data;

                    if (strategyDatas == null || strategyDatas.Length == 0)
                    {
                        Log.EditorError($"AB构建策略配置为空: `{ABBuildStrategy.AssetPath}`");
                        ABBuildStrategy.Instance.SelectAsset();

                        return EPipelineState.Fail;
                    }

                    context.Set<ABBuildStrategy>(ABBuiltinPipeline.ContextKey.STRATEGY_DATA, strategy);

                    return EPipelineState.Success;
                });
            }
        }
    }
}
