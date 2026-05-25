using Duo1JFramework.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Duo1JFramework.AudioAPI
{
    /// <summary>
    /// 音频管理器
    /// </summary>
    public class AudioManager : MonoSingleton<AudioManager>
    {
        /// <summary>
        /// AudioController对象池
        /// </summary>
        private GameObjectPool pool;

        /// <summary>
        /// 活跃控制器列表
        /// </summary>
        private readonly List<AudioMgrController> activeControllerList = new List<AudioMgrController>();

        /// <summary>
        /// 活跃控制器快照缓存
        /// </summary>
        private readonly List<AudioMgrController> activeControllerSnapshotCache = new List<AudioMgrController>();

        /// <summary>
        /// 音频分组字典
        /// </summary>
        private readonly Dictionary<EAudioCategory, AudioGroup> audioGroupDict = new Dictionary<EAudioCategory, AudioGroup>();

        /// <summary>
        /// 音频路径最后播放时间
        /// </summary>
        private readonly Dictionary<string, float> audioLastPlayTimeDict = new Dictionary<string, float>();

        /// <summary>
        /// 背景音乐控制器
        /// </summary>
        private AudioMgrController BgmController
        {
            get
            {
                if (bgmController == null)
                {
                    bgmController = PopCon().GetComponent<AudioMgrController>();
                    bgmController.IsBackgroundMusic = true;
                }

                return bgmController;
            }
        }
        private AudioMgrController bgmController;

        /// <summary>
        /// 持续播放
        /// </summary>
        public AudioMgrController PlayKeep(AudioData audioData, float fadeInTime = 0f)
        {
            if (!CanPlay(audioData))
            {
                return null;
            }

            AudioMgrController controller = PopCon();
            controller.PlayKeep(audioData, fadeInTime);
            MarkPlayed(audioData);

            return controller;
        }

        /// <summary>
        /// 在目标位置持续播放
        /// </summary>
        public AudioMgrController PlayKeepAt(AudioData audioData, Transform target, float fadeInTime = 0f)
        {
            if (!CanPlay(audioData))
            {
                return null;
            }

            AudioMgrController controller = PopCon();
            controller.PlayKeepAt(audioData, target, fadeInTime);
            MarkPlayed(audioData);

            return controller;
        }

        /// <summary>
        /// 停止所有持续音频播放
        /// </summary>
        public void StopAllKeep(float fadeOutTime = 0f)
        {
            StopByPlayType(EAudioPlayType.Keep, fadeOutTime, false);
        }

        /// <summary>
        /// 单次播放
        /// </summary>
        public AudioMgrController PlayOneShot(AudioData audioData)
        {
            if (!CanPlay(audioData))
            {
                return null;
            }

            AudioMgrController controller = PopCon();
            controller.PlayOneShot(audioData);
            MarkPlayed(audioData);

            return controller;
        }

        /// <summary>
        /// 在世界坐标单次播放
        /// </summary>
        public AudioMgrController PlayOneShotAt(AudioData audioData, Vector3 position)
        {
            if (!CanPlay(audioData))
            {
                return null;
            }

            AudioMgrController controller = PopCon();
            controller.PlayOneShotAt(audioData, position);
            MarkPlayed(audioData);

            return controller;
        }

        /// <summary>
        /// 停止所有单次音频播放
        /// </summary>
        public void StopAllOneShot(float fadeOutTime = 0f)
        {
            StopByPlayType(EAudioPlayType.OneShot, fadeOutTime, false);
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        public void PlayBackgroundMusic(AudioData audioData, float fadeInTime = 0f)
        {
            if (audioData != null)
            {
                audioData.SetCategory(EAudioCategory.BGM);
            }

            if (!CanPlay(audioData))
            {
                return;
            }

            BgmController.PlayBackgroundMusic(audioData, fadeInTime);
            MarkPlayed(audioData);
        }

        /// <summary>
        /// 切换背景音乐
        /// </summary>
        public void CrossFadeBackgroundMusic(AudioData audioData, float fadeOutTime = 1f, float fadeInTime = 1f)
        {
            if (bgmController != null)
            {
                AudioMgrController oldBgmController = bgmController;
                bgmController = null;
                oldBgmController.IsBackgroundMusic = false;
                oldBgmController.FadeOutAndStop(fadeOutTime);
            }

            PlayBackgroundMusic(audioData, fadeInTime);
        }

        /// <summary>
        /// 停止背景音乐
        /// </summary>
        public void StopBackgroundMusic(float fadeOutTime = 0f)
        {
            if (bgmController == null)
            {
                return;
            }

            bgmController.StopBackgroundMusic(fadeOutTime);
        }

        /// <summary>
        /// 注册活跃控制器
        /// </summary>
        public void RegisterActiveController(AudioMgrController controller)
        {
            if (controller == null || activeControllerList.Contains(controller))
            {
                return;
            }

            activeControllerList.Add(controller);
        }

        /// <summary>
        /// 取消注册活跃控制器
        /// </summary>
        public void UnRegisterActiveController(AudioMgrController controller)
        {
            if (controller == null)
            {
                return;
            }

            activeControllerList.Remove(controller);
        }

        /// <summary>
        /// 设置分类音量
        /// </summary>
        public void SetVolume(EAudioCategory category, float volume)
        {
            GetAudioGroup(category).SetVolume(volume);
            RefreshVolume(category);
        }

        /// <summary>
        /// 获取分类音量
        /// </summary>
        public float GetVolume(EAudioCategory category)
        {
            return GetAudioGroup(category).Volume;
        }

        /// <summary>
        /// 获取最终音量
        /// </summary>
        public float GetFinalVolume(EAudioCategory category)
        {
            AudioGroup masterGroup = GetAudioGroup(EAudioCategory.Master);
            AudioGroup categoryGroup = GetAudioGroup(category);

            if (masterGroup.Muted || categoryGroup.Muted)
            {
                return 0f;
            }

            if (category == EAudioCategory.Master)
            {
                return masterGroup.Volume;
            }

            return masterGroup.Volume * categoryGroup.Volume;
        }

        /// <summary>
        /// 设置分类静音
        /// </summary>
        public void SetMute(EAudioCategory category, bool mute)
        {
            GetAudioGroup(category).SetMute(mute);
            RefreshVolume(category);
        }

        /// <summary>
        /// 获取分类静音状态
        /// </summary>
        public bool GetMute(EAudioCategory category)
        {
            return GetAudioGroup(category).Muted;
        }

        /// <summary>
        /// 设置分类暂停
        /// </summary>
        public void SetPause(EAudioCategory category, bool pause)
        {
            GetAudioGroup(category).SetPause(pause);

            foreach (AudioMgrController controller in GetActiveControllerSnapshot())
            {
                if (controller.AudioData == null)
                {
                    continue;
                }

                if (category != EAudioCategory.Master && controller.AudioData.Category != category)
                {
                    continue;
                }

                if (pause)
                {
                    controller.PauseByGroup();
                }
                else if (!ShouldPause(controller.AudioData.Category))
                {
                    controller.Resume();
                }
            }
        }

        /// <summary>
        /// 获取分类暂停状态
        /// </summary>
        public bool GetPause(EAudioCategory category)
        {
            return GetAudioGroup(category).Paused;
        }

        /// <summary>
        /// 是否需要暂停指定分类
        /// </summary>
        public bool ShouldPause(EAudioCategory category)
        {
            return GetAudioGroup(EAudioCategory.Master).Paused || GetAudioGroup(category).Paused;
        }

        /// <summary>
        /// 设置混音器分组
        /// </summary>
        public void SetMixerGroup(EAudioCategory category, AudioMixerGroup mixerGroup)
        {
            GetAudioGroup(category).SetMixerGroup(mixerGroup);

            foreach (AudioMgrController controller in GetActiveControllerSnapshot())
            {
                if (controller.AudioData == null)
                {
                    continue;
                }

                if (category == EAudioCategory.Master || controller.AudioData.Category == category)
                {
                    controller.AudioSource.outputAudioMixerGroup = GetMixerGroup(controller.AudioData.Category);
                }
            }
        }

        /// <summary>
        /// 获取混音器分组
        /// </summary>
        public AudioMixerGroup GetMixerGroup(EAudioCategory category)
        {
            AudioMixerGroup categoryMixerGroup = GetAudioGroup(category).MixerGroup;
            if (categoryMixerGroup != null)
            {
                return categoryMixerGroup;
            }

            return GetAudioGroup(EAudioCategory.Master).MixerGroup;
        }

        /// <summary>
        /// 停止指定分类音频
        /// </summary>
        public void StopByCategory(EAudioCategory category, float fadeOutTime = 0f)
        {
            foreach (AudioMgrController controller in GetActiveControllerSnapshot())
            {
                if (controller.AudioData == null)
                {
                    continue;
                }

                if (category == EAudioCategory.Master || controller.AudioData.Category == category)
                {
                    StopController(controller, fadeOutTime);
                }
            }
        }

        /// <summary>
        /// 刷新分类音量
        /// </summary>
        private void RefreshVolume(EAudioCategory category)
        {
            foreach (AudioMgrController controller in GetActiveControllerSnapshot())
            {
                if (controller.AudioData == null)
                {
                    continue;
                }

                if (category == EAudioCategory.Master || controller.AudioData.Category == category)
                {
                    controller.RefreshVolume();
                }
            }
        }

        /// <summary>
        /// 停止指定播放类型音频
        /// </summary>
        private void StopByPlayType(EAudioPlayType playType, float fadeOutTime, bool includeBackgroundMusic)
        {
            foreach (AudioMgrController controller in GetActiveControllerSnapshot())
            {
                if (!includeBackgroundMusic && controller.IsBackgroundMusic)
                {
                    continue;
                }

                if (controller.AudioPlayType == playType)
                {
                    StopController(controller, fadeOutTime);
                }
            }
        }

        /// <summary>
        /// 停止控制器
        /// </summary>
        private void StopController(AudioMgrController controller, float fadeOutTime)
        {
            if (fadeOutTime > 0f)
            {
                controller.FadeOutAndStop(fadeOutTime);
            }
            else
            {
                controller.Stop();
            }
        }

        /// <summary>
        /// 是否可以播放
        /// </summary>
        private bool CanPlay(AudioData audioData)
        {
            if (audioData == null)
            {
                Log.ErrorForce("AudioData为空，无法播放");
                return false;
            }

            if (string.IsNullOrEmpty(audioData.AudioPath))
            {
                Log.ErrorForce("AudioPath为空，无法播放");
                return false;
            }

            if (audioData.Cooldown > 0f && audioLastPlayTimeDict.TryGetValue(audioData.AudioPath, out float lastPlayTime))
            {
                if (Time.time - lastPlayTime < audioData.Cooldown)
                {
                    return false;
                }
            }

            if (audioData.MaxSameAudioCount > 0 && CountSameAudio(audioData.AudioPath) >= audioData.MaxSameAudioCount)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 标记音频已播放
        /// </summary>
        private void MarkPlayed(AudioData audioData)
        {
            if (audioData == null || string.IsNullOrEmpty(audioData.AudioPath))
            {
                return;
            }

            audioLastPlayTimeDict[audioData.AudioPath] = Time.time;
        }

        /// <summary>
        /// 统计同路径音频数量
        /// </summary>
        private int CountSameAudio(string audioPath)
        {
            int count = 0;
            foreach (AudioMgrController controller in activeControllerList)
            {
                if (controller.AudioData != null && controller.AudioData.AudioPath == audioPath)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// 获取活跃控制器快照
        /// </summary>
        private List<AudioMgrController> GetActiveControllerSnapshot()
        {
            activeControllerSnapshotCache.Clear();
            activeControllerSnapshotCache.AddRange(activeControllerList);
            return activeControllerSnapshotCache;
        }

        /// <summary>
        /// 获取音频分组
        /// </summary>
        private AudioGroup GetAudioGroup(EAudioCategory category)
        {
            if (!audioGroupDict.TryGetValue(category, out AudioGroup group))
            {
                group = new AudioGroup(category);
                audioGroupDict.Add(category, group);
            }

            return group;
        }

        protected override void OnDispose()
        {
            foreach (AudioMgrController controller in GetActiveControllerSnapshot())
            {
                StopController(controller, 0f);
            }

            activeControllerList.Clear();
            activeControllerSnapshotCache.Clear();
            audioLastPlayTimeDict.Clear();
            audioGroupDict.Clear();
            bgmController = null;
        }

        public AudioMgrController PopCon()
        {
            return pool.Pop().GetComponent<AudioMgrController>();
        }

        public void PushCon(AudioMgrController controller)
        {
            if (controller == null || controller.IsBackgroundMusic)
            {
                return;
            }

            UnRegisterActiveController(controller);
            pool.Push(controller.gameObject);
        }

        /// <summary>
        /// Audio物体出池初始化
        /// </summary>
        private GameObject OnPopAudioGo(GameObject go)
        {
            AudioMgrController controller = go.GetComponent<AudioMgrController>();
            if (controller == null)
            {
                controller = go.AddComponent<AudioMgrController>();
            }
            else
            {
                controller.Clear();
            }

            go.SetActive(true);
            return go;
        }

        protected override void OnInit()
        {
            foreach (EAudioCategory category in Enum.GetValues(typeof(EAudioCategory)))
            {
                GetAudioGroup(category);
            }

            GameObject audioTemplate = new GameObject("AudioTemplate");
            audioTemplate.AddComponent<AudioSource>();
            audioTemplate.AddComponent<AudioMgrController>();
            pool = new GameObjectPool(audioTemplate, OnPopAudioGo, transform);
        }
    }
}
