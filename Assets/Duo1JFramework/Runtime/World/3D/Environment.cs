using Duo1JFramework.Graphics;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.World
{
    /// <summary>
    /// 世界场景环境
    /// </summary>
    public class Environment : BaseMono
    {
        [SerializeField]
        private LightBakedData lightBakedData;

        [SerializeField]
        private RendererLightmapParam[] rendererLMParam;

        [SerializeField]
        private bool fog;

        [SerializeField]
        private Color fogColor = Color.gray;

        [SerializeField]
        private float fogDensity = 0.01f;

        [SerializeField]
        private Material skybox;

        [SerializeField]
        private float timeOfDay;

        public float TimeOfDay => timeOfDay;

        private void Awake()
        {
            SetToRenderSystem();
        }

        /// <summary>
        /// 将数据设置到渲染系统
        /// </summary>
        public void SetToRenderSystem()
        {
            if (lightBakedData != null)
            {
                lightBakedData.SetToRenderSystem();
            }

            ApplyRendererLightmapParams();
            ApplyEnvironmentSetting();
        }

        /// <summary>
        /// 设置时间
        /// </summary>
        public void SetTimeOfDay(float timeOfDay)
        {
            this.timeOfDay = Mathf.Repeat(timeOfDay, 24f);
        }

        /// <summary>
        /// 应用环境设置
        /// </summary>
        public void ApplyEnvironmentSetting()
        {
            RenderSettings.fog = fog;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;

            if (skybox != null)
            {
                RenderSettings.skybox = skybox;
            }
        }

        /// <summary>
        /// 通过设置填充数据
        /// </summary>
        public void FillDataBySetting()
        {
            Assert.GuardEditor();

            if (Game.IsPrefabStage)
            {
                Log.EditorError("预制体Stage不可填充光照数据");
                return;
            }

            lightBakedData = new LightBakedData();
            lightBakedData.CopyFromRenderSystem();

            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            rendererLMParam = new RendererLightmapParam[renderers.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                rendererLMParam[i] = RendererLightmapParam.Create(renderers[i]).SetRendererPath(GetRendererPath(renderers[i]));
            }

            Log.EditorInfo("光照数据填充成功");
        }

        private void ApplyRendererLightmapParams()
        {
            if (rendererLMParam == null)
            {
                return;
            }

            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            Dictionary<string, Renderer> rendererMap = new Dictionary<string, Renderer>();
            foreach (Renderer renderer in renderers)
            {
                rendererMap[GetRendererPath(renderer)] = renderer;
            }

            for (int i = 0; i < rendererLMParam.Length; i++)
            {
                RendererLightmapParam param = rendererLMParam[i];
                if (param == null)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(param.RendererPath) && rendererMap.TryGetValue(param.RendererPath, out Renderer renderer))
                {
                    param.SetToRenderer(renderer);
                    continue;
                }

                if (i < renderers.Length)
                {
                    param.SetToRenderer(renderers[i]);
                }
            }
        }

        private string GetRendererPath(Renderer renderer)
        {
            Transform root = transform;
            Transform cur = renderer.transform;
            string path = cur.name;

            while (cur.parent != null && cur.parent != root)
            {
                cur = cur.parent;
                path = cur.name + "/" + path;
            }

            return path;
        }
    }
}
