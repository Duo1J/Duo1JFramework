using Duo1JFramework.ObjectPool;
using System;
using System.Diagnostics;
using System.Text;

using Debug = UnityEngine.Debug;

namespace Duo1JFramework
{
    /// <summary>
    /// 日志
    /// </summary>
    public static class Log
    {
        private static eLogLevel logLevel = eLogLevel.All;
        public static StringBuilder sb = new StringBuilder();

        /// <summary>
        /// 消息
        /// </summary>
        public static void Info(params object[] msg)
        {
            if (CheckLogLevelOpen(eLogLevel.Info))
            {
                Debug.Log(Concat("[Info] ", msg));
            }
        }

        /// <summary>
        /// 警告
        /// </summary>
        public static void Warn(params object[] msg)
        {
            if (CheckLogLevelOpen(eLogLevel.Warn))
            {
                Debug.LogWarning(Concat("[Warn] ", msg));
            }
        }

        /// <summary>
        /// 错误
        /// </summary>
        public static void Error(params object[] msg)
        {
            if (CheckLogLevelOpen(eLogLevel.Error))
            {
                Debug.LogError(Concat("[Error] ", msg));
            }
        }

        /// <summary>
        /// 异常打印
        /// </summary>
        public static void Exception(Exception e, params object[] msg)
        {
#if UNITY_EDITOR
            Debug.LogError(Concat("<color=red>[Exception]</color> ", msg) + $"\n<color=yellow>[ExceptionInfo]</color>{e.Message}");
#else
            Debug.LogError(Concat("[Exception] ", msg) + $"\n[ExceptionInfo]{e.Message}\n{e.StackTrace}");
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
        public static void LevelInfo(eLogLevel _logLevel, params object[] msg)
        {
            if (CheckLogLevelOpen(_logLevel))
            {
                Debug.Log(Concat($"<color=green>[{_logLevel}]</color> ", msg));
            }
        }

        /// <summary>
        /// 直接按等级打印警告
        /// </summary>
        public static void LevelWarn(eLogLevel _logLevel, params object[] msg)
        {
            if (CheckLogLevelOpen(_logLevel))
            {
                Debug.LogWarning(Concat($"<color=green>[{_logLevel}]</color> ", msg));
            }
        }

        /// <summary>
        /// 直接按等级打印错误
        /// </summary>
        public static void LevelError(eLogLevel _logLevel, params object[] msg)
        {
            if (CheckLogLevelOpen(_logLevel))
            {
                Debug.LogError(Concat($"<color=green>[{_logLevel}]</color> ", msg));
            }
        }

        /// <summary>
        /// 强制打印错误
        /// </summary>
        public static void ErrorForce(params object[] msg)
        {
            Debug.LogError(Concat("[Error] ", msg));
        }

        /// <summary>
        /// 设置打印等级
        /// </summary>
        public static void SetLogLevel(eLogLevel _logLevel)
        {
            logLevel = _logLevel;
        }

        /// <summary>
        /// 设置打印等级
        /// </summary>
        public static void SetLogLevel(eLogLevel _logLevel, bool isOpen)
        {
            if (_logLevel == eLogLevel.None)
            {
                if (isOpen)
                {
                    logLevel = eLogLevel.None;
                }
                else
                {
                    logLevel = eLogLevel.All;
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
        public static bool CheckLogLevelOpen(eLogLevel _logLevel)
        {
            if ((logLevel & eLogLevel.All) > 0)
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

        public static string Concat(params string[] msg)
        {
            return Concat(null, msg);
        }

        /// <summary>
        /// 获取当前调用栈
        /// </summary>
        public static string GetStackTrace()
        {
            string ret = null;

            StackTrace strackTrace = new StackTrace();
            Pool.StringBuilderPool.Using((sb) =>
            {
                foreach (StackFrame frame in strackTrace.GetFrames())
                {
                    sb.AppendLine($"{frame.GetMethod()} - {frame.GetFileColumnNumber()}");
                }
                ret = sb.ToString();
            });

            return ret ?? string.Empty;
        }
    }
}