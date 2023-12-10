namespace Duo1JFramework
{
    /// <summary>
    /// 单例基类
    /// </summary>
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static object locker = new object();

        private static T instance;

        public static T Instance
        {
            get
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new T();
                        instance.OnInit();
                    }
                }
                return instance;
            }
        }

        protected abstract void OnInit();
    }
}