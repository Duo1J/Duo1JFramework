using Duo1JFramework.Asset;
using System;
using UnityEditor.Compilation;
using UnityEngine;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI管理器
    /// </summary>
    public class UIManager : MonoSingleton<UIManager>
    {
        public Window OpenWindow(Window wnd)
        {
            Assert.NotNull(wnd, "窗口对象为空");

            UIConfig cfg = wnd.GetUIConfig();
            Assert.NotNull(cfg, $"窗口`{wnd.GetType().Name}`配置为空");

            LoadWindowAsset(wnd, cfg, () =>
            {
            });

            return wnd;
        }

        public void CloseWindow(Window wnd)
        {
        }

        private void LoadWindowAsset(Window wnd, UIConfig cfg, Action callback)
        {
            if (cfg.sync)
            {
                GameObject uiGo = AssetManager.Instance.LoadSync<GameObject>(cfg.path);
                Assert.NotNull(uiGo, $"无法加载到窗口资源`{cfg.path}`");
                LoadWindowAssetFin(wnd, cfg, uiGo);
                callback?.Invoke();
            }
            else
            {
                AssetManager.Instance.Load<GameObject>(cfg.path, (uiGo) =>
                {
                    Assert.NotNull(uiGo, $"无法加载到窗口资源`{cfg.path}`");
                    LoadWindowAssetFin(wnd, cfg, uiGo);
                    callback?.Invoke();
                });
            }
        }

        private void LoadWindowAssetFin(Window wnd, UIConfig cfg, GameObject uiGo)
        {
            wnd.Go = uiGo;
        }

        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}