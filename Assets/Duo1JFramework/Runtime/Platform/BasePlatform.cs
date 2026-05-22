using System;
using UnityEngine;
using UnityEngine.Profiling;

namespace Duo1JFramework.PlatformAPI
{
    /// <summary>
    /// 平台基础实现
    /// </summary>
    public abstract class BasePlatform : IPlatform
    {
        /// <summary>
        /// 当前实现的平台类型
        /// </summary>
        public abstract EPlatform Type { get; }

        /// <summary>
        /// 转换Unity平台后的运行平台类型
        /// </summary>
        public virtual EPlatform RuntimeType => ConvertRuntimePlatform(Application.platform);

        /// <summary>
        /// 是否运行在编辑器环境
        /// </summary>
        public virtual bool IsEditor => Type == EPlatform.Editor;

        /// <summary>
        /// 是否属于PC平台
        /// </summary>
        public virtual bool IsPC => Type == EPlatform.PC || IsDesktop;

        /// <summary>
        /// 是否属于桌面平台
        /// </summary>
        public virtual bool IsDesktop => Type == EPlatform.Windows || Type == EPlatform.MacOS || Type == EPlatform.Linux || RuntimeType == EPlatform.Windows || RuntimeType == EPlatform.MacOS || RuntimeType == EPlatform.Linux;

        /// <summary>
        /// 是否属于移动平台
        /// </summary>
        public virtual bool IsMobile => Type == EPlatform.Android || Type == EPlatform.iOS;

        /// <summary>
        /// 获取设备基础信息
        /// </summary>
        public virtual PlatformDeviceInfo DeviceInfo => new PlatformDeviceInfo
        {
            DeviceName = SystemInfo.deviceName,
            DeviceModel = SystemInfo.deviceModel,
            DeviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier,
            OperatingSystem = SystemInfo.operatingSystem,
            ProcessorType = SystemInfo.processorType,
            ProcessorCount = SystemInfo.processorCount,
            GraphicsDeviceName = SystemInfo.graphicsDeviceName,
            GraphicsMemorySize = SystemInfo.graphicsMemorySize,
            SystemLanguage = Application.systemLanguage.ToString()
        };

