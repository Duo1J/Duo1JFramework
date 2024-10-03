using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 基础MonoBehaviour
    /// </summary>
    public abstract class BaseMono : MonoBehaviour
    {
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

        public override string ToString()
        {
            return $"<Mono-{GetType().Name}-{name}: {GetInstanceID()}>";
        }
    }
}
