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
            public bool Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.STRATEGY_DATAS, out ABBuildStrategyData[] strategyDatas))
                    {
                        return false;
                    }

                    ABBuildData[] buildDatas = AssetBundleBuilder.StrategyToBuildData(strategyDatas);

                    if (buildDatas == null || buildDatas.Length == 0)
                    {
                        Log.EditorError($"AB构建数据为空，请检查策略配置: `{ABBuildStrategy.AssetPath}`");
                        ABBuildStrategy.Instance.SelectAsset();
                        return false;
                    }

                    context.Set<ABBuildData[]>(ABBuiltinPipeline.ContextKey.BUILD_DATAS, buildDatas);

                    return true;
                });
            }
        }
    }
}
