using System;

namespace Duo1JFramework
{
    public static class Framework
    {
        /// <summary>
        /// 框架是否已初始化
        /// </summary>
        public static bool Initialized { get; private set; } = false;

        /// <summary>
        /// 框架初始化
        /// </summary>
        public static void Init()
        {
            if (Initialized)
            {
                Log.ErrorForce($"{Def.FRAME_WORK_NAME} 重复初始化");
                return;
            }
            Initialized = true;

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

        /// <summary>
        /// 内存清理
        /// </summary>
        public static void GC()
        {
            Game.GC();
        }

        private static void InitInner()
        {
            Game.TriggerSingleton();
        }
    }
}