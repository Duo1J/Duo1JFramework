using System;
using UnityEngine;

namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// GameObject对象池
    /// </summary>
    public class GameObjectPool : CommonPool<GameObject>
    {
        public Transform ParentRoot => parentOverride == null ? Root.Instance.GoPoolRoot.transform : parentOverride.parent;

        private GameObject templateGo;
        private Transform parentOverride;

        public override void OnPushObject(GameObject o)
        {
            base.OnPushObject(o);
            o.SetParent(ParentRoot);
            o.SetActive(false);
        }

        public override void InitPool()
        {
            pool = new GObjectPool(() => templateGo);
        }

        public GameObjectPool(GameObject templateGo, Func<GameObject, GameObject> initCall, Transform parentOverride = null) : base(initCall)
        {
            Assert.NotNull(templateGo, "GameObjectPool构造参数错误，`templateGo`为空");

            this.templateGo = templateGo;
            this.parentOverride = parentOverride;

            templateGo.SetParent(ParentRoot);
            templateGo.SetActive(false);
        }
    }
}