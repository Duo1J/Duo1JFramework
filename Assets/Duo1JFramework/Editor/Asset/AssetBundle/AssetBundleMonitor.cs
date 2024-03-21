using UnityEngine;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// AssetBundle监视器
    /// </summary>
    public class AssetBundleMonitor : EditorWindowBase<AssetBundleMonitor>
    {
        private Vector2 scrollPos;

        private void OnGUI()
        {
            if (!LU.IsPlayingHelpBox())
            {
                return;
            }

            LU.Scroll(ref scrollPos, () =>
            {
                ABManager.Instance.DrawEditorInfo();
            });
        }
    }
}