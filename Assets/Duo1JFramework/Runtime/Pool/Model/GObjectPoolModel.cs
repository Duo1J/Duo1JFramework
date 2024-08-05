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
            GameObject templateGo = getTemplateCall();
            Assert.NotNull(templateGo, "GObjectPool::CreateNew 异常，`templateGo`为空");

            ObjectPoolItem<GameObject> newItem = new ObjectPoolItem<GameObject>(UObject.Instantiate(templateGo));
            OnCreateNew?.Invoke(newItem);
            newItem.Using = true;

            return newItem;
        }

        public GObjectPoolModel(Func<GameObject> getTemplateCall)
        {
            Assert.NotNull(getTemplateCall, "GObjectPool 构造参数错误，`getTemplateCall`为空");
            this.getTemplateCall = getTemplateCall;
        }
    }
}