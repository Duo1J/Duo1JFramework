using Duo1JFramework.Asset;
using System;
using UnityEngine;

namespace Duo1JFramework.AudioAPI
{
    /// <summary>
    /// 音频播放数据
    /// </summary>
    [Serializable]
    public class AudioData
    {
        /// <summary>
        /// 音频路径
        /// </summary>
        public string AudioPath { get; private set; }

        /// <summary>
        /// 加载方式
        /// </summary>
        public EAssetLoadType LoadType { get; private set; } = EAssetLoadType.Bundle;

        /// <summary>
        /// 同步加载
        /// </summary>
        public bool Sync { get; private set; } = false;

        /// <summary>
        /// 音频分类
        /// </summary>
        public EAudioCategory Category { get; private set; } = EAudioCategory.Common;

        /// <summary>
        /// 音量
        /// </summary>
        public float Volume { get; private set; } = 1f;

        /// <summary>
        /// 音调
        /// </summary>
        public float Pitch { get; private set; } = 1f;

        /// <summary>
        /// 立体声声像
        /// </summary>
        public float PanStereo { get; private set; } = 0f;

        /// <summary>
        /// 空间混合
        /// </summary>
        public float SpatialBlend { get; private set; } = 0f;

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; private set; } = 128;

        /// <summary>
        /// 是否循环，未设置时根据播放类型决定
        /// </summary>
        public bool? Loop { get; private set; }

        /// <summary>
        /// 是否忽略暂停
        /// </summary>
        public bool IgnorePause { get; private set; }

        /// <summary>
        /// 是否使用AudioSource原生单次播放
        /// </summary>
        public bool UseNativeOneShot { get; private set; }

        /// <summary>
        /// 同一路径播放冷却时间
        /// </summary>
        public float Cooldown { get; private set; }

        /// <summary>
        /// 同一路径最大播放数量，小于等于0表示不限制
        /// </summary>
        public int MaxSameAudioCount { get; private set; }

        /// <summary>
        /// 最小距离
        /// </summary>
        public float MinDistance { get; private set; } = 1f;

        /// <summary>
        /// 最大距离
        /// </summary>
        public float MaxDistance { get; private set; } = 500f;

        /// <summary>
        /// 音量衰减模式
        /// </summary>
        public AudioRolloffMode RolloffMode { get; private set; } = AudioRolloffMode.Logarithmic;

        /// <summary>
        /// 播放完成回调
        /// </summary>
        public Action<AudioData> FinishCallback { get; private set; }

        public AudioData SetLoadType(EAssetLoadType loadType)
        {
            LoadType = loadType;
            return this;
        }

        public AudioData SetSync(bool sync)
        {
            Sync = sync;
            return this;
        }

        public AudioData SetCategory(EAudioCategory category)
        {
            Category = category == EAudioCategory.Master ? EAudioCategory.Common : category;
            return this;
        }

        public AudioData SetVolume(float volume)
        {
            Volume = Mathf.Clamp01(volume);
            return this;
        }

        public AudioData SetPitch(float pitch)
        {
            Pitch = Mathf.Max(0.01f, pitch);
            return this;
        }

        public AudioData SetPanStereo(float panStereo)
        {
            PanStereo = Mathf.Clamp(panStereo, -1f, 1f);
            return this;
        }

        public AudioData SetSpatialBlend(float spatialBlend)
        {
            SpatialBlend = Mathf.Clamp01(spatialBlend);
            return this;
        }

        public AudioData SetPriority(int priority)
        {
            Priority = Mathf.Clamp(priority, 0, 256);
            return this;
        }

        public AudioData SetLoop(bool loop)
        {
            Loop = loop;
            return this;
        }

        public AudioData SetIgnorePause(bool ignorePause)
        {
            IgnorePause = ignorePause;
            return this;
        }

        public AudioData SetUseNativeOneShot(bool useNativeOneShot)
        {
            UseNativeOneShot = useNativeOneShot;
            return this;
        }

        public AudioData SetCooldown(float cooldown)
        {
            Cooldown = Mathf.Max(0f, cooldown);
            return this;
        }

        public AudioData SetMaxSameAudioCount(int maxSameAudioCount)
        {
            MaxSameAudioCount = Mathf.Max(0, maxSameAudioCount);
            return this;
        }

        public AudioData SetDistance(float minDistance, float maxDistance)
        {
            MinDistance = Mathf.Max(0f, minDistance);
            MaxDistance = Mathf.Max(MinDistance, maxDistance);
            return this;
        }

        public AudioData SetRolloffMode(AudioRolloffMode rolloffMode)
        {
            RolloffMode = rolloffMode;
            return this;
        }

        public AudioData SetFinishCallback(Action<AudioData> finishCallback)
        {
            FinishCallback = finishCallback;
            return this;
        }

        public AudioData(string audioPath)
        {
            AudioPath = audioPath;
        }

        public override string ToString()
        {
            return $"[category: {Category}, loadType: {LoadType}, sync: {Sync}, volume: {Volume}, pitch: {Pitch}, audioPath: {AudioPath}]";
        }
    }
}
