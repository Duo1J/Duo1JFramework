using System.Collections.Generic;

namespace Duo1JFramework.Pattern.Pipeline
{
    /// <summary>
    /// 管线
    /// </summary>
    public class Pipeline : IPipeline
    {
        /// <summary>
        /// 管线任务列表
        /// </summary>
        public List<ITask> TaskList { get; set; }

        /// <summary>
        /// 管线运行
        /// </summary>
        public virtual EPipelineState Run(IPipelineContext context)
        {
            Assert.NotNull(TaskList, "管线任务列表为空");

            foreach (ITask task in TaskList)
            {
                EPipelineState state = task.Run(context);
                switch (state)
                {
                    case EPipelineState.Success:
                        Log.Info($"执行管线任务: `{task.GetType().FullName}` 成功");
                        break;
                    case EPipelineState.Fail:
                        Log.Info($"执行管线任务: `{task.GetType().FullName}` 失败");
                        return EPipelineState.Fail;
                    case EPipelineState.Break:
                        Log.Info($"执行管线任务: `{task.GetType().FullName}` 中断");
                        return EPipelineState.Break;
                    default:
                        Log.ErrorForce($"未处理的管线状态: `{state.GetName()}`, Task: `{task.GetType().FullName}`");
                        return EPipelineState.Fail;
                }
            }

            return EPipelineState.Success;
        }

        /// <summary>
        /// 管线运行
        /// </summary>
        public static EPipelineState Run(IPipelineContext context, List<ITask> taskList)
        {
            return Create(taskList).Run(context);
        }

        /// <summary>
        /// 管线创建
        /// </summary>
        public static Pipeline Create(List<ITask> taskList)
        {
            Pipeline pipeline = new Pipeline();
            pipeline.TaskList = taskList;

            return pipeline;
        }
    }
}
