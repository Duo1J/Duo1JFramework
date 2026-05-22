using UnityEngine;

namespace Duo1JFramework.PlatformAPI
{
    /// <summary>
    /// 平台
    /// </summary>
    public class Platform
    {
        /// <summary>
        /// 当前平台接口
        /// </summary>
        public static IPlatform Current
        {
            get
            {
                if (current == null)
                {
                    Init();
                }

                return current;
            }
        }
        private static IPlatform current;

        /// <summary>
        /// 当前平台类型
        /// </summary>
        public static EPlatform Type => Current.Type;

        /// <summary>
        /// 当前运行平台类型
        /// </summary>
        public static EPlatform RuntimeType => Current.RuntimeType;

        /// <summary>
        /// 是否编辑器平台
        /// </summary>
        public static bool IsEditor => Current.IsEditor;

        /// <summary>
        /// 是否PC平台
        /// </summary>
        public static bool IsPC => Current.IsPC;

        /// <summary>
        /// 是否桌面平台
        /// </summary>
        public static bool IsDesktop => Current.IsDesktop;

        /// <summary>
        /// 是否移动平台
        /// </summary>
        public static bool IsMobile => Current.IsMobile;

        /// <summary>
        /// 平台设备信息
        /// </summary>
        public static PlatformDeviceInfo DeviceInfo => Current.DeviceInfo;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            if (current != null)
            {
                return;
            }

#if UNITY_EDITOR
            current = new EditorPlatform();
#elif UNITY_STANDALONE_WIN
            current = new PCPlatform();
#elif UNITY_STANDALONE_OSX
            current = new PCPlatform();
#elif UNITY_STANDALONE_LINUX
            current = new PCPlatform();
#elif UNITY_ANDROID
            current = new AndroidPlatform();
#elif UNITY_IOS
            current = new IOSPlatform();
#elif UNITY_WEBGL
            current = new DefaultPlatform();
#elif UNITY_SERVER
            current = new DefaultPlatform();
#else
            current = new DefaultPlatform();
#endif

            Log.Info($"初始化平台`{current.GetType().Name}` Type: {current.Type}, RuntimeType: {current.RuntimeType}, OS: {SystemInfo.operatingSystem}, Device: {SystemInfo.deviceModel}, Memory: {SystemInfo.systemMemorySize}MB");
        }

        /// <summary>
        /// 重置平台
        /// </summary>
        public static void Reset()
        {
            current = null;
        }

        private Platform()
        {
        }
    }
}