using UnityEngine;

namespace Duo1JFramework.ParticleAPI
{
    /// <summary>
    /// 粒子分组
    /// </summary>
    public class ParticleGroup
    {
        /// <summary>
        /// 分类
        /// </summary>
        public EParticleCategory Category { get; private set; }

        /// <summary>
        /// 时间缩放
        /// </summary>
        public float TimeScale { get; private set; } = 1f;

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool Hidden { get; private set; }

        /// <summary>
        /// 是否暂停
        /// </summary>
        public bool Paused { get; private set; }

        public void SetTimeScale(float timeScale)
        {
            TimeScale = Mathf.Max(0f, timeScale);
        }

        public void SetHidden(bool hidden)
        {
            Hidden = hidden;
        }

        public void SetPause(bool pause)
        {
            Paused = pause;
        }

        public ParticleGroup(EParticleCategory category)
        {
            Category = category;
        }
    }
}
