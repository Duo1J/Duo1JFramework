using System;
using UnityEngine;

namespace Duo1JFramework.PlatformAPI
{
    /// <summary>
    /// 平台接口
    /// </summary>
    public interface IPlatform
    {
        /// <summary>
        /// 平台类型枚举
        /// </summary>
        EPlatform Type { get; }

        /// <summary>
        /// 当前运行平台类型
        /// </summary>
        EPlatform RuntimeType { get; }

        /// <summary>
        /// 是否编辑器平台
        /// </summary>
        bool IsEditor { get; }

        /// <summary>
        /// 是否PC平台
        /// </summary>
        bool IsPC { get; }

        /// <summary>
        /// 是否桌面平台
        /// </summary>
        bool IsDesktop { get; }

        /// <summary>
        /// 是否移动平台
        /// </summary>
        bool IsMobile { get; }

        /// <summary>
        /// 平台设备信息
        /// </summary>
        PlatformDeviceInfo DeviceInfo { get; }

        #region Capability

        /// <summary>
        /// 是否支持平台能力
        /// </summary>
        bool HasCapability(EPlatformCapability capability);

        #endregion Capability

        #region Permission

        /// <summary>
        /// 检查平台权限
        /// </summary>
        EPlatformPermissionStatus CheckPermission(EPlatformPermission permission);

        /// <summary>
        /// 请求平台权限
        /// </summary>
        void RequestPermission(EPlatformPermission permission, Action<EPlatformPermissionStatus> callback = null);

        /// <summary>
        /// 打开平台权限设置
        /// </summary>
        bool OpenPermissionSettings();

        #endregion Permission

        #region SafeArea

        /// <summary>
        /// 获取安全区域
        /// </summary>
        Rect GetSafeArea();

        /// <summary>
        /// 获取屏幕尺寸
        /// </summary>
        Vector2Int GetScreenSize();

        /// <summary>
        /// 是否存在异形屏区域
        /// </summary>
        bool HasNotch();

        #endregion SafeArea

        #region Vibration

        /// <summary>
        /// 平台震动
        /// </summary>
        bool Vibrate();

        /// <summary>
        /// 平台震动
        /// </summary>
        bool Vibrate(long milliseconds);

        /// <summary>
        /// 轻触觉反馈
        /// </summary>
        bool LightImpact();

        /// <summary>
        /// 中触觉反馈
        /// </summary>
        bool MediumImpact();

        /// <summary>
        /// 重触觉反馈
        /// </summary>
        bool HeavyImpact();

        #endregion Vibration

        #region Clipboard

        /// <summary>
        /// 设置剪贴板文本
        /// </summary>
        bool SetClipboardText(string text);

        /// <summary>
        /// 获取剪贴板文本
        /// </summary>
        string GetClipboardText();

        /// <summary>
        /// 是否存在剪贴板文本
        /// </summary>
        bool HasClipboardText();

        #endregion Clipboard

        #region Dialog

        /// <summary>
        /// 显示短提示
        /// </summary>
        bool ShowToast(string message);

        /// <summary>
        /// 显示提示弹窗
        /// </summary>
        bool ShowAlert(string title, string message, Action callback = null);

        /// <summary>
        /// 显示确认弹窗
        /// </summary>
        bool ShowConfirm(string title, string message, Action<bool> callback = null);

        #endregion Dialog

        #region Performance

        /// <summary>
        /// 获取平台性能信息
        /// </summary>
        PlatformPerformanceInfo GetPerformanceInfo();

        #endregion Performance

        #region Network

        /// <summary>
        /// 当前网络状态
        /// </summary>
        NetworkReachability GetNetworkReachability();

        /// <summary>
        /// 是否连接Wifi或有线网络
        /// </summary>
        bool IsWifi();

        /// <summary>
        /// 是否连接移动网络
        /// </summary>
        bool IsCellular();

        /// <summary>
        /// 是否离线
        /// </summary>
        bool IsOffline();

        #endregion Network

        #region External

        /// <summary>
        /// 打开外部链接
        /// </summary>
        bool OpenURL(string url);

        #endregion External

        #region Notification

        /// <summary>
        /// 请求通知权限
        /// </summary>
        void RequestNotificationPermission(Action<EPlatformPermissionStatus> callback = null);

        /// <summary>
        /// 注册本地通知
        /// </summary>
        bool RegisterLocalNotification(string title, string message, DateTime fireTime);

        /// <summary>
        /// 取消所有本地通知
        /// </summary>
        bool CancelAllLocalNotifications();

        #endregion Notification

        #region Memory

        /// <summary>
        /// 获取总内存 (MB)
        /// </summary>
        int GetTotalMemory();

        /// <summary>
        /// 获取总内存 (MB)
        /// </summary>
        int GetTotalMemoryMB();

        /// <summary>
        /// 获取总保留内存 (B)
        /// </summary>
        long GetTotalReservedMemory();

        /// <summary>
        /// 获取总保留内存 (B)
        /// </summary>
        long GetTotalReservedMemoryBytes();

        /// <summary>
        /// 获取已使用堆内存 (B)
        /// </summary>
        long GetUsedHeapSize();

        /// <summary>
        /// 获取Mono已使用内存 (B)
        /// </summary>
        long GetMonoUsedSizeBytes();

        /// <summary>
        /// 获取总堆内存 (B)
        /// </summary>
        long GetTotalHeapSize();

        /// <summary>
        /// 获取Mono堆内存 (B)
        /// </summary>
        long GetMonoHeapSizeBytes();

        /// <summary>
        /// 获取已分配内存 (B)
        /// </summary>
        long GetAllocatedMemoryBytes();

        /// <summary>
        /// 获取显存大小 (MB)
        /// </summary>
        int GetGraphicsMemoryMB();

        #endregion Memory
    }
}