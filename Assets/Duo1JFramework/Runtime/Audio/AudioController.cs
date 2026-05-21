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

            if (data == null)
            {
                Log.ErrorForce("AudioController data为空，无法播放");
                return;
            }

            switch (audioPlayType)
            {
                case EAudioPlayType.OneShot:
                    PlayOneShot(data);
                    break;
                case EAudioPlayType.Keep:
                    PlayKeep(data);
                    break;
                default:
                    Log.ErrorForce($"AudioController::OnAwake 未处理的音频播放类型: `{audioPlayType}`");
                    break;
            }
        }
    }
}
