namespace Duo1JFramework.PlatformAPI
{
    /// <summary>
    /// 平台
    /// </summary>
    public class Platform
    {
        /// <summary>
        /// 当前平台接口
        /// </summary>
        public static IPlatform Current
        {
            get
            {
                if (current == null)
                {
                    Init();
                }

                return current;
            }
        }
        private static IPlatform current;

        public static EPlatform Type => Current.Type;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            if (current != null)
            {
                return;
            }

#if UNITY_EDITOR
            current = new PCPlatform();
#elif UNITY_STANDALONE_WIN
            current = new PCPlatform();
#else
            current = new DefaultPlatform();
#endif

            Log.Info($"初始化平台`{current.GetType().Name}`");
        }

        private Platform()
        {
        }
    }
}
