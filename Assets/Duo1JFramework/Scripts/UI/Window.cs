using Duo1JFramework.Event;
using Duo1JFramework.TimerUpdate;
using System;
using System.Collections.Generic;
using UnityEngine;

using UObject = UnityEngine.Object;

namespace Duo1JFramework.UI
{
    public abstract class Window
    {
        #region Field

        /// <summary>
        /// UI物体
        /// </summary>
        public GameObject Go
        {
            get => go;
            set
            {
                go = value;
                Controller = go.GetAndAssertComponent<UIController>($"窗口`{GetType().FullName}`未包含UIController组件");
            }
        }
        private GameObject go;

        /// <summary>
        /// UI配置
        /// </summary>
        public UIConfig Config
        {
            get
            {
                if (config == null)
                {
                    config = CreateUIConfig();
                }
                return config;
            }
        }
        private UIConfig config;

        /// <summary>
        /// UI控制器
        /// </summary>
        public UIController Controller { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Layer
        {
            get => layer;
            set
            {
                int parLayer = 0;
                Canvas parCanvas = Go.GetComponentInParent<Canvas>();
                if (parCanvas != null)
                {
                    parLayer = parCanvas.sortingOrder;
                }
                layer = parLayer + value;
                Controller.UpdateLayer(layer);
            }
        }
        private int layer;

        /// <summary>
        /// UpdateManager注册的更新回调
        /// </summary>
        private Action updater;

        /// <summary>
        /// 计时器列表
        /// </summary>
        private List<Timer> timerList;

        /// <summary>
        /// 事件列表
        /// </summary>
        private Dictionary<eEvent, List<Action<object>>> eventDict;

        private bool init = false;
        private bool dispose = false;

        #endregion Field

        #region Public

        /// <summary>
        /// 设置父节点
        /// </summary>
        public void SetParent(Transform par)
        {
            Assert.NotNull(par, "参数par为空");
            if (Go == null)
            {
                Log.Error($"窗口`{GetType().FullName}`未加载资源，无法设置父节点");
                return;
            }
            Go.transform.SetParent(par);
            RectTransform rectTf = Go.GetComponent<RectTransform>();
            if (rectTf != null)
            {
                rectTf.ExpandAnchor();
                rectTf.ResetSRT();
                rectTf.sizeDelta = Vector2.zero;
            }
        }

        /// <summary>
        /// 获取Go
        /// </summary>
        public GameObject GetGo(string goName)
        {
            return Controller.GetGo(goName);
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        public T GetCom<T>(string goName) where T : MonoBehaviour
        {
            return Controller.GetCom<T>(goName);
        }

        #endregion Public

        #region Protected

        #region Update

        protected void RegisterUpdate(Action _updater)
        {
            UpdateManager.Instance.Register(Go, _updater);
            updater = _updater;
        }

        protected void UnRegisterUpdate()
        {
            if (updater == null) return;
            UpdateManager.Instance.UnRegister(Go, updater);
            updater = null;
        }

        #endregion Update

        #region Timer

        /// <summary>
        /// 获取一个计时器
        /// </summary>
        protected Timer GetTimer(float interval, Action callback, int repeat = 1)
        {
            Timer timer = TimerManager.Instance.GetTimer(interval, callback, repeat);
            timerList ??= new List<Timer>();
            timerList.Add(timer);
            return timer;
        }

        /// <summary>
        /// 获取一个帧计时器
        /// </summary>
        protected Timer GetFrameTimer(int frame, Action callback, int repeat = 1)
        {
            Timer timer = TimerManager.Instance.GetFrameTimer(frame, callback, repeat);
            timerList ??= new List<Timer>();
            timerList.Add(timer);
            return timer;
        }

        /// <summary>
        /// 停止计时器
        /// </summary>
        protected void StopTimer(Timer timer)
        {
            timer.Stop();
            if (timerList == null) return;
            timerList.Remove(timer);
        }

        /// <summary>
        /// 停止所有计时器
        /// </summary>
        protected void StopAllTimer()
        {
            if (timerList == null) return;
            foreach (Timer timer in timerList)
            {
                timer.Stop();
            }
            timerList = null;
        }

        #endregion Timer

        #region Event

        /// <summary>
        /// 注册事件
        /// </summary>
        public void RegisterEvent(eEvent e, Action<object> callback)
        {
            eventDict ??= new Dictionary<eEvent, List<Action<object>>>();
            if (!eventDict.TryGetValue(e, out List<Action<object>> list))
            {
                list = new List<Action<object>>();
                eventDict.Add(e, list);
            }
            list.Add(callback);

            EventManager.Instance.Register(e, callback);
        }

        /// <summary>
        /// 取消注册事件
        /// </summary>
        public void UnRegisterEvent(eEvent e, Action<object> callback)
        {
            if (eventDict != null)
            {
                if (eventDict.TryGetValue(e, out List<Action<object>> list))
                {
                    list.Remove(callback);
                }
            }

            EventManager.Instance.UnRegister(e, callback);
        }

        /// <summary>
        /// 取消注册所有事件
        /// </summary>
        public void UnRegisterAllEvent()
        {
            if (eventDict == null) return;
            foreach (KeyValuePair<eEvent, List<Action<object>>> kv in eventDict)
            {
                foreach (Action<object> callback in kv.Value)
                {
                    EventManager.Instance.UnRegister(kv.Key, callback);
                }
            }
        }

        #endregion Event

        #endregion Protected

        #region Lifecycle

        /// <summary>
        /// 子类创建UI配置
        /// </summary>
        protected abstract UIConfig CreateUIConfig();

        /// <summary>
        /// 初始化
        /// </summary>
        public void OnInit()
        {
            if (init)
            {
                return;
            }
            init = true;
            OnInitInner();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void OnDispose()
        {
            if (dispose)
            {
                return;
            }
            dispose = true;

            OnDisposeInner();
            UnRegisterUpdate();
            StopAllTimer();
            UnRegisterAllEvent();

            if (Go != null)
            {
                UObject.DestroyImmediate(Go);
            }
        }

        /// <summary>
        /// 子类重写初始化
        /// </summary>
        protected abstract void OnInitInner();

        /// <summary>
        /// 子类重写销毁
        /// </summary>
        protected abstract void OnDisposeInner();

        #endregion Lifecycle
    }
}