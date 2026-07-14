using System;
using Duo1JFramework.ParticleAPI;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 特效片段
    /// </summary>
    [Serializable]
    public class VfxSegment : SequenceSegment
    {
        /// <summary>
        /// 特效资源路径
        /// </summary>
        public string VfxPath;

        /// <summary>
        /// 是否跟随Owner
        /// </summary>
        public bool AttachToOwner = true;

        /// <summary>
        /// 缩放
        /// </summary>
        public float Scale = 1f;

        [NonSerialized]
        private ParticleMgrController controller;

        public override void OnEnter(SkillContext ctx)
        {
            if (ctx == null || ctx.Owner == null || string.IsNullOrEmpty(VfxPath))
            {
                return;
            }

            if (!ParticleManager.TryGetInstance(out ParticleManager particleManager))
            {
                return;
            }

            ParticleData data = new ParticleData(VfxPath)
                .SetSync(true)
                .SetCategory(EParticleCategory.Combat)
                .SetScale(Scale);

            Transform ownerTf = ctx.Owner.transform;
            if (AttachToOwner)
            {
                controller = particleManager.PlayKeepAt(data, ownerTf);
            }
            else
            {
                controller = particleManager.PlayOneShotAt(data, ownerTf.position, ownerTf.rotation);
            }
        }

        public override void OnExit(SkillContext ctx)
        {
            if (controller != null)
            {
                if (AttachToOwner)
                {
                    controller.Stop();
                }
                controller = null;
            }
        }
    }
}
