using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// MonoBehaviour单例基类
    /// </summary>
    public abstract class MonoSingleton<T> : MonoRegister, ISingleton where T : BaseMono
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static T instance;

        /// <summary>
        /// 是否是单例
        /// </summary>
        public override bool IsSingleton => true;

        /// <summary>
        /// 是否已销毁
        /// </summary>
        private bool dispose = false;

        /// <summary>
        /// Go是否已销毁
        /// </summary>
        private bool goDestroyed = false;

        /// <summary>
        /// 是否需要添加到统一管理的节点下
        /// </summary>
        protected virtual bool AddToRoot { get; set; } = true;

        /// <summary>
        /// 获取单例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    if (Game.IsQuit)
                    {
                        Log.ErrorForce($"游戏状态已退出，但仍在创建{typeof(T).FullName}，请使用 `Game.IsQuit` 判断处理");
                        return null;
                    }
                    Instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject(typeof(T).Name);
                        go.AddComponent<T>();
                    }
                }
                return instance;
            }
            private set
            {
                T oldInstance = instance;
                instance = value;

                if (instance != null)
                {
                    SingletonManager.AddMonoSingleton(instance as ISingleton);
                }

                if (oldInstance != null)
                {
                    SingletonManager.RemoveMonoSingleton(oldInstance as ISingleton);
                }
            }
        }

        public bool IsDisposed
        {
            get => dispose;
            protected set => dispose = value;
        }

        /// <summary>
        /// 尝试获取已存在的单例，不会触发创建
        /// </summary>
        public static bool TryGetInstance(out T value)
        {
            value = instance;
            return value != null;
        }

        private void Awake()
        {
            if (instance == null)
            {
                Instance = this.Convert<T>();
                DontDestroyOnLoad(gameObject);
                if (AddToRoot && transform.parent != Root.SingletonRoot)
                {
                    gameObject.SetParent(Root.SingletonRoot);
                }
            }
            else if (instance != this)
            {
                DestroySelfGameObject();
                return;
            }

            OnInit();
        }

        private void DestroySelfGameObject()
        {
            if (gameObject == null)
            {
                return;
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DestroyImmediate(gameObject);
                return;
            }
#endif
            Destroy(gameObject);
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            if (dispose)
            {
                return;
            }
            dispose = true;

            OnDispose();

            if (!goDestroyed && gameObject != null)
            {
                goDestroyed = true;
                DestroySelfGameObject();
            }

            Instance = null;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            goDestroyed = true;
            Dispose();
        }

        /// <summary>
        /// 触发该单例
        /// </summary>
        public void Trigger()
        {
        }

        /// <summary>
        /// 子类初始化
        /// </summary>
        protected abstract void OnInit();

        /// <summary>
        /// 子类销毁
        /// </summary>
        protected abstract void OnDispose();
    }
}
