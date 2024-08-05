using Duo1JFramework.Asset;
using System;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色创建配置数据
    /// </summary>
    public class ActorData
    {
        /// <summary>
        /// 逻辑类型
        /// </summary>
        public Type LogicType { get; private set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 预制体路径
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// 同步加载
        /// </summary>
        public bool Sync { get; set; } = false;

        /// <summary>
        /// 加载方式
        /// </summary>
        public EAssetLoadType LoadType { get; set; } = EAssetLoadType.Bundle;

        public ActorData SetSync(bool sync)
        {
            Sync = sync;
            return this;
        }

        public ActorData SetLoadType(EAssetLoadType loadType)
        {
            LoadType = loadType;
            return this;
        }

        public ActorData(Type logicType, string name, string path)
        {
            if (name == null)
            {
                name = "";
            }

            LogicType = logicType;
            Name = name;
            Path = path;
        }

        public override string ToString()
        {
            return $"<ActorData--{Name}-{LogicType}-{Path}>";
        }
    }
}