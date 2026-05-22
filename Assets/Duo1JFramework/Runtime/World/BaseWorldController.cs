using System;
using UnityEngine;

namespace Duo1JFramework.World
{
    /// <summary>
    /// 世界场景控制器基类
    /// </summary>
    public abstract class BaseWorldController : BaseMono
    {
        /// <summary>
        /// 世界初始化事件
        /// </summary>
        public event Action<BaseWorldController> OnInited;

        /// <summary>
        /// 世界销毁事件
        /// </summary>
        public event Action<BaseWorldController> OnDestroyed;

        /// <summary>
        /// 世界暂停事件
        /// </summary>
        public event Action<BaseWorldController> OnPaused;

        /// <summary>
        /// 世界恢复事件
        /// </summary>
        public event Action<BaseWorldController> OnResumed;

        /// <summary>
        /// 世界场景数据
        /// </summary>
        public WorldData WorldData { get; private set; }

        /// <summary>
        /// 是否暂停
        /// </summary>
        public bool Paused { get; private set; }

        /// <summary>
        /// 世界场景预制体
        /// </summary>
        public GameObject AssetGo
        {
            get
            {
                if (asset == null)
                {
                    asset = gameObject;
                }
                return asset;
            }
            private set
            {
                asset = value;
            }
        }
        private GameObject asset;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(WorldData worldData)
        {
            WorldData = worldData;
            Paused = false;

            NotifyWorldEnter();
            OnSubInit();
            OnInited?.Invoke(this);
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            if (Paused)
            {
                return;
            }

            Paused = true;
            NotifyWorldPause();
            OnSubPause();
            OnPaused?.Invoke(this);
        }

        /// <summary>
        /// 恢复
        /// </summary>
        public void Resume()
        {
            if (!Paused)
            {
                return;
            }

            Paused = false;
            NotifyWorldResume();
            OnSubResume();
            OnResumed?.Invoke(this);
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Destroy()
        {
            NotifyWorldExit();
            OnSubDestroy();
            OnDestroyed?.Invoke(this);

            AssetGo?.DestroySmart();
            AssetGo = null;
            WorldData = null;
        }

        /// <summary>
        /// 子类初始化
        /// </summary>
        public abstract void OnSubInit();

        /// <summary>
        /// 子类销毁
        /// </summary>
        public abstract void OnSubDestroy();

        /// <summary>
        /// 子类暂停
        /// </summary>
        public virtual void OnSubPause()
        {
        }

        /// <summary>
        /// 子类恢复
        /// </summary>
        public virtual void OnSubResume()
        {
        }

        private void NotifyWorldEnter()
        {
            BaseWorldItem[] items = GetComponentsInChildren<BaseWorldItem>(true);
            foreach (BaseWorldItem item in items)
            {
                item.OnWorldEnter(this);
            }
        }

        private void NotifyWorldExit()
        {
            BaseWorldItem[] items = GetComponentsInChildren<BaseWorldItem>(true);
            foreach (BaseWorldItem item in items)
            {
                item.OnWorldExit(this);
            }
        }

        private void NotifyWorldPause()
        {
            BaseWorldItem[] items = GetComponentsInChildren<BaseWorldItem>(true);
            foreach (BaseWorldItem item in items)
            {
                item.OnWorldPause(this);
            }
        }

        private void NotifyWorldResume()
        {
            BaseWorldItem[] items = GetComponentsInChildren<BaseWorldItem>(true);
            foreach (BaseWorldItem item in items)
            {
                item.OnWorldResume(this);
            }
        }
    }
}