using Duo1JFramework.Pattern.Pipeline;
using UnityEngine.SceneManagement;

namespace Duo1JFramework.Bake
{
    /// <summary>
    /// 烘焙内置管线上下文
    /// </summary>
    public class BakeBuiltinPipelineContext : PipelineContext
    {
        public string activeScenePath;

        public BakeBuiltinPipelineContext()
        {
            activeScenePath = SceneManager.GetActiveScene().path;
        }
    }
}
