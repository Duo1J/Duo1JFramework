using Duo1JFramework.ObjectPool;
using Duo1JFramework.Ext.RX;

namespace Duo1JFramework.Ext.ObjectPool
{
    /// <summary>
    /// 响应式监听者池
    /// </summary>
    public class RxObserverPool : BaseObjectPool<RxObserver>
    {
        public override void OnPopObject(RxObserver o)
        {
            o.Clear();
        }
    }
}
