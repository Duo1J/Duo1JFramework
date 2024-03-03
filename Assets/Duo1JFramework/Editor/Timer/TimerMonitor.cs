using Duo1JFramework.TimerUpdate;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// ¼ÆÊ±Æ÷¼àÊÓ
    /// </summary>
    public class TimerMonitor : EditorWindowBase<TimerMonitor>
    {
        private Vector2 scrollPos;

        private void OnGUI()
        {
            if (!LU.IsPlayingTip())
            {
                return;
            }

            LU.Scroll(ref scrollPos, () =>
            {
                ShowActiveTimer();
            });
        }

        private void ShowActiveTimer()
        {
            LU.Vertical(() =>
            {
                foreach (Timer timer in TimerManager.Instance.TimerSet)
                {
                    ShowTimerInfo(timer);
                }
            });
        }

        private void ShowTimerInfo(Timer timer)
        {
            timer.GetTimerMonitorInfo();
        }
    }
}
