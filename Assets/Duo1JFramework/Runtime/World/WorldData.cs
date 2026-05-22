using Duo1JFramework.Asset;
using UnityEngine.Assertions;

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

        /// <summary>
        /// 实例名称
        /// </summary>
        public string InstanceName { get; private set; }

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

        public WorldData SetInstanceName(string instanceName)
        {
            InstanceName = instanceName;
            return this;
        }

        public WorldData(string name, string path)
        {
            Assert.NotNullOrEmpty(name, "世界场景名不可为空");
            Assert.NotNullOrEmpty(path, "世界预制体路径不可为空");

            Name = name;
            Path = path;
            InstanceName = name;
        }
    }
}