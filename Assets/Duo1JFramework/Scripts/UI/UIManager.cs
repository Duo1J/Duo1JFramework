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
        private List<Window> wndList;

        public Window OpenWindow(Window wnd)
        {
            try
            {
                Assert.NotNull(wnd, "窗口对象为空");

                Window w = GetWindow(wnd.GetType());
                if (w != null)
                {
                    Log.Info($"重复打开窗口`{w.GetType().FullName}`");
                    return w;
                }

                LoadWindowAsset(wnd, () =>
                {
                    Log.Info($"打开窗口`{typeof(Window).FullName}`");
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


            wndList.Add(wnd);
            return wnd;
        }

        public bool CloseWindow(Window wnd)
        {
            try
            {
                Assert.NotNull(wnd, "窗口对象为空");
                wnd.Dispose();
                return wndList.Remove(wnd);
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, $"关闭窗口`{(wnd == null ? "NULL" : wnd.GetType().FullName)}`失败");
                return false;
            }
        }

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

        /// <summary>
        /// 加载窗口资源
        /// </summary>
        private void LoadWindowAsset(Window wnd, Action callback)
        {
            UIConfig cfg = wnd.Config;
            Assert.NotNull(cfg, $"窗口`{wnd.GetType().Name}`配置为空");

            if (cfg.Sync)
            {
                GameObject uiGo;
                if (cfg.IsResource)
                {
                    uiGo = AssetManager.Instance.LoadResourceInsSync<GameObject>(cfg.Path);
                }
                else
                {
                    uiGo = AssetManager.Instance.LoadInsSync<GameObject>(cfg.Path);
                }
                Assert.NotNull(uiGo, $"无法加载到窗口资源`{cfg.Path}`");
                LoadWindowAssetPostProcess(wnd, uiGo);
                callback?.Invoke();
            }
            else
            {
                if (cfg.IsResource)
                {
                    AssetManager.Instance.LoadResourceIns<GameObject>(cfg.Path, (uiGo) =>
                    {
                        Assert.NotNull(uiGo, $"无法加载到窗口资源`{cfg.Path}`");
                        LoadWindowAssetPostProcess(wnd, uiGo);
                        callback?.Invoke();
                    });
                }
                else
                {
                    AssetManager.Instance.LoadIns<GameObject>(cfg.Path, (uiGo) =>
                    {
                        Assert.NotNull(uiGo, $"无法加载到窗口资源`{cfg.Path}`");
                        LoadWindowAssetPostProcess(wnd, uiGo);
                        callback?.Invoke();
                    });
                }
            }
        }

        /// <summary>
        /// 加载窗口资源后处理
        /// </summary>
        private void LoadWindowAssetPostProcess(Window wnd, GameObject uiGo)
        {
            wnd.Go = uiGo;
            Root.Instance.UIRoot.AddToLayer(wnd);
            AdjustWindowLayer(wnd);
            wnd.Init();
        }

        /// <summary>
        /// 调整窗口层级
        /// </summary>
        private void AdjustWindowLayer(Window wnd)
        {
            int maxLayer = 0;
            foreach (Window w in wndList)
            {
                if (w == wnd) continue;
                if (w.Layer > maxLayer) maxLayer = w.Layer;
            }
            wnd.Layer = maxLayer + Def.UI_STEP_LAYER;
        }

        protected override void OnInit()
        {
            wndList = new List<Window>();
        }

        protected override void OnDispose()
        {
            wndList = null;
        }
    }
}