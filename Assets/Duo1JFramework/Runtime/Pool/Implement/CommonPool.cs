using System;

namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// 通用对象池实例
    /// </summary>
    public class CommonPool<T> : BaseObjectPool<T> where T : class, new()
    {
        protected Func<T, T> OnPopCall;

        public override void OnPopObject(T o)
        {
            base.OnPopObject(o);
            OnPopCall?.Invoke(o);
        }

        public CommonPool(Func<T, T> onPopCall)
        {
            OnPopCall = onPopCall;
        }
    }
}
