namespace Duo1JFramework
{
    /// <summary>
    /// 单例接口
    /// </summary>
    public interface ISingleton : IDispose
    {
        /// <summary>
        /// 是否已销毁
        /// </summary>
        bool IsDisposed { get; }
    }
}
