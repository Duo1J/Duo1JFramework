using System;

namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// 通用对象池
    /// </summary>
    public class CommonPool<T> : BaseObjectPool<T> where T : new()
    {
        protected Func<T, T> InitCall;

        public static CommonPool<T> Create(Func<T, T> initCall)
        {
            return new CommonPool<T>(initCall);
        }

        public override T InitObject(T o)
        {
            o = base.InitObject(o);
            if (InitCall != null)
            {
                o = InitCall(o);
            }

            return o;
        }

        public CommonPool(Func<T, T> initCall)
        {
            InitCall = initCall;
        }
    }
}