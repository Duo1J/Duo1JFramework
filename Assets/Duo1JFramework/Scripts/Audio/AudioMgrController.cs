namespace Duo1JFramework.AudioAPI
{
    /// <summary>
    /// 管理器驱动音频控制器
    /// </summary>
    public class AudioMgrController : BaseAudioController
    {
        /// <summary>
        /// 是否是背景音乐
        /// </summary>
        public bool IsBackgroundMusic { get; set; }

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

            audioPlayType = EAudioPlayType.Keep;
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

        protected override void OnStop()
        {
            base.OnStop();

            if (!IsBackgroundMusic)
            {
                AudioManager.Instance.PushCon(this);
            }
        }

        protected override void OnClear()
        {
            base.OnClear();
            IsBackgroundMusic = false;
        }
    }
}