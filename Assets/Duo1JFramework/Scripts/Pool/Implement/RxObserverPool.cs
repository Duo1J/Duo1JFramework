using Duo1JFramework.RX;

namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// 响应式监听者池
    /// </summary>
    public class RxObserverPool : BaseObjectPool<RxObserver>
    {
        public override RxObserver InitObject(RxObserver o)
        {
            o.Clear();
            return o;
        }
    }
}
