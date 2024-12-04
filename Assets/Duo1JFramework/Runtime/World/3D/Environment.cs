using Duo1JFramework.Graphics;
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

            if (rendererLMParam != null)
            {
                Renderer[] renderers = GetComponentsInChildren<Renderer>();
                for (int i = 0, len = Mathf.Min(renderers.Length, rendererLMParam.Length); i < len; i++)
                {
                    rendererLMParam[i].SetToRenderer(renderers[i]);
                }
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
                rendererLMParam[i] = RendererLightmapParam.Create(renderers[i]);
            }

            Log.EditorInfo("光照数据填充成功");
        }
    }
}
