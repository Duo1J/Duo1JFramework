using System;
using UnityEngine;

namespace Duo1JFramework.Graphics
{
    /// <summary>
    /// Lightmap 烘焙数据
    /// </summary>
    [Serializable]
    public class LightmapBakedData
    {
        [SerializeField]
        private Texture2D lightmap;

        [SerializeField]
        private Texture2D lightmapDir;

        [SerializeField]
        private Texture2D shadowMask;

        public Texture2D Lightmap => lightmap;

        public Texture2D LightmapDir => lightmapDir;

        public Texture2D ShadowMask => shadowMask;

        /// <summary>
        /// 从LightmapData拷贝数据
        /// </summary>
        public LightmapBakedData CopyFromLightmapData(LightmapData lightmapData)
        {
            lightmap = lightmapData.lightmapColor;
            lightmapDir = lightmapData.lightmapDir;
            shadowMask = lightmapData.shadowMask;

            return this;
        }

        /// <summary>
        /// 转化为LightmapData
        /// </summary>
        public LightmapData ToLightmapData()
        {
            LightmapData lightmapData = new LightmapData();
            lightmapData.lightmapColor = Lightmap;
            lightmapData.lightmapDir = LightmapDir;
            lightmapData.shadowMask = ShadowMask;

            return lightmapData;
        }

        /// <summary>
        /// 从LightmapData创建
        /// </summary>
        public static LightmapBakedData Create(LightmapData lightmapData)
        {
            return new LightmapBakedData().CopyFromLightmapData(lightmapData);
        }
    }
}
