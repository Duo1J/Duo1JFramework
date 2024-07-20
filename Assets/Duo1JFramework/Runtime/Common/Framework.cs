using Duo1JFramework.Asset;
using System;

namespace Duo1JFramework
{
    public class Framework : IDisposable
    {
        /// <summary>
        /// 框架是否已初始化
        /// </summary>
        public static bool Initialized { get; private set; } = false;

        /// <summary>
        /// 内存清理
        /// </summary>
        public static void GC()
        {
            AssetManager.Instance.GC();
            System.GC.Collect();

            Log.Info("GC调用");
        }

        /// <summary>
        /// 框架初始化
        /// </summary>
        public static void Init()
        {
            Log4Net.Init();

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
        /// 框架关闭
        /// </summary>
        public static void Shutdown()
        {
            if (!Initialized)
            {
                return;
            }

            Log.Info($"{Def.FRAME_WORK_NAME} 关闭");
            Initialized = false;

            GC();
            ShutdownInner();
            Log4Net.Shutdown();
        }

        public void Dispose()
        {
            Shutdown();
        }

        #region Inner

        private Framework()
        {
        }

        private static void InitInner()
        {
            SingletonTrigger.Trigger();
        }

        private static void ShutdownInner()
        {
            SingletonTrigger.Shutdown();
        }

        #endregion Inner
    }
}