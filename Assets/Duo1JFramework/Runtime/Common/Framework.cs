using System;

namespace Duo1JFramework
{
    public static class Framework
    {
        public static void Init()
        {
            Log.Info($"{Def.FRAME_WORK_NAME} 初始化开始");

            try
            {
                Game.TriggerSingleton();

                Log.Info($"{Def.FRAME_WORK_NAME} 初始化成功");
            }
            catch (Exception e)
            {
                Log.Info($"{Def.FRAME_WORK_NAME} 初始化异常");
                Assert.ExceptHandle(e);
            }
        }
    }
}