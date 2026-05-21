using System.Collections.Generic;
using Duo1JFramework.Pattern.Pipeline;

namespace Duo1JFramework.Bake
{
    /// <summary>
    /// 烘焙内置管线
    /// </summary>
    public class BakeBuiltinPipeline : Pipeline
    {
        public static EPipelineState Build(BakeBuiltinPipelineContext context)
        {
            return Run(context, Tasks);
        }

        /// <summary>
        /// 管线任务列表
        /// </summary>
        public static readonly List<ITask> Tasks = new List<ITask>()
        {
            new BakeBuiltinTask.CheckStrategy(),
            new BakeBuiltinTask.ClearBakeData(),
            new BakeBuiltinTask.BakeScene(),
            new BakeBuiltinTask.SaveAndRefresh(),
        };

        /// <summary>
        /// 管线环境上下文缓存Key
        /// </summary>
        public class ContextKey
        {
            /// <summary>
            /// BakeStrategy 缓存
            /// </summary>
            public const string STRATEGY_DATA = "STRATEGY_DATA";

            /// <summary>
            /// BakeSceneData[] 缓存
            /// </summary>
            public const string SCENE_DATAS = "SCENE_DATAS";
        }
    }
}
