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

                LoadWindowAsset(wnd, () =>
                {
                    Log.Info($"打开窗口`{typeof(Window).FullName}`");
                });
            }
            catch (CommonException e)
            {
                Log.Error($"打开窗口`{typeof(Window).FullName}`失败");
                return null;
            }


            wndList.Add(wnd);
            return wnd;
        }

        public void CloseWindow(Window wnd)
        {
            wndList.Remove(wnd);
        }

        public Window GetWindow(Type wndType)
        {
            foreach (Window wnd in wndList)
            {
                if (typeof(Window) == wndType)
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

            if (cfg.sync)
            {
                GameObject uiGo = AssetManager.Instance.LoadSync<GameObject>(cfg.path);
                Assert.NotNull(uiGo, $"无法加载到窗口资源`{cfg.path}`");
                LoadWindowAssetPostProcess(wnd, uiGo);
                callback?.Invoke();
            }
            else
            {
                AssetManager.Instance.Load<GameObject>(cfg.path, (uiGo) =>
                {
                    Assert.NotNull(uiGo, $"无法加载到窗口资源`{cfg.path}`");
                    LoadWindowAssetPostProcess(wnd, uiGo);
                    callback?.Invoke();
                });
            }
        }

        /// <summary>
        /// 加载窗口资源后处理
        /// </summary>
        /// <param name="wnd"></param>
        /// <param name="uiGo"></param>
        private void LoadWindowAssetPostProcess(Window wnd, GameObject uiGo)
        {
            wnd.Go = uiGo;
            Root.UIRoot.AddToLayer(wnd);
            //TODO 层级调整
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