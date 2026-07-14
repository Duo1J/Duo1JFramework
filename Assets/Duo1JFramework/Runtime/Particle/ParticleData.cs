using Duo1JFramework.Asset;
using System;
using UnityEngine;

namespace Duo1JFramework.ParticleAPI
{
    /// <summary>
    /// 粒子播放数据
    /// </summary>
    [Serializable]
    public class ParticleData
    {
        /// <summary>
        /// 预制体路径
        /// </summary>
        public string ParticlePath { get; private set; }

        /// <summary>
        /// 加载方式
        /// </summary>
        public EAssetLoadType LoadType { get; private set; } = EAssetLoadType.Bundle;

        /// <summary>
        /// 同步加载
        /// </summary>
        public bool Sync { get; private set; } = false;

        /// <summary>
        /// 分类
        /// </summary>
        public EParticleCategory Category { get; private set; } = EParticleCategory.Common;

        /// <summary>
        /// 播放速度
        /// </summary>
        public float Speed { get; private set; } = 1f;

        /// <summary>
        /// 缩放
        /// </summary>
        public float Scale { get; private set; } = 1f;

        /// <summary>
        /// 自动回收时长(仅OneShot生效, 0表示根据粒子系统自动判定)
        /// </summary>
        public float AutoRecycleTime { get; private set; } = 0f;

        /// <summary>
        /// 同路径播放冷却
        /// </summary>
        public float Cooldown { get; private set; }

        /// <summary>
        /// 同路径最大并发数, 小于等于0表示不限制
        /// </summary>
        public int MaxSameParticleCount { get; private set; }

        /// <summary>
        /// 是否忽略暂停
        /// </summary>
        public bool IgnorePause { get; private set; }

        /// <summary>
        /// 播放完成回调
        /// </summary>
        public Action<ParticleData> FinishCallback { get; private set; }

        public ParticleData SetLoadType(EAssetLoadType loadType)
        {
            LoadType = loadType;
            return this;
        }

        public ParticleData SetSync(bool sync)
        {
            Sync = sync;
            return this;
        }

        public ParticleData SetCategory(EParticleCategory category)
        {
            Category = category;
            return this;
        }

        public ParticleData SetSpeed(float speed)
        {
            Speed = Mathf.Max(0f, speed);
            return this;
        }

        public ParticleData SetScale(float scale)
        {
            Scale = Mathf.Max(0f, scale);
            return this;
        }

        public ParticleData SetAutoRecycleTime(float time)
        {
            AutoRecycleTime = Mathf.Max(0f, time);
            return this;
        }

        public ParticleData SetCooldown(float cooldown)
        {
            Cooldown = Mathf.Max(0f, cooldown);
            return this;
        }

        public ParticleData SetMaxSameParticleCount(int count)
        {
            MaxSameParticleCount = Mathf.Max(0, count);
            return this;
        }

        public ParticleData SetIgnorePause(bool ignorePause)
        {
            IgnorePause = ignorePause;
            return this;
        }

        public ParticleData SetFinishCallback(Action<ParticleData> finishCallback)
        {
            FinishCallback = finishCallback;
            return this;
        }

        public ParticleData(string particlePath)
        {
            ParticlePath = particlePath;
        }

        public override string ToString()
        {
            return $"[category: {Category}, loadType: {LoadType}, sync: {Sync}, speed: {Speed}, scale: {Scale}, particlePath: {ParticlePath}]";
        }
    }
}
