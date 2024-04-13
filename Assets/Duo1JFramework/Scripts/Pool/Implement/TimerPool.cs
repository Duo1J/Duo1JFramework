using Duo1JFramework.TimerUpdate;

namespace Duo1JFramework.ObjectPool
{
    public class TimerPool : BaseObjectPool<Timer>
    {
        public override Timer InitObject(Timer o)
        {
            o.Dispose();
            return o;
        }
    }
}
