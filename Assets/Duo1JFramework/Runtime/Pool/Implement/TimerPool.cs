using Duo1JFramework.TimerUpdate;

namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// Timer对象池实例
    /// </summary>
    public class TimerPool : BaseObjectPool<Timer>
    {
        public override void OnPopObject(Timer o)
        {
            o.Dispose();
        }
    }
}
