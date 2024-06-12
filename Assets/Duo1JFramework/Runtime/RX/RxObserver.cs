using System;

namespace Duo1JFramework.RX
{
    /// <summary>
    /// 响应式监听者
    /// </summary>
    public class RxObserver
    {
        private Action subscribeCall;
        private Func<bool> whereCall;
        private Action endCall;

        private byte updateFlag = (byte)UpdateFlag.None;

        private int cnt = 0;
        private int maxCnt = -1;

        public bool Ended { get; private set; } = false;

        #region API

        public void Subscribe(Action subscribeCall)
        {
            this.subscribeCall = subscribeCall;
        }

        public RxObserver Where(Func<bool> whereCall)
        {
            this.whereCall = whereCall;
            return this;
        }

        public RxObserver Update(bool open = true)
        {
            if (open)
            {
                updateFlag = (byte)UpdateFlag.Update;
            }
            else
            {
                updateFlag = (byte)(updateFlag & ~(byte)UpdateFlag.Update);
            }

            return this;
        }

        public RxObserver FixedUpdate(bool open = true)
        {
            if (open)
            {
                updateFlag = (byte)UpdateFlag.FixedUpdate;
            }
            else
            {
                updateFlag = (byte)(updateFlag & ~(byte)UpdateFlag.FixedUpdate);
            }

            return this;
        }

        public RxObserver LateUpdate(bool open = true)
        {
            if (open)
            {
                updateFlag = (byte)UpdateFlag.LateUpdate;
            }
            else
            {
                updateFlag = (byte)(updateFlag & ~(byte)UpdateFlag.LateUpdate);
            }

            return this;
        }

        public RxObserver MaxCnt(int maxCnt)
        {
            this.maxCnt = maxCnt;
            return this;
        }

        public RxObserver End(Action endCall)
        {
            this.endCall = endCall;
            return this;
        }

        #endregion API

        #region Inner

        public void _OnSubscribe()
        {
            if (Ended)
            {
                return;
            }

            if (subscribeCall == null)
            {
                return;
            }

            if (whereCall != null && !whereCall())
            {
                return;
            }

            subscribeCall();

            cnt++;
            if (maxCnt >= 0 && cnt >= maxCnt)
            {
                Rx.Instance.End(this);
            }
        }

        public void _OnUpdate()
        {
            if (Ended)
            {
                return;
            }

            if ((updateFlag & (byte)UpdateFlag.Update) > 0)
            {
                _OnSubscribe();
            }
        }

        public void _OnFixedUpdate()
        {
            if (Ended)
            {
                return;
            }

            if ((updateFlag & (byte)UpdateFlag.FixedUpdate) > 0)
            {
                _OnSubscribe();
            }

        }

        public void _OnLateUpdate()
        {
            if (Ended)
            {
                return;
            }

            if ((updateFlag & (byte)UpdateFlag.LateUpdate) > 0)
            {
                _OnSubscribe();
            }
        }

        public void _OnEnd()
        {
            endCall?.Invoke();
            endCall = null;
            Ended = true;
        }

        public void Clear()
        {
            _OnEnd();
            subscribeCall = null;
            whereCall = null;

            updateFlag = (byte)UpdateFlag.None;

            cnt = 0;
            maxCnt = -1;

            Ended = false;
        }

        ~RxObserver()
        {
            _OnEnd();
        }

        #endregion Inner

        enum UpdateFlag
        {
            None = 0,
            Update = 1,
            FixedUpdate = 1 << 1,
            LateUpdate = 1 << 2,
        }
    }
}
