using System;
using UnityEngine;

using UObject = UnityEngine.Object;

namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// GameObject对象池实现
    /// </summary>
    public class GObjectPoolModel : ObjectPoolModel<GameObject>
    {
        private Func<GameObject> getTemplateCall;

        public override ObjectPoolItem<GameObject> CreateNew()
        {
            GameObject templateGo = getTemplateCall?.Invoke();
            Assert.NotNull(templateGo, "ObjectPoolItem::CreateNew异常, `templateGo` 为空");

            ObjectPoolItem<GameObject> newItem = new ObjectPoolItem<GameObject>(this, UObject.Instantiate(templateGo));
            OnCreateNew?.Invoke(newItem);
            newItem.Using = true;

            return newItem;
        }

        protected override void DestroyItem(ObjectPoolItem<GameObject> item)
        {
            GameObject value = item?.Value;
            base.DestroyItem(item);

            value?.DestroySmart();
        }

        public GObjectPoolModel(Func<GameObject> getTemplateCall)
        {
            Assert.NotNullArg(getTemplateCall, "getTemplateCall");
            this.getTemplateCall = getTemplateCall;
        }
    }
}