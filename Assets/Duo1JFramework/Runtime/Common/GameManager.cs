using Duo1JFramework.Event;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 游戏管理器
    /// </summary>
    public class GameManager : MonoSingleton<GameManager>
    {
        private void OnApplicationQuit()
        {
            //Log.Info("OnApplicationQuit");

            Game.IsQuit = true;

            EventManager.Instance.Broadcast(eEvent.APP_QUIT);

            Framework.Shutdown();
        }

        private void OnApplicationFocus(bool focus)
        {
            //Log.Info($"OnApplicationFocus: {focus}");

            if (focus)
                EventManager.Instance.Broadcast(eEvent.APP_FOCUS);
            else
                EventManager.Instance.Broadcast(eEvent.APP_UNFOCUS);
        }

        private void OnApplicationPause(bool pause)
        {
            //Log.Info($"OnApplicationFocus: {pause}");

            if (pause)
                EventManager.Instance.Broadcast(eEvent.APP_PAUSE);
            else
                EventManager.Instance.Broadcast(eEvent.APP_RESUME);
        }

        private void OnLowMemory()
        {
            Log.Info("LowMemory");
        }

        protected override void OnInit()
        {
            Application.lowMemory += OnLowMemory;
        }

        protected override void OnDispose()
        {
            Application.lowMemory -= OnLowMemory;
        }
    }
}