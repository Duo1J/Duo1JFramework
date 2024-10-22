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
        public List<ITask> TastList { get; set; }

        /// <summary>
        /// 管线运行
        /// </summary>
        public virtual bool Run(IPipelineContext context)
        {
            Assert.NotNull(TastList, "管线任务列表为空");

            return TastList.All(task => task.Run(context));
        }

        /// <summary>
        /// 管线运行
        /// </summary>
        public static bool Run(IPipelineContext context, List<ITask> tastList)
        {
            return Create(tastList).Run(context);
        }

        /// <summary>
        /// 管线创建
        /// </summary>
        public static Pipeline Create(List<ITask> tastList)
        {
            Pipeline pipeline = new Pipeline();
            pipeline.TastList = tastList;

            return pipeline;
        }
    }
}
