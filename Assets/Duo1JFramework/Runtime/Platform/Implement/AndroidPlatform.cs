using System;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace Duo1JFramework.PlatformAPI
{
    /// <summary>
    /// 安卓平台实现
    /// </summary>
    public class AndroidPlatform : BasePlatform
    {
        public override EPlatform Type => EPlatform.Android;

        /// <summary>
        /// 判断Android平台能力支持情况
        /// </summary>
        public override bool HasCapability(EPlatformCapability capability)
        {
            switch (capability)
            {
                case EPlatformCapability.Permission:
                case EPlatformCapability.Notification:
                    return true;
                default:
                    return base.HasCapability(capability);
            }
        }

        /// <summary>
        /// 检查Android权限状态
        /// </summary>
        public override EPlatformPermissionStatus CheckPermission(EPlatformPermission permission)
        {
#if UNITY_ANDROID
            string permissionName = ConvertPermission(permission);
            if (string.IsNullOrEmpty(permissionName))
            {
                return EPlatformPermissionStatus.NotSupported;
            }

            return Permission.HasUserAuthorizedPermission(permissionName) ? EPlatformPermissionStatus.Granted : EPlatformPermissionStatus.Denied;
#else
            return base.CheckPermission(permission);
#endif
        }

        /// <summary>
        /// 请求Android运行时权限
        /// </summary>
        public override void RequestPermission(EPlatformPermission permission, Action<EPlatformPermissionStatus> callback = null)
        {
#if UNITY_ANDROID
            string permissionName = ConvertPermission(permission);
            if (string.IsNullOrEmpty(permissionName))
            {
                callback?.Invoke(EPlatformPermissionStatus.NotSupported);
                return;
            }

            if (Permission.HasUserAuthorizedPermission(permissionName))
            {
                callback?.Invoke(EPlatformPermissionStatus.Granted);
                return;
            }

            PermissionCallbacks callbacks = new PermissionCallbacks();
            callbacks.PermissionGranted += _ => callback?.Invoke(EPlatformPermissionStatus.Granted);
            callbacks.PermissionDenied += _ => callback?.Invoke(EPlatformPermissionStatus.Denied);
            callbacks.PermissionDeniedAndDontAskAgain += _ => callback?.Invoke(EPlatformPermissionStatus.Denied);
            Permission.RequestUserPermission(permissionName, callbacks);
#else
            base.RequestPermission(permission, callback);
#endif
        }

        /// <summary>
        /// 触发Android指定时长震动
        /// </summary>
        public override bool Vibrate(long milliseconds)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator"))
                {
                    vibrator?.Call("vibrate", milliseconds);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Error($"Android震动失败: {e.Message}");
                return false;
            }
#else
            return base.Vibrate(milliseconds);
#endif
        }

        /// <summary>
        /// 打开Android应用详情设置页
        /// </summary>
        public override bool OpenPermissionSettings()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                using (AndroidJavaObject uri = new AndroidJavaClass("android.net.Uri").CallStatic<AndroidJavaObject>("parse", "package:" + Application.identifier))
                using (AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "android.settings.APPLICATION_DETAILS_SETTINGS", uri))
                {
                    currentActivity.Call("startActivity", intent);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Error($"打开Android设置失败: {e.Message}");
                return false;
            }
#else
            return base.OpenPermissionSettings();
#endif
        }

#if UNITY_ANDROID
        /// <summary>
        /// 转换为Android权限字符串
        /// </summary>
        private static string ConvertPermission(EPlatformPermission permission)
        {
            switch (permission)
            {
                case EPlatformPermission.Camera:
                    return Permission.Camera;
                case EPlatformPermission.Microphone:
                    return Permission.Microphone;
                case EPlatformPermission.Location:
                    return Permission.FineLocation;
                case EPlatformPermission.Storage:
                    return "android.permission.WRITE_EXTERNAL_STORAGE";
                default:
                    return string.Empty;
            }
        }
#endif
    }
}
