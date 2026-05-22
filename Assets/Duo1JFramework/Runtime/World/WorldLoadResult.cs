namespace Duo1JFramework.World
{
    /// <summary>
    /// 世界加载结果
    /// </summary>
    public class WorldLoadResult
    {
        /// <summary>
        /// 是否加载成功
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// 世界场景数据
        /// </summary>
        public WorldData WorldData { get; private set; }

        /// <summary>
        /// 世界场景控制器
        /// </summary>
        public BaseWorldController Controller { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static WorldLoadResult CreateSuccess(WorldData worldData, BaseWorldController controller)
        {
            return new WorldLoadResult()
            {
                Success = true,
                WorldData = worldData,
                Controller = controller,
            };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static WorldLoadResult CreateFail(WorldData worldData, string error)
        {
            return new WorldLoadResult()
            {
                Success = false,
                WorldData = worldData,
                Error = error,
            };
        }
    }
}
