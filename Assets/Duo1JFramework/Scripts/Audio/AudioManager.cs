namespace Duo1JFramework.AudioAPI
{
    /// <summary>
    /// 音频管理器
    /// </summary>
    public class AudioManager : MonoSingleton<AudioManager>
    {
        /// <summary>
        /// 持续播放
        /// </summary>
        public AudioData PlayKeep()
        {
            //todo hlj
            return null;
        }

        /// <summary>
        /// 停止所有持续播放
        /// </summary>
        public void StopAllKeep()
        {

        }

        /// <summary>
        /// 单次播放
        /// </summary>
        public AudioData PlayOneShot()
        {
            return null;
        }

        /// <summary>
        /// 停止所有单次播放
        /// </summary>
        public void StopAllOneShot()
        {

        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        public AudioData PlayBackgroundMusic()
        {
            return null;
        }

        /// <summary>
        /// 停止背景音乐
        /// </summary>
        public void StopBackgroundMusic()
        {

        }

        protected override void OnDispose()
        {
        }

        protected override void OnInit()
        {
        }
    }
}