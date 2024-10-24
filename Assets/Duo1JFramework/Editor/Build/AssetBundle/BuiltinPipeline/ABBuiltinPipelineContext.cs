using Duo1JFramework.Pattern.Pipeline;
using UnityEditor;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// AssetBundle内置管线上下文
    /// </summary>
    public class ABBuiltinPipelineContext : PipelineContext
    {
        public BuildTarget buildTarget;

        public ABBuiltinPipelineContext(BuildTarget buildTarget)
        {
            this.buildTarget = buildTarget;
        }
    }
}
