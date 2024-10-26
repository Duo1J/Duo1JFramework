using System.Collections.Generic;
using Duo1JFramework.Pattern.Pipeline;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// AssetBundle内置管线
    /// </summary>
    public class ABBuiltinPipeline : Pipeline
    {
        public static bool Build(ABBuiltinPipelineContext context)
        {
            return Run(context, Tasks);
        }

        /// <summary>
        /// 管线任务列表
        /// </summary>
        public static readonly List<ITask> Tasks = new List<ITask>()
        {
            new ABBuiltinTask.ClearAllAssetBundleBuild(),
            new ABBuiltinTask.CheckStrategy(),
            new ABBuiltinTask.StrategyToBuildData(),
            new ABBuiltinTask.BuildDataToBuildInputList(),
            new ABBuiltinTask.BuildAssetBundles(),
            new ABBuiltinTask.BuildAB2AssetMap(),
            new ABBuiltinTask.BuildAB2HashMap(),
            new ABBuiltinTask.BuildAB2CRCMap(),
            new ABBuiltinTask.BuildAB2MD5Map(),
            new ABBuiltinTask.RenameAllAssetBundle(),
            new ABBuiltinTask.CreateABMapData(),
        };

        /// <summary>
        /// 管线环境上下文缓存Key
        /// </summary>
        public class ContextKey
        {
            /// <summary>
            /// ABBuildStrategy 缓存
            /// </summary>
            public const string STRATEGY_DATA = "STRATEGY_DATA";

            /// <summary>
            /// ABBuildData[] 缓存
            /// </summary>
            public const string BUILD_DATAS = "BUILD_DATAS";

            /// <summary>
            /// List<AssetBundleBuild> 缓存
            /// </summary>
            public const string BUILD_INPUT_LIST = "BUILD_INPUT_LIST";

            /// <summary>
            /// Dictionary<string, List<ABMapAssetData>> ab2AssetMap缓存
            /// </summary>
            public const string AB_TO_ASSET_MAP = "AB_TO_ASSET_MAP";

            /// <summary>
            /// Dictionary<string, string> ab2HashMap 缓存
            /// </summary>
            public const string AB_TO_HASH_MAP = "AB_2_HASH_MAP";

            /// <summary>
            /// Dictionary<string, uint> ab2CrcMap 缓存
            /// </summary>
            public const string AB_TO_CRC_MAP = "AB_TO_CRC_MAP";

            /// <summary>
            /// Dictionary<string, string> ab2MD5Map 缓存
            /// </summary>
            public const string AB_TO_MD5_MAP = "AB_TO_MD5_MAP";
        }
    }
}
