namespace Duo1JFramework.Pattern.Pipeline
{
    /// <summary>
    /// 管线任务接口
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// 管线任务运行
        /// </summary>
        /// <returns>任务是否执行成功</returns>
        bool Run(IPipelineContext context);
    }
}
