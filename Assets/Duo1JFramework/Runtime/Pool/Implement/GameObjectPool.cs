using System;
using UnityEngine;

namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// GameObject对象池实例
    /// </summary>
    public class GameObjectPool : CommonPool<GameObject>
    {
        public Transform ParentRoot => parentOverride == null ? Root.GoPoolRoot.transform : parentOverride;

        private GameObject templateGo;
        private Transform parentOverride;
        private bool activeOnPop;
        private bool inactiveOnPush;

        public override void OnPushObject(GameObject o)
        {
            base.OnPushObject(o);

            if (o == null)
            {
                return;
            }

            o.SetParent(ParentRoot);

            if (inactiveOnPush)
            {
                o.SetActive(false);
            }
        }

        public override void OnPopObject(GameObject o)
        {
            base.OnPopObject(o);

            if (o == null)
            {
                return;
            }

            o.SetParent(ParentRoot);

            if (activeOnPop)
            {
                o.SetActive(true);
            }
        }

        protected override bool IsValidObject(GameObject o)
        {
            return o != null;
        }

        public override void InitPool()
        {
            SetPool(new GObjectPoolModel(() => templateGo, poolItemList?.Capacity ?? 0));
        }

        public GameObjectPool(GameObject templateGo, Func<GameObject, GameObject> initCall, Transform parentOverride = null, bool activeOnPop = false, bool inactiveOnPush = true, int initialCapacity = 0, int prewarmCount = 0) : base(initCall, initialCapacity)
        {
            Assert.NotNullArg(templateGo, "templateGo");

            this.templateGo = templateGo;
            this.parentOverride = parentOverride;
            this.activeOnPop = activeOnPop;
            this.inactiveOnPush = inactiveOnPush;

            InitPool();

            templateGo.SetParent(ParentRoot);

            if (inactiveOnPush)
            {
                templateGo.SetActive(false);
            }

            Prewarm(prewarmCount);
        }
    }
}
