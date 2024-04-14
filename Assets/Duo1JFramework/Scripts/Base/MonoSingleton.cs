using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// MonoBehaviour单例基类
    /// </summary>
    public abstract class MonoSingleton<T> : MonoRegister, IDispose where T : BaseMono
    {
        private static T instance;

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

        private void Awake()
        {
            if (instance == null)
            {
                instance = this.Convert<T>();
                DontDestroyOnLoad(gameObject);
                if (AddToRoot && transform.parent != Root.Instance.SingletonRoot)
                {
                    transform.SetParent(Root.Instance.SingletonRoot.transform);
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