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
        /// 模型预制体路径
        /// </summary>
        public string Path { get; private set; }

        public ActorData(Type logicType, string name, string path)
        {
            LogicType = logicType;
            Name = name;
            Path = path;
        }
    }
}