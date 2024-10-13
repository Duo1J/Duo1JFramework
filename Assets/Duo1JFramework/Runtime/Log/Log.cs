using System;
using System.Diagnostics;
using System.Text;

using UDebug = UnityEngine.Debug;

namespace Duo1JFramework
{
    /// <summary>
    /// 日志
    /// </summary>
    public class Log
    {
        /// <summary>
        /// 日志等级
        /// </summary>
        private static ELogLevel logLevel = ELogLevel.All;

        private static StringBuilder sb = new StringBuilder();

        public const string INFO_TAG = "[Info] ";
        public const string WARN_TAG = "[Warn] ";
        public const string ERROR_TAG = "[Error] ";

        public const string EXCEPT_TAG = "[Exception] ";
        public const string EXCEPT_INFO_TAG = "[ExceptionInfo] ";

        /// <summary>
        /// 消息
        /// </summary>
        public static void Info(params object[] msg)
        {
            if (CheckLogLevelOpen(ELogLevel.Info))
            {
                UDebug.Log(Concat(INFO_TAG, msg));
            }
        }

        /// <summary>
        /// 警告
        /// </summary>
        public static void Warn(params object[] msg)
        {
            if (CheckLogLevelOpen(ELogLevel.Warn))
            {
                UDebug.LogWarning(Concat(WARN_TAG, msg));
            }
        }

        /// <summary>
        /// 错误
        /// </summary>
        public static void Error(params object[] msg)
        {
            if (CheckLogLevelOpen(ELogLevel.Error))
            {
                UDebug.LogError(Concat(ERROR_TAG, msg));
            }
        }

        /// <summary>
        /// 强制打印错误
        /// </summary>
        public static void ErrorForce(params object[] msg)
        {
            UDebug.LogError(Concat(ERROR_TAG, msg));
        }

        /// <summary>
        /// 异常打印
        /// </summary>
        public static void Exception(Exception e, params object[] msg)
        {
#if UNITY_EDITOR
            UDebug.LogError(Concat($"<color=red>{EXCEPT_TAG}</color>", msg) + $"\n<color=yellow>{EXCEPT_INFO_TAG}</color>{e.Message}\n\n{e.StackTrace}\n");
#else
            UDebug.LogError(Concat(EXCEPT_TAG, msg) + $"\n{EXCEPT_INFO_TAG}{e.Message}\n\n{e.StackTrace}\n");
#endif
        }

        /// <summary>
        /// 编辑器消息
        /// </summary>
        [Conditional("UNITY_EDITOR")]
        public static void EditorInfo(params object[] msg)
        {
            Info(msg);
        }

        /// <summary>
        /// 编辑器警告
        /// </summary>
        [Conditional("UNITY_EDITOR")]
        public static void EditorWarn(params object[] msg)
        {
            Warn(msg);
        }

        /// <summary>
        /// 编辑器错误
        /// </summary>
        [Conditional("UNITY_EDITOR")]
        public static void EditorError(params object[] msg)
        {
            Error(msg);
        }

        /// <summary>
        /// 直接按等级打印消息
        /// </summary>
        public static void LevelInfo(ELogLevel _logLevel, params object[] msg)
        {
            if (CheckLogLevelOpen(_logLevel))
            {
#if UNITY_EDITOR
                UDebug.Log(Concat($"<color=green>[{_logLevel.GetName()}]</color> ", msg));
#else
                UDebug.Log(Concat($"[{_logLevel.GetName()}] ", msg));
#endif
            }
        }

        /// <summary>
        /// 直接按等级打印警告
        /// </summary>
        public static void LevelWarn(ELogLevel _logLevel, params object[] msg)
        {
            if (CheckLogLevelOpen(_logLevel))
            {
#if UNITY_EDITOR
                UDebug.LogWarning(Concat($"<color=green>[{_logLevel.GetName()}]</color> ", msg));
#else
                UDebug.LogWarning(Concat($"[{_logLevel.GetName()}] ", msg));
#endif
            }
        }

        /// <summary>
        /// 直接按等级打印错误
        /// </summary>
        public static void LevelError(ELogLevel _logLevel, params object[] msg)
        {
            if (CheckLogLevelOpen(_logLevel))
            {
#if UNITY_EDITOR
                UDebug.LogError(Concat($"<color=green>[{_logLevel.GetName()}]</color> ", msg));
#else
                UDebug.LogError(Concat($"[{_logLevel.GetName()}] ", msg));
#endif
            }
        }

        /// <summary>
        /// 设置打印等级
        /// </summary>
        public static void SetLogLevel(ELogLevel _logLevel)
        {
            logLevel = _logLevel;
        }

        /// <summary>
        /// 设置打印等级
        /// </summary>
        public static void SetLogLevel(ELogLevel _logLevel, bool isOpen)
        {
            if (_logLevel == ELogLevel.None)
            {
                if (isOpen)
                {
                    logLevel = ELogLevel.None;
                }
                else
                {
                    logLevel = ELogLevel.All;
                }
                return;
            }

            if (isOpen)
            {
                logLevel |= _logLevel;
            }
            else
            {
                logLevel &= ~_logLevel;
            }
        }

        /// <summary>
        /// 检查打印等级是否开放
        /// </summary>
        public static bool CheckLogLevelOpen(ELogLevel _logLevel)
        {
            if ((logLevel & ELogLevel.All) > 0)
            {
                return true;
            }
            return (logLevel & _logLevel) > 0;
        }

        /// <summary>
        /// 将字符串数组以逗号拼接
        /// </summary>
        public static string Concat(string tag, params object[] msg)
        {
            sb.Clear();

            bool appended = false;
            if (tag != null)
            {
                sb.Append(tag);
            }

            for (int i = 0; i < msg.Length; i++)
            {
                if (!appended)
                {
                    appended = true;
                    sb.Append(msg[i]);
                }
                else
                {
                    sb.AppendFormat(", {0}", msg[i]);
                }
            }
            return sb.ToString();
        }

        private Log()
        {
        }
    }
}
