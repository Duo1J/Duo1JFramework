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
            public bool Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    ABBuildStrategyData[] strategyDatas = ABBuildStrategy.Instance.Data;

                    if (strategyDatas == null || strategyDatas.Length == 0)
                    {
                        Log.EditorError($"AB构建策略配置为空: `{ABBuildStrategy.AssetPath}`");
                        ABBuildStrategy.Instance.SelectAsset();

                        return false;
                    }

                    context.Set<ABBuildStrategyData[]>(ABBuiltinPipeline.ContextKey.STRATEGY_DATAS, strategyDatas);

                    return true;
                });
            }
        }
    }
}
