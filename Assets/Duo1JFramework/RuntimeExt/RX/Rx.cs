using Duo1JFramework.Ext.ObjectPool;
using System.Collections.Generic;

namespace Duo1JFramework.Ext.RX
{
    /// <summary>
    /// 响应式API
    /// </summary>
    public class Rx : MonoSingleton<Rx>
    {
        #region API

        /// <summary>
        /// 创建观察者
        /// </summary>
        public static RxObserver Observer
        {
            get
            {
                RxObserver observer = Pool.RxObserverPool.Pop();
                Instance.observerList.Add(observer);
                return observer;
            }
        }

        #endregion API

        #region Inner

        private List<RxObserver> observerList;
        private List<int> removeList;

        private void OnUpdate()
        {
            observerList.ForEach((observer) =>
            {
                observer._OnUpdate();
            });
        }

        private void OnFixedUpdate()
        {
            observerList.ForEach((observer) =>
            {
                observer._OnFixedUpdate();
            });
        }

        private void OnLateUpdate()
        {
            observerList.ForEach((observer) =>
            {
                observer._OnLateUpdate();
            });

            removeList.Sort((lhs, rhs) =>
            {
                return rhs - lhs;
            });

            removeList.ForEach((remIdx) =>
            {
                observerList.RemoveAt(remIdx);
            });
            removeList.Clear();
        }

        public void End(RxObserver observer)
        {
            int remIdx = -1;
            for (int i = 0; i < observerList.Count; i++)
            {
                if (observer == observerList[i])
                {
                    remIdx = i;
                    break;
                }
            }

            if (remIdx >= 0)
            {
                Pool.RxObserverPool.Push(observerList[remIdx]);
                removeList.Add(remIdx);
            }

            observer._OnEnd();
        }

        protected override void OnDispose()
        {
            for (int i = 0; i < observerList.Count; i++)
            {
                RxObserver observer = observerList[i];
                Pool.RxObserverPool.Push(observer);
                observer._OnEnd();
            }

            observerList.Clear();
            observerList = null;
            removeList.Clear();
            removeList = null;
        }

        protected override void OnInit()
        {
            observerList = new List<RxObserver>();
            removeList = new List<int>();

            Register.RegisterUpdate(OnUpdate);
            Register.RegisterFixedUpdate(OnFixedUpdate);
            Register.RegisterLateUpdate(OnLateUpdate);
        }

        #endregion Inner
    }
}
