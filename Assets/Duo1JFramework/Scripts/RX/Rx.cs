using Duo1JFramework.ObjectPool;
using System.Collections.Generic;

namespace Duo1JFramework.RX
{
    /// <summary>
    /// 响应式API
    /// </summary>
    public partial class Rx : MonoSingleton<Rx>
    {
        private List<ObjectPoolItem<RxObserver>> observerList;
        private List<int> removeList;

        public static RxObserver Observer
        {
            get
            {
                ObjectPoolItem<RxObserver> poolItem = Pool.RxObserverPool.Pop();
                Instance.observerList.Add(poolItem);

                return poolItem.Value;
            }
        }

        private void OnUpdate()
        {
            observerList.ForEach((observer) =>
            {
                observer.Value._OnUpdate();
            });
        }

        private void OnFixedUpdate()
        {
            observerList.ForEach((observer) =>
            {
                observer.Value._OnFixedUpdate();
            });
        }

        private void OnLateUpdate()
        {
            removeList.ForEach((remIdx) =>
            {
                observerList.RemoveAt(remIdx);
            });
            removeList.Clear();

            observerList.ForEach((observer) =>
            {
                observer.Value._OnLateUpdate();
            });
        }

        public void End(RxObserver observer)
        {
            int remIdx = -1;
            for (int i = 0; i < observerList.Count; i++)
            {
                if (observer == observerList[i].Value)
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
                ObjectPoolItem<RxObserver> observer = observerList[i];
                Pool.RxObserverPool.Push(observer);
                observer.Value._OnEnd();
            }

            observerList.Clear();
            observerList = null;
            removeList.Clear();
            removeList = null;
        }

        protected override void OnInit()
        {
            observerList = new List<ObjectPoolItem<RxObserver>>();
            removeList = new List<int>();

            Register.RegisterUpdate(OnUpdate);
            Register.RegisterFixedUpdate(OnFixedUpdate);
            Register.RegisterLateUpdate(OnLateUpdate);
        }
    }
}
