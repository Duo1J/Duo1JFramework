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
                foreach (IEditorDrawer drawer in TimerManager.Instance.TimerSet)
                {
                    drawer.Draw();
                }
            });
        }
    }
}
