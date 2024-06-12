namespace Duo1JFramework
{
    public static class Framework
    {
        public static void Init()
        {
            Game.TriggerSingleton();

            Log.Info($"{Def.FRAME_WORK_NAME} initialization succeeded.");
        }
    }
}