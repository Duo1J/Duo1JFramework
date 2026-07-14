namespace Duo1JFramework.ParticleAPI
{
    /// <summary>
    /// 挂载驱动粒子控制器
    /// </summary>
    public class ParticleController : BaseParticleController
    {
        public ParticleData data;

        protected override void OnAwake()
        {
            base.OnAwake();

            if (data == null)
            {
                Log.ErrorForce("ParticleController data为空, 无法播放");
                return;
            }

            switch (particlePlayType)
            {
                case EParticlePlayType.OneShot:
                    PlayOneShot(data);
                    break;
                case EParticlePlayType.Keep:
                    PlayKeep(data);
                    break;
                default:
                    Log.ErrorForce($"ParticleController::OnAwake 未处理的粒子播放类型: `{particlePlayType}`");
                    break;
            }
        }
    }
}
