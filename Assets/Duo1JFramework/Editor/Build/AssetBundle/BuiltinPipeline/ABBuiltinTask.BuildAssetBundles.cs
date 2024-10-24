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
            public bool Run(IPipelineContext context)
            {
                return Util.TryCatch(() =>
                {
                    ABBuiltinPipelineContext ctx = Assert.Convert<ABBuiltinPipelineContext>(context);
                    if (ctx == null)
                    {
                        return false;
                    }

                    if (ctx.buildTarget == BuildTarget.NoTarget)
                    {
                        Log.EditorError($"构建目标异常: `{ctx.buildTarget.GetName()}`");
                        return false;
                    }

                    if (!context.TryGet(ABBuiltinPipeline.ContextKey.BUILD_INPUT_LIST, out List<AssetBundleBuild> buildInputList))
                    {
                        return false;
                    }

                    EditorUtility.DisplayProgressBar("构建AssetBundle", "正在构建AssetBundle...", 0.3f);

                    AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(
                        PathUtil.GetAssetBundleEditorRoot().CheckDir(),
                        buildInputList.ToArray(),
                        ABBuildStrategy.Instance.BuildOptions,
                        ctx.buildTarget
                    );

                    if (manifest == null)
                    {
                        Log.EditorInfo($"构建 `{ctx.buildTarget.GetName()}` 平台的AssetBundle失败");
                        return false;
                    }

                    return true;
                });
            }
        }
    }
}
