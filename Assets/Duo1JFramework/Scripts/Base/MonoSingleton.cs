using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// MonoBehaviour单例基类
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        private static GameObject singletonRoot;

        private bool dispose = false;

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

        /// <summary>
        /// 单例物体根节点
        /// </summary>
        public static GameObject SingletonRoot
        {
            get
            {
                if (singletonRoot == null)
                {
                    singletonRoot = new GameObject("SingletonRoot");
                    DontDestroyOnLoad(singletonRoot);
                }
                return singletonRoot;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
                if (AddToRoot && transform.parent != SingletonRoot)
                {
                    transform.SetParent(SingletonRoot.transform);
                }
            }
            else if (instance != this)
            {
                DestroyImmediate(gameObject);
            }

            OnInit();
        }

        /// <summary>
        /// 清除该单例
        /// </summary>
        public void Dispose()
        {
            if (dispose)
            {
                return;
            }
            dispose = true;
            OnDispose();
            DestroyImmediate(gameObject);
        }

        /// <summary>
        /// 单例触发
        /// </summary>
        public virtual void Trigger()
        {
        }

        protected abstract void OnInit();

        protected abstract void OnDispose();
    }
}