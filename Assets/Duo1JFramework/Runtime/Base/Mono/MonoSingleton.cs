using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// MonoBehaviour单例基类
    /// </summary>
    public abstract class MonoSingleton<T> : MonoRegister, IDispose where T : BaseMono
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static T instance;

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
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject(typeof(T).Name);
                        go.AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this.Convert<T>();
                DontDestroyOnLoad(gameObject);
                if (AddToRoot && transform.parent != Root.SingletonRoot)
                {
                    gameObject.SetParent(Root.SingletonRoot);
                }
            }
            else if (instance != this)
            {
                DestroyImmediate(gameObject);
            }

            OnInit();
        }

        /// <summary>
        /// 销毁该单例
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
                DestroyImmediate(gameObject);
            }
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