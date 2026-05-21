using System;

namespace Duo1JFramework.Bake
{
    /// <summary>
    /// 烘焙场景数据
    /// </summary>
    [Serializable]
    public class BakeSceneData
    {
        [Label("场景路径")]
        public string scenePath;

        [Label("启用雾效")]
        public bool enableFog;

        public BakeSceneData()
        {
        }

        public BakeSceneData(string scenePath)
        {
            this.scenePath = scenePath;
        }

        /// <summary>
        /// 检查是否有效
        /// </summary>
        public bool CheckValid()
        {
            return !string.IsNullOrEmpty(scenePath) && scenePath.EndsWith(".unity");
        }
    }
}
