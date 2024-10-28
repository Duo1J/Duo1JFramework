namespace Duo1JFramework
{
    /// <summary>
    /// 单例基类
    /// </summary>
    public abstract class Singleton<T> : BaseObject, ISingleton where T : Singleton<T>, new()
    {
        private static object locker = new object();

        /// <summary>
        /// 单例
        /// </summary>
        private static T instance;

        /// <summary>
        /// 是否已销毁
        /// </summary>
        private bool dispose = false;

        /// <summary>
        /// 获得单例
        /// </summary>
        public static T Instance
        {
            get
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        Instance = new T();
                        instance.OnInit();
                    }
                }

                return instance;
            }
            private set
            {
                T oldInstance = instance;
                instance = value;

                if (instance != null)
                {
                    SingletonManager.AddSingleton(instance as ISingleton);
                }

                if (oldInstance != null)
                {
                    SingletonManager.RemoveSingleton(oldInstance as ISingleton);
                }
            }
        }

        public bool IsDisposed
        {
            get => dispose;
            protected set => dispose = value;
        }

        /// <summary>
        /// 是否是单例
        /// </summary>
        public override bool IsSingleton => true;

        /// <summary>
        /// 子类初始化
        /// </summary>
        protected abstract void OnInit();

        /// <summary>
        /// 子类销毁
        /// </summary>
        protected abstract void OnDispose();

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            if (dispose)
            {
                return;
            }

            dispose = true;

            OnDispose();

            Instance = null;
        }
    }
}
