namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色配置数据
    /// </summary>
    public class ActorCfgData
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 模型路径
        /// </summary>
        public string Path { get; private set; }

        public ActorCfgData(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}