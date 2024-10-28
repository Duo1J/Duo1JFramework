using Duo1JFramework.Asset;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI管理器
    /// </summary>
    public class UIManager : MonoSingleton<UIManager>
    {
        /// <summary>
        /// UI相机
        /// </summary>
        public Camera UICamera => UIRoot.UICamera;

        /// <summary>
        /// UI根Canvas
        /// </summary>
        public Canvas UICanvas => UIRoot.UICanvas;

        /// <summary>
        /// 窗口自增ID
        /// </summary>
        private AutoIncID incID;

        /// <summary>
        /// 窗口列表
        /// </summary>
        private List<Window> wndList;

        /// <summary>
        /// 打开窗口
        /// </summary>
        public T OpenWindow<T>() where T : Window, new()
        {
            Window wnd = OpenWindow(new T());
            T ret = wnd as T;
            Assert.NotNull(ret, $"Window转换 `{typeof(T).FullName}` 失败");

            return ret;
        }

        /// <summary>
        /// 打开窗口
        /// </summary>
        public Window OpenWindow(Window wnd)
        {
            try
            {
                Assert.NotNullArg(wnd, "wnd");

                Type wndType = wnd.GetType();
                if (IsWindowOpened(wndType))
                {
                    Log.Warn($"重复打开窗口`{wndType.FullName}`，执行回退到窗口`BackToWindow()`");
                    return BackToWindow(wndType);
                }

                wndList.Add(wnd);

                LoadWindowAsset(wnd, () =>
                {
                    Log.Info($"打开窗口`{wnd.GetType().FullName}`");
                });
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, $"打开窗口`{(wnd == null ? "NULL" : wnd.GetType().FullName)}`失败");
                if (wnd != null)
                {
                    CloseWindow(wnd);
                }
                return null;
            }

            return wnd;
        }

        /// <summary>
        /// 回退到窗口
        /// </summary>
        public Window BackToWindow<T>() where T : Window, new()
        {
            return BackToWindow(typeof(T));
        }

        /// <summary>
        /// 回退到窗口
        /// </summary>
        public Window BackToWindow(Type wndType)
        {
            Window tarWnd = GetWindow(wndType);
            if (tarWnd == null)
            {
                Log.ErrorForce($"未打开窗口`{wndType.FullName}`，无法回退");
                return null;
            }

            List<Window> removeList = null;
            foreach (Window wnd in wndList)
            {
                if (!wnd.IsConstLayer && wnd.ID < tarWnd.ID)
                {
                    if (removeList == null) removeList = new List<Window>();
                    removeList.Add(wnd);
                }
            }

            if (removeList != null)
            {
                foreach (Window wnd in removeList)
                {
                    CloseWindow(wnd);
                }
            }

            return tarWnd;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public bool CloseWindow<T>() where T : Window, new()
        {
            Window window = GetWindow<T>();
            if (window == null)
            {
                Log.ErrorForce($"未打开窗口`{typeof(T).FullName}`，无法关闭");
                return false;
            }

            return CloseWindow(window);
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public bool CloseWindow(Window wnd)
        {
            try
            {
                Assert.NotNullArg(wnd, "wnd");

                wnd.Dispose();
                bool ret = wndList.Remove(wnd);
                AdjustFullscreenStrategy();

                return ret;
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, $"关闭窗口`{(wnd == null ? "NULL" : wnd.GetType().FullName)}`失败");
                return false;
            }
        }

        /// <summary>
        /// 窗口是否已打开
        /// </summary>
        public bool IsWindowOpened<T>() where T : Window, new()
        {
            return IsWindowOpened(typeof(T));
        }

        /// <summary>
        /// 窗口是否已打开
        /// </summary>
        public bool IsWindowOpened(Type wndType)
        {
            return GetWindow(wndType) != null;
        }

        /// <summary>
        /// 获取窗口
        /// </summary>
        public Window GetWindow<T>() where T : Window, new()
        {
            return GetWindow(typeof(T));
        }

        /// <summary>
        /// 获取窗口
        /// </summary>
        public Window GetWindow(Type wndType)
        {
            foreach (Window wnd in wndList)
            {
                if (wnd.GetType() == wndType)
                {
                    return wnd;
                }
            }

            return null;
        }

        #region Inner

        /// <summary>
        /// 加载窗口资源
        /// </summary>
        private void LoadWindowAsset(Window wnd, Action callback)
        {
            UIData cfg = wnd.Config;
            Assert.NotNull(cfg, $"窗口 `{wnd.GetType().Name}` 配置为空");

            IAssetHandle<GameObject> handle = AssetManager.Instance.LoadByTypeSync<GameObject>(cfg.Path, cfg.LoadType);
            GameObject uiGo = null;
            if (handle != null)
            {
                uiGo = handle.Instantiate();
                handle.Release();
                handle = null;
            }

            Assert.NotNull(uiGo, $"无法加载到窗口资源 `{cfg.Path}`");
            LoadWindowAssetPostProcess(wnd, uiGo);
            callback?.Invoke();
        }

        /// <summary>
        /// 加载窗口资源后处理
        /// </summary>
        private void LoadWindowAssetPostProcess(Window wnd, GameObject uiGo)
        {
            wnd.Go = uiGo;
            Root.UIRoot.AddToLayer(wnd);
            AdjustWindowSortingOrder(wnd);
            AdjustFullscreenStrategy();
            wnd.Init();
        }

        /// <summary>
        /// 调整窗口层级
        /// </summary>
        private void AdjustWindowSortingOrder(Window wnd)
        {
            int maxSortingOrderOffset = 0;
            foreach (Window w in wndList)
            {
                if (w == wnd) continue;
                if (w.Layer != wnd.Layer) continue;

                int sortingOrderOffset = w.SortingOrderOffset;
                if (sortingOrderOffset > maxSortingOrderOffset) maxSortingOrderOffset = sortingOrderOffset;
            }
            wnd.SortingOrderOffset = maxSortingOrderOffset + Def.UI.STEP_LAYER;
        }

        /// <summary>
        /// 调整全屏策略
        /// </summary>
        private void AdjustFullscreenStrategy()
        {
            long maxFullscreenID = long.MinValue;
            foreach (Window wnd in wndList)
            {
                if (!wnd.IsConstLayer && wnd.Config.IsFullScreen && wnd.ID > maxFullscreenID)
                {
                    maxFullscreenID = wnd.ID;
                }
            }

            foreach (Window wnd in wndList)
            {
                if (!wnd.IsConstLayer && wnd.ID < maxFullscreenID)
                {
                    wnd.MoveToFar();
                }
                else
                {
                    wnd.ResetRectTransform();
                }
            }
        }

        /// <summary>
        /// 获取窗口自增ID
        /// </summary>
        public long GetIncID()
        {
            return incID.NewID;
        }

        protected override void OnInit()
        {
            incID = AutoIncID.Create();
            wndList = new List<Window>();
        }

        protected override void OnDispose()
        {
            wndList = null;
        }

        #endregion Inner
    }
}
