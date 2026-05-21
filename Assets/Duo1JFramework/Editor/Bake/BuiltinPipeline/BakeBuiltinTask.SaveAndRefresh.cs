using Duo1JFramework.Pattern.Pipeline;

namespace Duo1JFramework.Bake
{
    public partial class BakeBuiltinTask
    {
        /// <summary>
        /// 保存并刷新
        /// </summary>
        public class SaveAndRefresh : ITask
        {
            public EPipelineState Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    EditorUtil.SaveAndRefresh("BakeBuiltinPipeline::SaveAndRefresh");
                    Log.EditorInfo("内置烘焙管线完成");

                    return EPipelineState.Success;
                });
            }
        }
    }
}
