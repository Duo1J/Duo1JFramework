namespace Duo1JFramework.AudioAPI
{
    /// <summary>
    /// 挂载驱动音频控制器
    /// </summary>
    public class AudioController : BaseAudioController
    {
        public AudioData data;

        protected override void OnAwake()
        {
            base.OnAwake();

            switch (audioPlayType)
            {
                case eAudioPlayType.OneShot:
                    PlayOneShot(data);
                    break;
                case eAudioPlayType.Keep:
                    PlayKeep(data);
                    break;
                default:
                    Log.ErrorForce($"AudioController::OnAwake 未处理的音频播放类型: `{audioPlayType}`");
                    break;
            }
        }
    }
}
