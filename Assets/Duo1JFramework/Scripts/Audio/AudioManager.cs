using Duo1JFramework.Event;
using Duo1JFramework.ObjectPool;
using UnityEngine;

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
        /// 背景音乐控制器
        /// </summary>
        private AudioController BgmController
        {
            get
            {
                if (bgmController == null)
                {
                    bgmController = PopCon().GetComponent<AudioController>();
                    bgmController.IsBackgroundMusic = true;
                }

                return bgmController;
            }
        }
        private AudioController bgmController;

        /// <summary>
        /// 持续播放
        /// </summary>
        public AudioController PlayKeep(AudioData audioData)
        {
            AudioController controller = PopCon();
            controller.PlayKeep(audioData);
            return controller;
        }

        /// <summary>
        /// 停止所有持续音频播放
        /// </summary>
        public void StopAllKeep()
        {
            EventManager.Instance.Broadcast(eEvent.AUDIO_STOP_ALL_KEEP);
        }

        /// <summary>
        /// 单次播放
        /// </summary>
        public AudioController PlayOneShot(AudioData audioData)
        {
            AudioController controller = PopCon();
            controller.PlayOneShot(audioData);
            return controller;
        }

        /// <summary>
        /// 停止所有单次音频播放
        /// </summary>
        public void StopAllOneShot()
        {
            EventManager.Instance.Broadcast(eEvent.AUDIO_STOP_ALL_ONE_SHOT);
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        public void PlayBackgroundMusic(AudioData audioData)
        {
            BgmController.PlayBackgroundMusic(audioData);
        }

        /// <summary>
        /// 停止背景音乐
        /// </summary>
        public void StopBackgroundMusic()
        {
            BgmController.StopBackgroundMusic();
        }

        protected override void OnDispose()
        {
        }

        public AudioController PopCon()
        {
            return pool.Pop().GetComponent<AudioController>();
        }

        public void PushCon(AudioController controller)
        {
            pool.Push(controller.gameObject);
        }

        /// <summary>
        /// Audio物体出池初始化
        /// </summary>
        private GameObject OnPopAudioGo(GameObject go)
        {
            AudioController controller = go.GetComponent<AudioController>();
            if (controller == null)
            {
                controller = go.AddComponent<AudioController>();
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
            GameObject audioTemplate = new GameObject("AudioTemplate");
            audioTemplate.AddComponent<AudioSource>();
            audioTemplate.AddComponent<AudioController>();
            pool = new GameObjectPool(audioTemplate, OnPopAudioGo, transform);
        }
    }
}