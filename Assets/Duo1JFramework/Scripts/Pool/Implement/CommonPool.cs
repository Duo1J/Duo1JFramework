using System;

namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// 通用对象池
    /// </summary>
    public class CommonPool<T> : BaseObjectPool<T> where T : class, new()
    {
        protected Func<T, T> OnPopCall;

        public override T OnPopObject(T o)
        {
            o = base.OnPopObject(o);
            if (OnPopCall != null)
            {
                o = OnPopCall(o);
            }

            return o;
        }

        public CommonPool(Func<T, T> onPopCall)
        {
            OnPopCall = onPopCall;
        }
    }
}