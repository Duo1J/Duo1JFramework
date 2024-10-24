namespace Duo1JFramework.Pattern.Pipeline
{
    /// <summary>
    /// 管线环境上下文接口
    /// </summary>
    public interface IPipelineContext
    {
        /// <summary>
        /// 通过类型设置参数
        /// </summary>
        void Set<T>(T obj);

        /// <summary>
        /// 通过Key设置参数
        /// </summary>
        void Set<T>(string key, T obj);

        /// <summary>
        /// 尝试通过类型获取参数
        /// </summary>
        bool TryGet<T>(out T value);

        /// <summary>
        /// 尝试通过Key获取参数
        /// </summary>
        bool TryGet<T>(string key, out T value);
    }
}
