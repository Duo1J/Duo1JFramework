namespace Duo1JFramework.Pattern.IOC
{
    /// <summary>
    /// IOC服务
    /// </summary>
    public class Service
    {
        /// <summary>
        /// 静态服务容器
        /// </summary>
        private static readonly ServiceContainer serviceContainer = new ServiceContainer();

        /// <summary>
        /// 获取服务
        /// </summary>
        public static T Get<T>() where T : class
        {
            return serviceContainer.Get<T>();
        }

        /// <summary>
        /// 尝试获取服务
        /// </summary>
        public bool TryGet<T>(out T value) where T : class
        {
            return serviceContainer.TryGet<T>(out value);
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        public static void Register<T>(T obj) where T : class
        {
            serviceContainer.Register<T>(obj);
        }

        /// <summary>
        /// 取消注册服务
        /// </summary>
        public static bool UnRegister<T>() where T : class
        {
            return serviceContainer.UnRegister<T>();
        }

        /// <summary>
        /// 取消注册所有服务
        /// </summary>
        public static void UnRegisterAll()
        {
            serviceContainer.UnRegisterAll();
        }
    }
}
