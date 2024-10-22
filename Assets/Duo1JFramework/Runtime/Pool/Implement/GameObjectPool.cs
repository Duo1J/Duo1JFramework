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

        public override void OnPushObject(GameObject o)
        {
            base.OnPushObject(o);
            o.SetParent(ParentRoot);
            o.SetActive(false);
        }

        public override void OnPopObject(GameObject o)
        {
            base.OnPopObject(o);
            o.SetParent(ParentRoot);
        }

        public override void InitPool()
        {
            pool = new GObjectPoolModel(() => templateGo);
        }

        public GameObjectPool(GameObject templateGo, Func<GameObject, GameObject> initCall, Transform parentOverride = null) : base(initCall)
        {
            Assert.NotNullArg(templateGo, "templateGo");

            this.templateGo = templateGo;
            this.parentOverride = parentOverride;

            templateGo.SetParent(ParentRoot);
            templateGo.SetActive(false);
        }
    }
}
