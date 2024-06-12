using Duo1JFramework.Asset;
using Duo1JFramework.Event;
using System;
using UnityEngine;

namespace Duo1JFramework.AudioAPI
{
    /// <summary>
    /// 基础音频控制器
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public abstract class BaseAudioController : MonoRegister
    {
        /// <summary>
        /// 播放类型
        /// </summary>
        [SerializeField]
        protected EAudioPlayType audioPlayType = EAudioPlayType.OneShot;

        /// <summary>
        /// 音频播放器
        /// </summary>
        protected AudioSource audioSource;

        /// <summary>
        /// 音频数据
        /// </summary>
        protected AudioData audioData;

        /// <summary>
        /// 单次播放中
        /// </summary>
        private bool oneShotPlaying = false;

        /// <summary>
        /// 持续播放
        /// </summary>
        public void PlayKeep(AudioData audioData)
        {
            audioPlayType = EAudioPlayType.Keep;
            SetAudioDataAndLoad(audioData, Play);
        }

        /// <summary>
        /// 单次播放
        /// </summary>
        public void PlayOneShot(AudioData audioData)
        {
            audioPlayType = EAudioPlayType.OneShot;
            SetAudioDataAndLoad(audioData, Play);
        }

        /// <summary>
        /// 设置播放数据并加载
        /// </summary>
        protected void SetAudioDataAndLoad(AudioData audioData, Action finCall = null)
        {
            this.audioData = audioData;

            if (audioData.sync)
            {
                AudioClip clip = AssetManager.Instance.LoadByTypeSync<AudioClip>(audioData.loadType, audioData.audioPath);
                AudioClipLoadedPostprocess(clip);
                finCall?.Invoke();
            }
            else
            {
                AssetManager.Instance.LoadByType<AudioClip>(audioData.loadType, audioData.audioPath, (clip) =>
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
                Log.ErrorForce($"{audioData} 加载失败");
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
                case EAudioPlayType.OneShot:
                    audioSource.loop = false;
                    break;
                case EAudioPlayType.Keep:
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

            if (audioPlayType == EAudioPlayType.OneShot)
            {
                oneShotPlaying = true;
            }

            OnPlay();
        }

        protected virtual void OnPlay()
        {
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            audioSource.Pause();

            OnPause();
        }

        protected virtual void OnPause()
        {
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

            OnStop();
        }

        protected virtual void OnStop()
        {
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

            audioPlayType = EAudioPlayType.OneShot;
            oneShotPlaying = false;

            OnClear();
        }

        protected virtual void OnClear()
        {
        }

        protected void OnUpdate()
        {
            if (audioSource == null)
            {
                return;
            }

            if (audioPlayType != EAudioPlayType.OneShot)
            {
                return;
            }

            if (oneShotPlaying && !audioSource.isPlaying)
            {
                Stop();
            }

            OnSubUpdate();
        }

        protected virtual void OnSubUpdate()
        {
        }

        private void Awake()
        {
            audioSource = this.GetOrAddComponent<AudioSource>();
            audioSource.playOnAwake = false;

            Register.RegisterUpdate(OnUpdate);
            Clear();

            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }
    }
}