        /// <summary>
        /// 判断平台是否支持指定能力
        /// </summary>
        public virtual bool HasCapability(EPlatformCapability capability)
        {
            switch (capability)
            {
                case EPlatformCapability.Clipboard:
                case EPlatformCapability.Keyboard:
                case EPlatformCapability.Mouse:
                case EPlatformCapability.Network:
                case EPlatformCapability.Battery:
                    return true;
                case EPlatformCapability.Touch:
                case EPlatformCapability.Gyroscope:
                    return Input.touchSupported;
                case EPlatformCapability.SafeArea:
                    return IsMobile;
                case EPlatformCapability.Vibration:
                    return IsMobile;
                case EPlatformCapability.Permission:
                case EPlatformCapability.Notification:
                case EPlatformCapability.FilePicker:
                case EPlatformCapability.NativeDialog:
                    return false;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 检查平台权限状态
        /// </summary>
        public virtual EPlatformPermissionStatus CheckPermission(EPlatformPermission permission)
        {
            return EPlatformPermissionStatus.NotSupported;
        }

        /// <summary>
        /// 请求平台权限
        /// </summary>
        public virtual void RequestPermission(EPlatformPermission permission, Action<EPlatformPermissionStatus> callback = null)
        {
            callback?.Invoke(CheckPermission(permission));
        }

        /// <summary>
        /// 打开权限设置页
        /// </summary>
        public virtual bool OpenPermissionSettings()
        {
            return false;
        }

        /// <summary>
        /// 获取屏幕安全区域
        /// </summary>
        public virtual Rect GetSafeArea()
        {
            return Screen.safeArea;
        }

        /// <summary>
        /// 获取屏幕尺寸
        /// </summary>
        public virtual Vector2Int GetScreenSize()
        {
            return new Vector2Int(Screen.width, Screen.height);
        }

        /// <summary>
        /// 判断是否存在异形屏区域
        /// </summary>
        public virtual bool HasNotch()
        {
            Rect safeArea = GetSafeArea();
            return safeArea.x > 0 || safeArea.y > 0 || safeArea.width < Screen.width || safeArea.height < Screen.height;
        }

        /// <summary>
        /// 触发默认震动
        /// </summary>
        public virtual bool Vibrate()
        {
            if (!HasCapability(EPlatformCapability.Vibration))
            {
                return false;
            }

            Handheld.Vibrate();
            return true;
        }

        /// <summary>
        /// 触发指定时长震动
        /// </summary>
        public virtual bool Vibrate(long milliseconds)
        {
            return Vibrate();
        }

        /// <summary>
        /// 触发轻触觉反馈
        /// </summary>
        public virtual bool LightImpact()
        {
            return Vibrate();
        }

        /// <summary>
        /// 触发中触觉反馈
        /// </summary>
        public virtual bool MediumImpact()
        {
            return Vibrate();
        }

        /// <summary>
        /// 触发重触觉反馈
        /// </summary>
        public virtual bool HeavyImpact()
        {
            return Vibrate();
        }

        /// <summary>
        /// 写入剪贴板文本
        /// </summary>
        public virtual bool SetClipboardText(string text)
        {
            GUIUtility.systemCopyBuffer = text ?? string.Empty;
            return true;
        }

        /// <summary>
        /// 读取剪贴板文本
        /// </summary>
        public virtual string GetClipboardText()
        {
            return GUIUtility.systemCopyBuffer ?? string.Empty;
        }

        /// <summary>
        /// 判断剪贴板是否有文本
        /// </summary>
        public virtual bool HasClipboardText()
        {
            return !string.IsNullOrEmpty(GetClipboardText());
        }

        /// <summary>
        /// 显示短提示
        /// </summary>
        public virtual bool ShowToast(string message)
        {
            Log.Info(message ?? string.Empty);
            return true;
        }

        /// <summary>
        /// 显示提示弹窗
        /// </summary>
        public virtual bool ShowAlert(string title, string message, Action callback = null)
        {
            Log.Info($"{title}\n{message}");
            callback?.Invoke();
            return true;
        }

        /// <summary>
        /// 显示确认弹窗
        /// </summary>
        public virtual bool ShowConfirm(string title, string message, Action<bool> callback = null)
        {
            Log.Info($"{title}\n{message}");
            callback?.Invoke(false);
            return true;
        }

        /// <summary>
        /// 获取平台性能信息
        /// </summary>
        public virtual PlatformPerformanceInfo GetPerformanceInfo()
        {
            return new PlatformPerformanceInfo
            {
                FPS = Time.unscaledDeltaTime > 0 ? 1f / Time.unscaledDeltaTime : 0f,
                TotalReservedMemoryBytes = GetTotalReservedMemoryBytes(),
                AllocatedMemoryBytes = GetAllocatedMemoryBytes(),
                MonoUsedMemoryBytes = GetMonoUsedSizeBytes(),
                MonoHeapMemoryBytes = GetMonoHeapSizeBytes(),
                BatteryLevel = SystemInfo.batteryLevel,
                BatteryStatus = SystemInfo.batteryStatus,
                NetworkReachability = GetNetworkReachability()
            };
        }

        /// <summary>
        /// 获取当前网络连接状态
        /// </summary>
        public virtual NetworkReachability GetNetworkReachability()
        {
            return Application.internetReachability;
        }

        /// <summary>
        /// 判断是否为局域网连接
        /// </summary>
        public virtual bool IsWifi()
        {
            return GetNetworkReachability() == NetworkReachability.ReachableViaLocalAreaNetwork;
        }

        /// <summary>
        /// 判断是否为移动网络连接
        /// </summary>
        public virtual bool IsCellular()
        {
            return GetNetworkReachability() == NetworkReachability.ReachableViaCarrierDataNetwork;
        }

        /// <summary>
        /// 判断是否离线
        /// </summary>
        public virtual bool IsOffline()
        {
            return GetNetworkReachability() == NetworkReachability.NotReachable;
        }

        /// <summary>
        /// 打开外部链接
        /// </summary>
        public virtual bool OpenURL(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            Log.Info($"Open URL: {url}");
            Application.OpenURL(url);
            return true;
        }

        /// <summary>
        /// 请求通知权限
        /// </summary>
        public virtual void RequestNotificationPermission(Action<EPlatformPermissionStatus> callback = null)
        {
            callback?.Invoke(EPlatformPermissionStatus.NotSupported);
        }

        /// <summary>
        /// 注册本地通知
        /// </summary>
        public virtual bool RegisterLocalNotification(string title, string message, DateTime fireTime)
        {
            return false;
        }

        /// <summary>
        /// 取消所有本地通知
        /// </summary>
        public virtual bool CancelAllLocalNotifications()
        {
            return false;
        }

        /// <summary>
        /// 获取系统总内存
        /// </summary>
        public virtual int GetTotalMemory()
        {
            return GetTotalMemoryMB();
        }

        /// <summary>
        /// 获取系统总内存MB
        /// </summary>
        public virtual int GetTotalMemoryMB()
        {
            return SystemInfo.systemMemorySize;
        }

        /// <summary>
        /// 获取总保留内存
        /// </summary>
        public virtual long GetTotalReservedMemory()
        {
            return GetTotalReservedMemoryBytes();
        }

        /// <summary>
        /// 获取总保留内存字节数
        /// </summary>
        public virtual long GetTotalReservedMemoryBytes()
        {
            return Profiler.GetTotalReservedMemoryLong();
        }

        /// <summary>
        /// 获取已使用堆内存
        /// </summary>
        public virtual long GetUsedHeapSize()
        {
            return GetMonoUsedSizeBytes();
        }

        /// <summary>
        /// 获取Mono已使用内存字节数
        /// </summary>
        public virtual long GetMonoUsedSizeBytes()
        {
            return Profiler.GetMonoUsedSizeLong();
        }

        /// <summary>
        /// 获取总堆内存
        /// </summary>
        public virtual long GetTotalHeapSize()
        {
            return GetMonoHeapSizeBytes();
        }

        /// <summary>
        /// 获取Mono堆内存字节数
        /// </summary>
        public virtual long GetMonoHeapSizeBytes()
        {
            return Profiler.GetMonoHeapSizeLong();
        }

        /// <summary>
        /// 获取已分配内存字节数
        /// </summary>
        public virtual long GetAllocatedMemoryBytes()
        {
            return Profiler.GetTotalAllocatedMemoryLong();
        }

        /// <summary>
        /// 获取显存大小MB
        /// </summary>
        public virtual int GetGraphicsMemoryMB()
        {
            return SystemInfo.graphicsMemorySize;
        }

        /// <summary>
        /// 转换Unity运行平台类型
        /// </summary>
        protected static EPlatform ConvertRuntimePlatform(RuntimePlatform platform)
        {
            switch (platform)
            {
                case UnityEngine.RuntimePlatform.OSXEditor:
                case UnityEngine.RuntimePlatform.WindowsEditor:
                case UnityEngine.RuntimePlatform.LinuxEditor:
                    return EPlatform.Editor;
                case UnityEngine.RuntimePlatform.WindowsPlayer:
                    return EPlatform.Windows;
                case UnityEngine.RuntimePlatform.OSXPlayer:
                    return EPlatform.MacOS;
                case UnityEngine.RuntimePlatform.LinuxPlayer:
                    return EPlatform.Linux;
                case UnityEngine.RuntimePlatform.Android:
                    return EPlatform.Android;
                case UnityEngine.RuntimePlatform.IPhonePlayer:
                    return EPlatform.iOS;
                case UnityEngine.RuntimePlatform.WebGLPlayer:
                    return EPlatform.WebGL;
                default:
                    return EPlatform.Default;
            }
        }
    }
}
