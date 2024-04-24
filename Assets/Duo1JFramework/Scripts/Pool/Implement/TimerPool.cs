using Duo1JFramework.TimerUpdate;

namespace Duo1JFramework.ObjectPool
{
    public class TimerPool : BaseObjectPool<Timer>
    {
        public override void OnPopObject(Timer o)
        {
            o.Dispose();
        }
    }
}
