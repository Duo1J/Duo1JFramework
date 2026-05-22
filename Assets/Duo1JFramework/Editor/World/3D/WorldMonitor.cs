using Duo1JFramework.World;
using UnityEngine;

namespace Duo1JFramework.World
{
    /// <summary>
    /// 世界监视器
    /// </summary>
    public class WorldMonitor : BaseEditorWindow<WorldMonitor>
    {
        private Vector2 scrollPos;

        private bool update = false;

        private void OnGUI()
        {
            if (!ED.IsPlayingHelpBox())
            {
                return;
            }

            ED.Toggle(ref update, "每帧更新");

            ED.Scroll(ref scrollPos, () =>
            {
                GUILayout.Label("世界管理器");
                ED.Vertical(() =>
                {
                    WorldManager.Instance.DrawEditorInfo();
                }, "box");

                GUILayout.Space(10);

                GUILayout.Label("世界四叉树管理器");
                ED.Vertical(() =>
                {
                    WorldQuadManager.Instance.DrawEditorInfo();
                }, "box");
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
