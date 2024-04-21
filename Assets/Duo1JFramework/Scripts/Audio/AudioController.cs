using Duo1JFramework.Asset;
using Duo1JFramework.Event;
using System;
using UnityEngine;

namespace Duo1JFramework.AudioAPI
{
    /// <summary>
    /// 音频控制器
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioController : MonoRegister
    {
        /// <summary>
        /// 播放类型
        /// </summary>
        private eAudioPlayType audioPlayType = eAudioPlayType.None;

        /// <summary>
        /// 音频播放器
        /// </summary>
        private AudioSource audioSource;

        /// <summary>
        /// 音频数据
        /// </summary>
        private AudioData audioData;

        /// <summary>
        /// 单次播放中
        /// </summary>
        private bool oneShotPlaying = false;

        /// <summary>
        /// 是否是背景音乐
        /// </summary>
        public bool IsBackgroundMusic { get; set; }

        /// <summary>
        /// 持续播放
        /// </summary>
        public void PlayKeep(AudioData audioData)
        {
            audioPlayType = eAudioPlayType.Keep;
            SetAudioDataAndLoad(audioData, Play);
        }

        /// <summary>
        /// 单次播放
        /// </summary>
        public void PlayOneShot(AudioData audioData)
        {
            audioPlayType = eAudioPlayType.OneShot;
            SetAudioDataAndLoad(audioData, Play);
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        public void PlayBackgroundMusic(AudioData audioData)
        {
            if (!IsBackgroundMusic)
            {
                Log.ErrorForce($"{ToString()} 非背景音乐控制器");
                return;
            }

            audioPlayType = eAudioPlayType.Keep;
            SetAudioDataAndLoad(audioData, Play);
        }

        /// <summary>
        /// 停止背景音乐
        /// </summary>
        public void StopBackgroundMusic()
        {
            if (!IsBackgroundMusic)
            {
                Log.ErrorForce($"{ToString()} 非背景音乐控制器");
                return;
            }

            Stop();
        }

        /// <summary>
        /// 设置播放数据并加载
        /// </summary>
        protected void SetAudioDataAndLoad(AudioData audioData, Action finCall = null)
        {
            this.audioData = audioData;

            if (audioData.Sync)
            {
                AudioClip clip = AssetManager.Instance.LoadByTypeSync<AudioClip>(audioData.LoadType, audioData.AudioPath);
                AudioClipLoadedPostprocess(clip);
                finCall?.Invoke();
            }
            else
            {
                AssetManager.Instance.LoadByType<AudioClip>(audioData.LoadType, audioData.AudioPath, (clip) =>
                {
                    AudioClipLoadedPostprocess(clip);
                    finCall?.Invoke();
                });
            }
        }

        /// <summary>
        /// 音频片段加载完成后处理
        /// </summary>
        protected void AudioClipLoadedPostprocess(AudioClip clip)
        {
            if (clip == null)
            {
                return;
            }

            audioSource.clip = clip;
            SetLoopByPlayType();
        }

        /// <summary>
        /// 通过播放类型设置是否循环
        /// </summary>
        protected void SetLoopByPlayType()
        {
            switch (audioPlayType)
            {
                case eAudioPlayType.OneShot:
                    audioSource.loop = false;
                    break;
                case eAudioPlayType.Keep:
                    audioSource.loop = true;
                    break;
                default:
                    Log.ErrorForce($"AudioClipLoadedPostprocess 未处理的音频播放类型: `{audioPlayType}`");
                    break;
            }
        }

        /// <summary>
        /// 播放
        /// </summary>
        public void Play()
        {
            if (audioSource.clip == null)
            {
                Log.ErrorForce($"{ToString()} AudioClip为空，无法播放");
                return;
            }

            audioSource.Play();

            if (audioPlayType == eAudioPlayType.OneShot)
            {
                oneShotPlaying = true;
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            audioSource.Pause();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            audioSource.Stop();
            audioSource.clip = null;
            audioData = null;

            oneShotPlaying = false;

            if (!IsBackgroundMusic)
            {
                AudioManager.Instance.PushCon(this);
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.clip = null;
            }
            audioData = null;

            audioPlayType = eAudioPlayType.None;
            oneShotPlaying = false;
            IsBackgroundMusic = false;
        }

        protected void OnUpdate()
        {
            if (audioSource == null)
            {
                return;
            }

            if (audioPlayType != eAudioPlayType.OneShot)
            {
                return;
            }
            if (IsBackgroundMusic)
            {
                return;
            }

            if (oneShotPlaying && !audioSource.isPlaying)
            {
                Stop();
            }
        }

        protected void Awake()
        {
            audioSource = this.GetOrAddComponent<AudioSource>();
            audioSource.playOnAwake = false;

            Register.RegisterEvent(eEvent.AUDIO_STOP_ALL_ONE_SHOT, (param) =>
            {
                if (IsBackgroundMusic)
                {
                    return;
                }
                if (audioPlayType != eAudioPlayType.OneShot)
                {
                    return;
                }

                Stop();
            });

            Register.RegisterEvent(eEvent.AUDIO_STOP_ALL_KEEP, (param) =>
            {
                if (IsBackgroundMusic)
                {
                    return;
                }
                if (audioPlayType != eAudioPlayType.Keep)
                {
                    return;
                }

                Stop();
            });

            Register.RegisterUpdate(OnUpdate);

            Clear();
        }
    }
}