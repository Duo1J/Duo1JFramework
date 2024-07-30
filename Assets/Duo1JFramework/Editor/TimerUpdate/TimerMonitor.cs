using Duo1JFramework.TimerUpdate;
using UnityEngine;

namespace Duo1JFramework.TimerUpdate
{
    /// <summary>
    /// 计时器监视
    /// </summary>
    public class TimerMonitor : BaseEditorWindow<TimerMonitor>
    {
        private Vector2 scrollPos;

        private void OnGUI()
        {
            if (!ED.IsPlayingHelpBox())
            {
                return;
            }

            ED.Scroll(ref scrollPos, () =>
            {
                DrawActiveTimer();
                GUILayout.Space(10);
                DrawRemoveTimer();
            });
        }

        private void DrawActiveTimer()
        {
            GUILayout.Label("计时器列表");

            foreach (Timer timer in TimerManager.Instance.TimerSet_Editor)
            {
                ED.Vertical(() =>
                {
                    timer.DrawEditorInfo();
                }, "box");
            }
        }

        private void DrawRemoveTimer()
        {
            GUILayout.Label("待移除计时器列表");

            foreach (Timer timer in TimerManager.Instance.RemoveSet_Editor)
            {
                ED.Vertical(() =>
                {
                    timer.DrawEditorInfo();
                }, "box");
            }
        }
    }
}
