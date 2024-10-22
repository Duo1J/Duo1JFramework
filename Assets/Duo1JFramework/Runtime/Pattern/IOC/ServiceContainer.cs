using Duo1JFramework.DataStructure;

namespace Duo1JFramework.Pattern.IOC
{
    /// <summary>
    /// IOC服务容器
    /// </summary>
    public class ServiceContainer
    {
        /// <summary>
        /// 服务类型集合
        /// </summary>
        private TypeSet typeSet = new TypeSet();

        /// <summary>
        /// 获取服务
        /// </summary>
        public T Get<T>() where T : class
        {
            return typeSet.Get<T>();
        }

        /// <summary>
        /// 尝试获取值
        /// </summary>
        public bool TryGet<T>(out T value) where T : class
        {
            return typeSet.TryGetValue<T>(out value);
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        public void Register<T>(T obj) where T : class
        {
            typeSet.Add<T>(obj);
        }

        /// <summary>
        /// 取消注册服务
        /// </summary>
        public bool UnRegister<T>() where T : class
        {
            return typeSet.Remove<T>();
        }

        /// <summary>
        /// 取消注册所有服务
        /// </summary>
        public void UnRegisterAll()
        {
            typeSet.Clear();
        }
    }
}
