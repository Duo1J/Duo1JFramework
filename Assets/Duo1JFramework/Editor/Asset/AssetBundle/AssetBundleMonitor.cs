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
            if (!ED.IsPlayingHelpBox())
            {
                return;
            }

            ED.Toggle(ref update, "每帧更新");

            ED.Scroll(ref scrollPos, () =>
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