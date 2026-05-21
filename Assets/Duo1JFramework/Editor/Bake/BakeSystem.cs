using Duo1JFramework.Pattern.Pipeline;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Duo1JFramework.Bake
{
    /// <summary>
    /// 烘焙系统
    /// </summary>
    public class BakeSystem
    {
        /// <summary>
        /// 执行全量烘焙
        /// </summary>
        public static bool BakeAll()
        {
            BakeBuiltinPipelineContext context = new BakeBuiltinPipelineContext();
            try
            {
                return BakeBuiltinPipeline.Build(context) == EPipelineState.Success;
            }
            finally
            {
                RestoreActiveScene(context);
                EditorUtility.ClearProgressBar();
            }
        }

        /// <summary>
        /// 恢复激活场景
        /// </summary>
        private static void RestoreActiveScene(BakeBuiltinPipelineContext context)
        {
            if (context == null || string.IsNullOrEmpty(context.activeScenePath))
            {
                return;
            }

            EditorSceneManager.OpenScene(context.activeScenePath, OpenSceneMode.Single);
        }

        /// <summary>
        /// 清理烘焙数据
        /// </summary>
        public static void ClearBakeData()
        {
            Lightmapping.Clear();
            Log.EditorInfo("清理烘焙数据完成");
        }

        private BakeSystem()
        {
        }
    }
}
