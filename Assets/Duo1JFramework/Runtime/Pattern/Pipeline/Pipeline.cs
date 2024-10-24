using System.Collections.Generic;
using System.Linq;

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
        public virtual bool Run(IPipelineContext context)
        {
            Assert.NotNull(TaskList, "管线任务列表为空");

            return TaskList.All((task) =>
            {
                bool success = task.Run(context);
                Log.Info($"执行管线任务: `{task.GetType().FullName}` {(success ? "成功" : "失败")}");
                return success;
            });
        }

        /// <summary>
        /// 管线运行
        /// </summary>
        public static bool Run(IPipelineContext context, List<ITask> taskList)
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
