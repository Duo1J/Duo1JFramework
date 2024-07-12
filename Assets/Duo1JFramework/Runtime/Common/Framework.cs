using System;

namespace Duo1JFramework
{
    public static class Framework
    {
        private static bool init = false;

        /// <summary>
        /// 框架初始化
        /// </summary>
        public static void Init()
        {
            if (init)
            {
                Log.ErrorForce($"{Def.FRAME_WORK_NAME} 重复初始化");
                return;
            }
            init = true;

            Log.Info($"{Def.FRAME_WORK_NAME} 初始化开始");

            try
            {
                InitInner();

                Log.Info($"{Def.FRAME_WORK_NAME} 初始化成功");
            }
            catch (Exception e)
            {
                Log.ErrorForce($"{Def.FRAME_WORK_NAME} 初始化异常");
                Assert.ExceptHandle(e);
            }
        }

        private static void InitInner()
        {
            Game.TriggerSingleton();
        }
    }
}