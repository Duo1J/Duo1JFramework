namespace Duo1JFramework
{
    /// <summary>
    /// 单例触发器
    /// </summary>
    public static class SingletonTrigger
    {
        /// <summary>
        /// 触发内部单例类
        /// </summary>
        public static void Trigger()
        {
            Log.Info("触发单例");

            GameManager.Instance.Trigger();
        }

        /// <summary>
        /// 停止所有单例类
        /// </summary>
        public static void Shutdown()
        {
            Log.Info("停止所有单例");
        }
    }
}
