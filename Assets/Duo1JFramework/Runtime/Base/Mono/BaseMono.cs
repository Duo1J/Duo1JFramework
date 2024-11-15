using Duo1JFramework.Asset;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 所有MonoBehaviour的基类
    /// </summary>
    public abstract class BaseMono : MonoBehaviour
    {
        /// <summary>
        /// 资源加载集合
        /// </summary>
        public AssetCollection Asset
        {
            get
            {
                if (assetCollection == null)
                {
                    assetCollection = new AssetCollection();
                }

                if (IsSingleton)
                {
                    Log.Warn("单例使用 `AssetManager` 进行资源加载");
                }

                return assetCollection;
            }
        }

        private AssetCollection assetCollection;

        /// <summary>
        /// 是否是单例
        /// </summary>
        public virtual bool IsSingleton => false;

        /// <summary>
        /// 缓存transform
        /// </summary>
        public Transform transformCache
        {
            get
            {
                if (_transformCache == null)
                {
                    _transformCache = transform;
                }

                return _transformCache;
            }
        }

        private Transform _transformCache;

        /// <summary>
        /// 缓存gameObject
        /// </summary>
        public GameObject gameObjectCache
        {
            get
            {
                if (_gameObjectCache == null)
                {
                    _gameObjectCache = gameObject;
                }

                return _gameObjectCache;
            }
        }

        private GameObject _gameObjectCache;

        /// <summary>
        /// 设置组件是否可用
        /// </summary>
        public void SetEnabled(bool enabled)
        {
            this.enabled = enabled;
        }

        /// <summary>
        /// 获取或添加组件
        /// </summary>
        public T GetOrAddCom<T>() where T : Component
        {
            return this.GetOrAddComponent<T>();
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public T AddCom<T>() where T : Component
        {
            return gameObjectCache.AddComponent<T>();
        }

        public override string ToString()
        {
            return $"<Mono-{GetType().Name}-{name}: {GetInstanceID()}>";
        }

        protected virtual void OnDestroy()
        {
            if (assetCollection != null)
            {
                assetCollection.Dispose();
                assetCollection = null;
            }
        }
    }
}
