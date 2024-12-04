using System;
using UnityEngine;

namespace Duo1JFramework.Graphics
{
    /// <summary>
    /// 光照烘焙数据集
    /// </summary>
    [Serializable]
    public class LightBakedData
    {
        [SerializeField]
        private LightmapBakedData[] lightmapBakedDataList;

        [SerializeField]
        private Texture reflectionProbe;

        public LightmapBakedData[] LightmapBakedDataList => lightmapBakedDataList;

        public Texture ReflectionProbe => reflectionProbe;

        /// <summary>
        /// 从渲染系统拷贝数据
        /// </summary>
        public void CopyFromRenderSystem()
        {
            LightmapData[] lightmaps = LightmapSettings.lightmaps;
            lightmapBakedDataList = new LightmapBakedData[lightmaps.Length];

            for (int i = 0; i < lightmaps.Length; i++)
            {
                LightmapData lightmapData = lightmaps[i];
                lightmapBakedDataList[i] = LightmapBakedData.Create(lightmapData);
            }

            reflectionProbe = RenderSettings.customReflectionTexture;
        }

        /// <summary>
        /// 设置数据到渲染系统
        /// </summary>
        public void SetToRenderSystem()
        {
            if (LightmapBakedDataList != null)
            {
                LightmapData[] lightmaps = new LightmapData[LightmapBakedDataList.Length];
                for (int i = 0; i < LightmapBakedDataList.Length; i++)
                {
                    lightmaps[i] = LightmapBakedDataList[i].ToLightmapData();
                }

                LightmapSettings.lightmaps = lightmaps;
            }
            else
            {
                LightmapSettings.lightmaps = Array.Empty<LightmapData>();
            }

            RenderSettings.customReflectionTexture = ReflectionProbe;
        }
    }
}
