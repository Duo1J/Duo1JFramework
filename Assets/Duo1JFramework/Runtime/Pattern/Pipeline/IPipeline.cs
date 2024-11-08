using System.Collections.Generic;

namespace Duo1JFramework.Pattern.Pipeline
{
    /// <summary>
    /// 管线接口
    /// </summary>
    public interface IPipeline
    {
        /// <summary>
        /// 管线任务列表
        /// </summary>
        List<ITask> TaskList { get; set; }

        /// <summary>
        /// 管线运行
        /// </summary>
        EPipelineState Run(IPipelineContext context);
    }
}
