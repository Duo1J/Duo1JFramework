using Duo1JFramework.Asset;
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
        /// 播放类型
        /// </summary>
        public EAudioPlayType AudioPlayType => audioPlayType;

        /// <summary>
        /// 音频播放器
        /// </summary>
        protected AudioSource audioSource;

        /// <summary>
        /// 音频播放器
        /// </summary>
        public AudioSource AudioSource => audioSource;

        /// <summary>
        /// 音频数据
        /// </summary>
        protected AudioData audioData;

        /// <summary>
        /// 音频数据
        /// </summary>
        public AudioData AudioData => audioData;

        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool Playing => audioSource != null && audioSource.clip != null && audioSource.isPlaying;

        /// <summary>
        /// 单次播放中
        /// </summary>
        private bool oneShotPlaying = false;

        /// <summary>
        /// 是否已暂停
        /// </summary>
        private bool paused = false;

        /// <summary>
        /// 当前音频句柄
        /// </summary>
        private IAssetHandle<AudioClip> audioHandle;

        /// <summary>
        /// 播放版本
        /// </summary>
        private int playVersion = 0;

        /// <summary>
        /// 运行时音量
        /// </summary>
        private float runtimeVolume = 1f;

        /// <summary>
        /// 待淡入时间
        /// </summary>
        private float pendingFadeInTime = 0f;

        /// <summary>
        /// 淡入淡出时间
        /// </summary>
        private float fadeDuration = 0f;

        /// <summary>
        /// 淡入淡出计时
        /// </summary>
        private float fadeTimer = 0f;

        /// <summary>
        /// 淡入淡出起始音量
        /// </summary>
        private float fadeStartVolume = 1f;

        /// <summary>
        /// 淡入淡出目标音量
        /// </summary>
        private float fadeTargetVolume = 1f;

        /// <summary>
        /// 淡出结束后停止
        /// </summary>
        private bool stopWhenFadeFinished = false;

        /// <summary>
        /// 跟随目标
        /// </summary>
        private Transform followTarget;

        /// <summary>
        /// 持续播放
        /// </summary>
        public void PlayKeep(AudioData audioData, float fadeInTime = 0f)
        {
            audioPlayType = EAudioPlayType.Keep;
            pendingFadeInTime = Mathf.Max(0f, fadeInTime);
            SetAudioDataAndLoad(audioData, Play);
        }

        /// <summary>
        /// 单次播放
        /// </summary>
        public void PlayOneShot(AudioData audioData)
        {
            audioPlayType = EAudioPlayType.OneShot;
            pendingFadeInTime = 0f;
            SetAudioDataAndLoad(audioData, Play);
        }

        /// <summary>
        /// 在世界坐标播放单次音频
        /// </summary>
        public void PlayOneShotAt(AudioData audioData, Vector3 position)
        {
            transform.position = position;
            followTarget = null;
            PlayOneShot(audioData);
        }

        /// <summary>
        /// 在目标位置持续播放音频
        /// </summary>
        public void PlayKeepAt(AudioData audioData, Transform target, float fadeInTime = 0f)
        {
            followTarget = target;
            if (followTarget != null)
            {
                transform.position = followTarget.position;
            }
            PlayKeep(audioData, fadeInTime);
        }

        /// <summary>
        /// 设置播放数据并加载
        /// </summary>
        protected void SetAudioDataAndLoad(AudioData audioData, Action finCall = null)
        {
            if (audioData == null)
            {
                Log.ErrorForce($"{ToString()} AudioData为空，无法播放");
                Stop();
                return;
            }

            if (string.IsNullOrEmpty(audioData.AudioPath))
            {
                Log.ErrorForce($"{ToString()} AudioPath为空，无法播放");
                Stop();
                return;
            }

            playVersion++;
            int currentVersion = playVersion;

            ReleaseAudioHandle();
            this.audioData = audioData;
            oneShotPlaying = false;
            paused = false;
            stopWhenFadeFinished = false;
            fadeDuration = 0f;

            if (audioData.Sync)
            {
                IAssetHandle<AudioClip> handle = Asset.LoadByTypeSync<AudioClip>(audioData.AudioPath, audioData.LoadType);
                if (AudioClipLoadedPostprocess(handle, currentVersion))
                {
                    finCall?.Invoke();
                }
            }
            else
            {
                AssetManager.Instance.LoadByType<AudioClip>(audioData.AudioPath, (handle) =>
                {
                    if (AudioClipLoadedPostprocess(handle, currentVersion))
                    {
                        finCall?.Invoke();
                    }
                }, audioData.LoadType);
            }
        }

        /// <summary>
        /// 音频片段加载完成后处理
        /// </summary>
        protected bool AudioClipLoadedPostprocess(IAssetHandle<AudioClip> handle, int loadVersion)
        {
            if (loadVersion != playVersion)
            {
                handle?.Dispose();
                return false;
            }

            if (handle == null || handle.Error())
            {
                Log.ErrorForce($"{audioData} 加载失败");
                handle?.Dispose();
                Stop();
                return false;
            }

            audioHandle = handle;
            audioSource.clip = handle.Asset;
            ApplyAudioDataSetting();
            SetLoopByPlayType();
            return true;
        }

        /// <summary>
        /// 应用音频播放参数
        /// </summary>
        protected void ApplyAudioDataSetting()
        {
            if (audioData == null || audioSource == null)
            {
                return;
            }

            runtimeVolume = audioData.Volume;
            audioSource.pitch = audioData.Pitch;
            audioSource.panStereo = audioData.PanStereo;
            audioSource.spatialBlend = audioData.SpatialBlend;
            audioSource.priority = audioData.Priority;
            audioSource.minDistance = audioData.MinDistance;
            audioSource.maxDistance = audioData.MaxDistance;
            audioSource.rolloffMode = audioData.RolloffMode;

            if (AudioManager.TryGetInstance(out AudioManager audioManager))
            {
                audioSource.outputAudioMixerGroup = audioManager.GetMixerGroup(audioData.Category);
            }

            RefreshVolume();
        }

        /// <summary>
        /// 刷新音量
        /// </summary>
        public void RefreshVolume()
        {
            if (audioData == null || audioSource == null)
            {
                return;
            }

            float categoryVolume = 1f;
            if (AudioManager.TryGetInstance(out AudioManager audioManager))
            {
                categoryVolume = audioManager.GetFinalVolume(audioData.Category);
            }

            audioSource.volume = Mathf.Clamp01(runtimeVolume * categoryVolume);
        }

        /// <summary>
        /// 通过播放类型设置是否循环
        /// </summary>
        protected void SetLoopByPlayType()
        {
            if (audioData != null && audioData.Loop.HasValue)
            {
                audioSource.loop = audioData.Loop.Value;
                return;
            }

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
                Stop();
                return;
            }

            if (pendingFadeInTime > 0f)
            {
                runtimeVolume = 0f;
                RefreshVolume();
            }

            if (audioPlayType == EAudioPlayType.OneShot && audioData.UseNativeOneShot)
            {
                audioSource.PlayOneShot(audioSource.clip, 1f);
            }
            else
            {
                audioSource.Play();
            }
            paused = false;

            if (audioPlayType == EAudioPlayType.OneShot)
            {
                oneShotPlaying = true;
            }

            if (pendingFadeInTime > 0f)
            {
                FadeTo(audioData.Volume, pendingFadeInTime);
                pendingFadeInTime = 0f;
            }

            if (AudioManager.TryGetInstance(out AudioManager audioManager) && audioManager.ShouldPause(audioData.Category) && !audioData.IgnorePause)
            {
                Pause();
            }

            OnPlay();
        }

        protected virtual void OnPlay()
        {
            if (AudioManager.TryGetInstance(out AudioManager audioManager) && this is AudioMgrController controller)
            {
                audioManager.RegisterActiveController(controller);
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            if (audioSource == null || paused)
            {
                return;
            }

            audioSource.Pause();
            paused = true;

            OnPause();
        }

        protected virtual void OnPause()
        {
        }

        /// <summary>
        /// 继续播放
        /// </summary>
        public void Resume()
        {
            if (audioSource == null || !paused)
            {
                return;
            }

            audioSource.UnPause();
            paused = false;

            OnResume();
        }

        protected virtual void OnResume()
        {
        }

        /// <summary>
        /// 按分组暂停
        /// </summary>
        public void PauseByGroup()
        {
            if (audioData != null && audioData.IgnorePause)
            {
                return;
            }

            Pause();
        }

        /// <summary>
        /// 淡出后停止
        /// </summary>
        public void FadeOutAndStop(float fadeOutTime)
        {
            if (fadeOutTime <= 0f)
            {
                Stop();
                return;
            }

            FadeTo(0f, fadeOutTime, true);
        }

        /// <summary>
        /// 淡入淡出到目标音量
        /// </summary>
        public void FadeTo(float targetVolume, float duration, bool stopWhenFinished = false)
        {
            fadeStartVolume = runtimeVolume;
            fadeTargetVolume = Mathf.Clamp01(targetVolume);
            fadeDuration = Mathf.Max(0f, duration);
            fadeTimer = 0f;
            stopWhenFadeFinished = stopWhenFinished;

            if (fadeDuration <= 0f)
            {
                runtimeVolume = fadeTargetVolume;
                RefreshVolume();
                if (stopWhenFadeFinished)
                {
                    Stop();
                }
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            Stop(false);
        }

        /// <summary>
        /// 停止
        /// </summary>
        protected void Stop(bool invokeFinishCallback)
        {
            AudioData stoppedData = audioData;

            playVersion++;
            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.clip = null;
            }
            ReleaseAudioHandle();
            audioData = null;
            followTarget = null;

            oneShotPlaying = false;
            paused = false;
            fadeDuration = 0f;
            stopWhenFadeFinished = false;
            pendingFadeInTime = 0f;

            OnStop();

            if (invokeFinishCallback)
            {
                stoppedData?.FinishCallback?.Invoke(stoppedData);
            }
        }

        protected virtual void OnStop()
        {
            if (AudioManager.TryGetInstance(out AudioManager audioManager) && this is AudioMgrController controller)
            {
                audioManager.UnRegisterActiveController(controller);
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            playVersion++;
            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.clip = null;
                audioSource.loop = false;
                audioSource.volume = 1f;
                audioSource.pitch = 1f;
                audioSource.panStereo = 0f;
                audioSource.spatialBlend = 0f;
                audioSource.outputAudioMixerGroup = null;
            }
            ReleaseAudioHandle();
            audioData = null;
            followTarget = null;

            audioPlayType = EAudioPlayType.OneShot;
            oneShotPlaying = false;
            paused = false;
            runtimeVolume = 1f;
            pendingFadeInTime = 0f;
            fadeDuration = 0f;
            stopWhenFadeFinished = false;

            OnClear();
        }

        protected virtual void OnClear()
        {
            if (AudioManager.TryGetInstance(out AudioManager audioManager) && this is AudioMgrController controller)
            {
                audioManager.UnRegisterActiveController(controller);
            }
        }

        protected void OnUpdate()
        {
            if (audioSource == null)
            {
                return;
            }

            if (followTarget != null)
            {
                transform.position = followTarget.position;
            }

            UpdateFade();

            if (audioPlayType == EAudioPlayType.OneShot && oneShotPlaying && !audioSource.isPlaying && !paused)
            {
                Stop(true);
                return;
            }

            OnSubUpdate();
        }

        private void UpdateFade()
        {
            if (fadeDuration <= 0f)
            {
                return;
            }

            fadeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(fadeTimer / fadeDuration);
            runtimeVolume = Mathf.Lerp(fadeStartVolume, fadeTargetVolume, t);
            RefreshVolume();

            if (t >= 1f)
            {
                bool shouldStop = stopWhenFadeFinished;
                fadeDuration = 0f;
                stopWhenFadeFinished = false;

                if (shouldStop)
                {
                    Stop();
                }
            }
        }

        protected virtual void OnSubUpdate()
        {
        }

        private void ReleaseAudioHandle()
        {
            audioHandle?.Dispose();
            audioHandle = null;
        }

        private void Awake()
        {
            audioSource = this.GetOrAddComponent<AudioSource>();
            audioSource.playOnAwake = false;

            Reg.RegisterUpdate(OnUpdate);
            Clear();

            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }
    }
}
