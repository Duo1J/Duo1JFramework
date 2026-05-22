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
            RichText = true;

            if (!ED.IsPlayingHelpBox())
            {
                return;
            }

            ED.Toggle(ref update, "每帧更新");

            ED.Scroll(ref scrollPos, () =>
            {
                GUILayout.Label("<color=yellow><size=14>世界管理器</size></color>");
                ED.Vertical(() =>
                {
                    WorldManager.Instance.DrawEditorInfo();
                }, "box");

                GUILayout.Space(10);

                GUILayout.Label("<color=aqua><size=14>世界四叉树管理器</size></color>");
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
