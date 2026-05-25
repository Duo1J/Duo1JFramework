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
        /// 是否正在销毁或已销毁
        /// </summary>
        private static bool disposingOrDisposed = false;

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
                    if (Game.IsQuit || disposingOrDisposed)
                    {
                        Log.ErrorForce($"`{typeof(T).FullName}` 正处于退出/销毁阶段，禁止通过Instance重新创建，请在调用前判断 `Game.IsQuit` 或使用 `TryGetInstance`");
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
                    disposingOrDisposed = false;
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
            if (Game.IsQuit || disposingOrDisposed)
            {
                Log.ErrorForce($"`{typeof(T).FullName}` 在退出/销毁阶段被唤醒，已销毁当前物体: `{gameObject.name}`");
                gameObject?.DestroySmart();
                return;
            }

            if (instance == null)
            {
                Instance = this.Convert<T>();
                DontDestroyOnLoad(gameObject);
                if (AddToRoot && transform.parent != Root.SingletonRoot)
                {
                    gameObject.SetParent(Root.SingletonRoot);
                }
            }
            else if (!ReferenceEquals(instance, this))
            {
                Log.ErrorForce($"检测到重复Mono单例 `{typeof(T).FullName}`，保留 `{instance.gameObject.name}`，销毁重复物体 `{gameObject.name}`，请检查场景或预制体中是否误放了多个实例");
                gameObject?.DestroySmart();
                return;
            }

            OnInit();
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

            bool isCurrentInstance = ReferenceEquals(instance, this);
            if (isCurrentInstance)
            {
                disposingOrDisposed = true;
            }
            dispose = true;

            OnDispose();

            if (!goDestroyed && gameObject != null)
            {
                goDestroyed = true;
                gameObject?.DestroySmart();
            }

            if (isCurrentInstance)
            {
                Instance = null;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            goDestroyed = true;
            if (instance == this)
            {
                disposingOrDisposed = true;
                Dispose();
            }
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
