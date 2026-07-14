namespace Duo1JFramework.ParticleAPI
{
    /// <summary>
    /// 管理器驱动粒子控制器
    /// </summary>
    public class ParticleMgrController : BaseParticleController
    {
        protected override void OnStop()
        {
            base.OnStop();

            if (ParticleManager.TryGetInstance(out ParticleManager particleManager))
            {
                particleManager.PushCon(this);
            }
        }
    }
}
