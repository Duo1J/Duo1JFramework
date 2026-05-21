using System.Collections.Generic;
using Duo1JFramework.Pattern.Pipeline;
using UnityEditor;

namespace Duo1JFramework.Bake
{
    public partial class BakeBuiltinTask
    {
        /// <summary>
        /// 检查烘焙策略
        /// </summary>
        public class CheckStrategy : ITask
        {
            public EPipelineState Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    BakeStrategy strategy = BakeStrategy.Instance;
                    BakeSceneData[] sceneDatas = GetBakeSceneDatas(strategy);

                    if (sceneDatas == null || sceneDatas.Length == 0)
                    {
                        Log.EditorError($"烘焙场景列表为空: `{BakeStrategy.AssetPath}`");
                        strategy.SelectAsset();
                        return EPipelineState.Fail;
                    }

                    foreach (BakeSceneData sceneData in sceneDatas)
                    {
                        if (sceneData == null || !sceneData.CheckValid())
                        {
                            Log.EditorError($"烘焙场景配置无效: `{sceneData?.scenePath}`");
                            strategy.SelectAsset();
                            return EPipelineState.Fail;
                        }
                    }

                    context.Set(BakeBuiltinPipeline.ContextKey.STRATEGY_DATA, strategy);
                    context.Set(BakeBuiltinPipeline.ContextKey.SCENE_DATAS, sceneDatas);

                    return EPipelineState.Success;
                });
            }

            /// <summary>
            /// 获取需烘焙场景数据
            /// </summary>
            private static BakeSceneData[] GetBakeSceneDatas(BakeStrategy strategy)
            {
                if (strategy.SceneDatas != null && strategy.SceneDatas.Length > 0)
                {
                    return strategy.SceneDatas;
                }

                List<BakeSceneData> ret = new List<BakeSceneData>();
                foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
                {
                    if (scene.enabled)
                    {
                        ret.Add(new BakeSceneData(scene.path));
                    }
                }

                return ret.ToArray();
            }
        }
    }
}
