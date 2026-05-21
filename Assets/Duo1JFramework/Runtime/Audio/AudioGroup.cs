using UnityEngine;
using UnityEngine.Audio;

namespace Duo1JFramework.AudioAPI
{
    /// <summary>
    /// 音频分组
    /// </summary>
    public class AudioGroup
    {
        /// <summary>
        /// 音频分类
        /// </summary>
        public EAudioCategory Category { get; private set; }

        /// <summary>
        /// 音量
        /// </summary>
        public float Volume { get; private set; } = 1f;

        /// <summary>
        /// 是否静音
        /// </summary>
        public bool Muted { get; private set; }

        /// <summary>
        /// 是否暂停
        /// </summary>
        public bool Paused { get; private set; }

        /// <summary>
        /// 混音器分组
        /// </summary>
        public AudioMixerGroup MixerGroup { get; private set; }

        /// <summary>
        /// 设置音量
        /// </summary>
        public void SetVolume(float volume)
        {
            Volume = Mathf.Clamp01(volume);
        }

        /// <summary>
        /// 设置静音
        /// </summary>
        public void SetMute(bool mute)
        {
            Muted = mute;
        }

        /// <summary>
        /// 设置暂停
        /// </summary>
        public void SetPause(bool pause)
        {
            Paused = pause;
        }

        /// <summary>
        /// 设置混音器分组
        /// </summary>
        public void SetMixerGroup(AudioMixerGroup mixerGroup)
        {
            MixerGroup = mixerGroup;
        }

        public AudioGroup(EAudioCategory category)
        {
            Category = category;
        }
    }
}
