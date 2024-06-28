using UnityEngine;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// AssetBundle监视器
    /// </summary>
    public class AssetBundleMonitor : BaseEditorWindow<AssetBundleMonitor>
    {
        private Vector2 scrollPos;

        private bool update;

        private void OnGUI()
        {
            if (!LU.IsPlayingHelpBox())
            {
                return;
            }

            LU.Toggle(ref update, "每帧更新");

            LU.Scroll(ref scrollPos, () =>
            {
                ABManager.Instance.DrawEditorInfo();
            });
        }

        private void OnInspectorUpdate()
        {
            if (update)
            {
                Repaint();
            }
        }
    }
}