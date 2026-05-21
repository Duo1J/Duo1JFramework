using Duo1JFramework.Pattern.Pipeline;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Duo1JFramework.Bake
{
    public partial class BakeBuiltinTask
    {
        /// <summary>
        /// 构建场景烘焙
        /// </summary>
        public class BakeScene : ITask
        {
            public EPipelineState Run(IPipelineContext context)
            {
                try
                {
                    return Util.TryCatch(() =>
                    {
                        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        {
                            Log.EditorError("烘焙构建取消，存在未保存场景");
                            return EPipelineState.Break;
                        }

                        if (!context.TryGet(BakeBuiltinPipeline.ContextKey.SCENE_DATAS, out BakeSceneData[] sceneDatas))
                        {
                            return EPipelineState.Fail;
                        }

                        for (int i = 0; i < sceneDatas.Length; i++)
                        {
                            if (!BakeSingleScene(sceneDatas[i], i, sceneDatas.Length))
                            {
                                return EPipelineState.Fail;
                            }
                        }

                        return EPipelineState.Success;
                    });
                }
                finally
                {
                    EditorUtility.ClearProgressBar();
                }
            }

            /// <summary>
            /// 烘焙单个场景
            /// </summary>
            private static bool BakeSingleScene(BakeSceneData sceneData, int index, int total)
            {
                EditorUtility.DisplayProgressBar("烘焙", $"打开场景: {sceneData.scenePath}", (float)index / total);
                Scene scene = EditorSceneManager.OpenScene(sceneData.scenePath, OpenSceneMode.Single);
                if (!scene.IsValid())
                {
                    Log.EditorError($"烘焙打开场景失败: `{sceneData.scenePath}`");
                    return false;
                }

                ApplySceneBakeSetting(sceneData);

                PrepareReflectionProbes(scene);

                EditorUtility.DisplayProgressBar("烘焙", $"烘焙场景: {sceneData.scenePath}", (float)index / total);
                if (!Lightmapping.Bake())
                {
                    Log.EditorError($"烘焙场景失败: `{sceneData.scenePath}`");
                    return false;
                }

                EditorSceneManager.SaveScene(scene);
                Log.EditorInfo($"烘焙场景完成: `{sceneData.scenePath}`");

                return true;
            }

            /// <summary>
            /// 应用场景烘焙配置
            /// </summary>
            private static void ApplySceneBakeSetting(BakeSceneData sceneData)
            {
                Lightmapping.giWorkflowMode = GIWorkflowMode.OnDemand;
                RenderSettings.fog = sceneData.enableFog;
            }

            /// <summary>
            /// 准备反射探针
            /// </summary>
            private static void PrepareReflectionProbes(Scene scene)
            {
                foreach (GameObject rootGo in scene.GetRootGameObjects())
                {
                    ReflectionProbe[] probes = rootGo.GetComponentsInChildren<ReflectionProbe>(true);
                    foreach (ReflectionProbe probe in probes)
                    {
                        probe.mode = ReflectionProbeMode.Baked;
                        EditorUtility.SetDirty(probe);
                    }
                }
            }
        }
    }
}
