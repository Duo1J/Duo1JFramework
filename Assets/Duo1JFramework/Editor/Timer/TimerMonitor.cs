using Duo1JFramework.TimerUpdate;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 计时器监视
    /// </summary>
    public class TimerMonitor : EditorWindowBase<TimerMonitor>
    {
        private Vector2 scrollPos;

        private void OnGUI()
        {
            if (!LU.IsPlayingTip_Editor())
            {
                return;
            }

            LU.Scroll(ref scrollPos, () =>
            {
                DrawActiveTimer();
                GUILayout.Space(10);
                DrawRemoveTimer();
            });
        }

        private void DrawActiveTimer()
        {
            GUILayout.Label("计时器列表");

            foreach (Timer timer in TimerManager.Instance.TimerSet)
            {
                LU.Vertical(() =>
                {
                    timer.Draw();
                }, "box");
            }
        }

        private void DrawRemoveTimer()
        {
            GUILayout.Label("待移除计时器列表");

            foreach (Timer timer in TimerManager.Instance.RemoveSet)
            {
                LU.Vertical(() =>
                {
                    timer.Draw();
                }, "box");
            }
        }
    }
}
