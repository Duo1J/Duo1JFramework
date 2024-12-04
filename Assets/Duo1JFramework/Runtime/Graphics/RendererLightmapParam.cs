using System;
using UnityEngine;

namespace Duo1JFramework.Graphics
{
    /// <summary>
    /// Renderer Lightmap 参数
    /// </summary>
    [Serializable]
    public class RendererLightmapParam
    {
        [SerializeField]
        private int lightmapIndex;

        [SerializeField]
        private Vector4 lightmapScaleOffset;

        public int LightmapIndex => lightmapIndex;

        public Vector4 LightmapScaleOffset => lightmapScaleOffset;

        /// <summary>
        /// 从Renderer拷贝数据
        /// </summary>
        public RendererLightmapParam CopyFromRenderer(Renderer renderer)
        {
            lightmapIndex = renderer.lightmapIndex;
            lightmapScaleOffset = renderer.lightmapScaleOffset;

            return this;
        }

        /// <summary>
        /// 设置数据到Renderer
        /// </summary>
        public void SetToRenderer(Renderer renderer)
        {
            renderer.lightmapIndex = LightmapIndex;
            renderer.lightmapScaleOffset = LightmapScaleOffset;
        }

        /// <summary>
        /// 从Renderer创建
        /// </summary>
        public static RendererLightmapParam Create(Renderer renderer)
        {
            return new RendererLightmapParam().CopyFromRenderer(renderer);
        }
    }
}
