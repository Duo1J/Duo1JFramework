using Duo1JFramework.Asset;

namespace Duo1JFramework.World
{
    /// <summary>
    /// 世界场景数据
    /// </summary>
    public class WorldData
    {
        /// <summary>
        /// 世界场景名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 预制体路径
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// 是否同步加载
        /// </summary>
        public bool Sync { get; private set; } = false;

        /// <summary>
        /// 加载方式
        /// </summary>
        public EAssetLoadType LoadType { get; private set; } = EAssetLoadType.Bundle;

        public WorldData SetLoadType(EAssetLoadType loadType)
        {
            LoadType = loadType;
            return this;
        }

        public WorldData SetSync(bool sync)
        {
            Sync = sync;
            return this;
        }

        public WorldData(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}