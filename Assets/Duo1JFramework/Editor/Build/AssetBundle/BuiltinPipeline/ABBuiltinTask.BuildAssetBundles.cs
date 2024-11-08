using System.Collections.Generic;
using Duo1JFramework.Pattern.Pipeline;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.Build
{
    public partial class ABBuiltinTask
    {
        /// <summary>
        /// 构建AssetBundle
        /// </summary>
        public class BuildAssetBundles : ITask
        {
            public EPipelineState Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    ABBuiltinPipelineContext ctx = Assert.Convert<ABBuiltinPipelineContext>(context);
                    if (ctx == null)
                    {
                        return EPipelineState.Fail;
                    }

                    if (ctx.buildTarget == BuildTarget.NoTarget)
                    {
                        Log.EditorError($"构建目标异常: `{ctx.buildTarget.GetName()}`");
                        return EPipelineState.Fail;
                    }

                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.BUILD_INPUT_LIST, out List<AssetBundleBuild> buildInputList))
                    {
                        return EPipelineState.Fail;
                    }

                    if (buildInputList.Count == 0)
                    {
                        Log.EditorError($"AssetBundle构建输入列表为空, 无需执行构建");
                        return EPipelineState.Break;
                    }

                    AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(
                        PathUtil.GetAssetBundleEditorRoot().CheckDir(),
                        buildInputList.ToArray(),
                        ABBuildStrategy.Instance.BuildOptions,
                        ctx.buildTarget
                    );

                    if (manifest == null)
                    {
                        Log.EditorError($"构建 `{ctx.buildTarget.GetName()}` 平台的AssetBundle失败");
                        return EPipelineState.Fail;
                    }

                    return EPipelineState.Success;
                });
            }
        }
    }
}
