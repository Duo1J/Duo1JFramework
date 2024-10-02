using Duo1JFramework.Asset;
using Duo1JFramework.PlatformAPI;
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
            _Init();
        }

        /// <summary>
        /// 框架关闭
        /// </summary>
        public static void Shutdown()
        {
            _Shutdown();
        }

        /// <summary>
        /// 内存清理
        /// </summary>
        public static void GC()
        {
            System.GC.Collect();
            AssetManager.Instance.GC();

            Log.Info($"GC调用\n{DbgUtil.GetMemoryInfo()}");
        }

        #region Inner

        /// <summary>
        /// 打印Logo
        /// </summary>
        private static void PrintLogo()
        {
#if UNITY_EDITOR
            Log.Info($"<size=16><{Def.FRAME_WORK_NAME}></size>");
#else
            Log.Info($"<{Def.FRAME_WORK_NAME}>");
#endif
        }

        /// <summary>
        /// 内部初始化逻辑
        /// </summary>
        private static void InitInner()
        {
            Platform.Init();
            SingletonManager.TriggerInner();
        }

        /// <summary>
        /// 内部关闭逻辑
        /// </summary>
        private static void ShutdownInner()
        {
            SingletonManager.DisposeAll();
        }

        /// <summary>
        /// 框架初始化
        /// </summary>
        private static void _Init()
        {
            Log4Net.Init();

            if (Initialized)
            {
                Log.ErrorForce($"{Def.FRAME_WORK_NAME} 重复初始化");
                return;
            }
            Initialized = true;

            PrintLogo();

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
        private static void _Shutdown()
        {
            if (!Initialized)
            {
                return;
            }
            Initialized = false;

            Log.Info($"{Def.FRAME_WORK_NAME} 开始关闭");
            try
            {
                GC();
                ShutdownInner();

                Log.Info($"{Def.FRAME_WORK_NAME} 关闭成功");
            }
            catch (Exception e)
            {
                Log.ErrorForce($"{Def.FRAME_WORK_NAME} 关闭异常");
                Assert.ExceptHandle(e);
            }

            Log4Net.Shutdown();
        }

        #endregion Inner
    }
}