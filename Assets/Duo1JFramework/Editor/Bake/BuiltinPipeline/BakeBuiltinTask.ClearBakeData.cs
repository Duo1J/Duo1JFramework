using Duo1JFramework.Pattern.Pipeline;
using UnityEditor;

namespace Duo1JFramework.Bake
{
    public partial class BakeBuiltinTask
    {
        /// <summary>
        /// 清理烘焙数据
        /// </summary>
        public class ClearBakeData : ITask
        {
            public EPipelineState Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    if (!context.TryGet(BakeBuiltinPipeline.ContextKey.STRATEGY_DATA, out BakeStrategy strategy))
                    {
                        return EPipelineState.Fail;
                    }

                    if (!strategy.ClearBakeData)
                    {
                        Log.EditorInfo("无需清理烘焙数据");
                        return EPipelineState.Success;
                    }

                    Lightmapping.Clear();
                    Log.EditorInfo("清理烘焙数据完成");

                    return EPipelineState.Success;
                });
            }
        }
    }
}
