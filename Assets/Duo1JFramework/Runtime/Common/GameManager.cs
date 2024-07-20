using Duo1JFramework.Event;

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

        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}