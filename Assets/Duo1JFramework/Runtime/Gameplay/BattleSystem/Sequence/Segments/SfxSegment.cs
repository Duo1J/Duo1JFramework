using System;
using Duo1JFramework.Asset;
using Duo1JFramework.AudioAPI;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 音效片段
    /// </summary>
    [Serializable]
    public class SfxSegment : SequenceSegment
    {
        /// <summary>
        /// 音效资源路径
        /// </summary>
        public string SfxPath;

        /// <summary>
        /// 音量
        /// </summary>
        public float Volume = 1f;

        /// <summary>
        /// 是否跟随Owner
        /// </summary>
        public bool AttachToOwner = true;

        /// <summary>
        /// 资源加载方式
        /// </summary>
        public EAssetLoadType LoadType = EAssetLoadType.Resources;

        /// <summary>
        /// 音频分类
        /// </summary>
        public EAudioCategory Category = EAudioCategory.SFX;

        public override void OnEnter(SkillContext ctx)
        {
            if (ctx == null || ctx.Owner == null || string.IsNullOrEmpty(SfxPath))
            {
                return;
            }

            if (!AudioManager.TryGetInstance(out AudioManager audioMgr))
            {
                return;
            }

            AudioData audioData = new AudioData(SfxPath)
                .SetLoadType(LoadType)
                .SetSync(true)
                .SetCategory(Category)
                .SetVolume(Volume)
                .SetSpatialBlend(1f);

            if (AttachToOwner)
            {
                audioData.SetLoop(false);
                audioMgr.PlayKeepAt(audioData, ctx.Owner.transform);
            }
            else
            {
                audioMgr.PlayOneShotAt(audioData, ctx.Owner.transform.position);
            }
        }
    }
}
