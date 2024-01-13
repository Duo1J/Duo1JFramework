using System;
using System.Text;
using UnityEngine;

namespace Duo1JFramework
{
    public static class Log
    {
        private static LogLevel logLevel = LogLevel.All;
        public static StringBuilder sb = new StringBuilder();

        /// <summary>
        /// log
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(params object[] msg)
        {
            if (CheckLogLevelOpen(LogLevel.Info))
            {
                Debug.Log(Concat("[Info] ", msg));
            }
        }

        /// <summary>
        /// 警告
        /// </summary>
        public static void Warn(params object[] msg)
        {
            if (CheckLogLevelOpen(LogLevel.Warn))
            {
                Debug.LogWarning(Concat("[Warn] ", msg));
            }
        }

        /// <summary>
        /// 错误
        /// </summary>
        public static void Error(params object[] msg)
        {
            if (CheckLogLevelOpen(LogLevel.Error))
            {
                Debug.LogError(Concat("[Error] ", msg));
            }
        }

        /// <summary>
        /// 异常打印
        /// </summary>
        public static void Exception(Exception e, params object[] msg)
        {
            Debug.LogError(Concat("[Exception] ", msg) + $"\n\t{e.Message}\n{e.StackTrace}");
        }

        /// <summary>
        /// 编辑器log
        /// </summary>
        /// <param name="msg"></param>
        public static void EditorInfo(params object[] msg)
        {
            if (Game.IsEditor)
            {
                Info(msg);
            }
        }

        /// <summary>
        /// 编辑器警告
        /// </summary>
        /// <param name="msg"></param>
        public static void EditorWarn(params object[] msg)
        {
            if (Game.IsEditor)
            {
                Warn(msg);
            }
        }

        /// <summary>
        /// 直接按等级打印
        /// </summary>
        public static void Level(LogLevel _logLevel, params object[] msg)
        {
            if (CheckLogLevelOpen(_logLevel))
            {
                Debug.Log(Concat($"[{_logLevel}] ", msg));
            }
        }

        /// <summary>
        /// 强制打印错误
        /// </summary>
        /// <param name="msg"></param>
        public static void ErrorForce(params object[] msg)
        {
            Debug.LogError(Concat("[Error] ", msg));
        }

        /// <summary>
        /// 设置打印等级
        /// </summary>
        public static void SetLogLevel(LogLevel _logLevel)
        {
            logLevel = _logLevel;
        }

        /// <summary>
        /// 设置打印等级
        /// </summary>
        public static void SetLogLevel(LogLevel _logLevel, bool isOpen)
        {
            if (isOpen)
            {
                if (_logLevel == LogLevel.None)
                {
                    logLevel = LogLevel.None;
                }
                else
                {
                    logLevel |= _logLevel;
                }
            }
            else
            {
                logLevel &= ~_logLevel;
            }
        }

        /// <summary>
        /// 检查打印等级是否开放
        /// </summary>
        public static bool CheckLogLevelOpen(LogLevel _logLevel)
        {
            if ((logLevel & LogLevel.All) > 0)
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
    }

    /// <summary>
    /// 日志打印等级
    /// </summary>
    public enum LogLevel
    {
        None = 0,
        Info = 1,
        Warn = 1 << 1,
        Error = 1 << 2,
        All = 1 << 30
    }
}