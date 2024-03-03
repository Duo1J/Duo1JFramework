using System;
using System.Text;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 日志
    /// </summary>
    public static class Log
    {
        private static LogLevel logLevel = LogLevel.All;
        public static StringBuilder sb = new StringBuilder();

        /// <summary>
        /// 消息
        /// </summary>
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
            Debug.LogError(Concat("<color=red>[Exception]</color> ", msg) + $"{e.Message}\n{e.StackTrace}");
        }

        /// <summary>
        /// 编辑器消息
        /// </summary>
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
        public static void EditorWarn(params object[] msg)
        {
            if (Game.IsEditor)
            {
                Warn(msg);
            }
        }

        /// <summary>
        /// 编辑器错误
        /// </summary>
        public static void EditorError(params object[] msg)
        {
            if (Game.IsEditor)
            {
                Error(msg);
            }
        }

        /// <summary>
        /// 直接按等级打印消息
        /// </summary>
        public static void Level(LogLevel _logLevel, params object[] msg)
        {
            if (CheckLogLevelOpen(_logLevel))
            {
                Debug.Log(Concat($"<color=green>[{_logLevel}]</color> ", msg));
            }
        }

        /// <summary>
        /// 直接按等级打印警告
        /// </summary>
        public static void LevelWarn(LogLevel _logLevel, params object[] msg)
        {
            if (CheckLogLevelOpen(_logLevel))
            {
                Debug.LogWarning(Concat($"<color=green>[{_logLevel}]</color> ", msg));
            }
        }

        /// <summary>
        /// 直接按等级打印错误
        /// </summary>
        public static void LevelError(LogLevel _logLevel, params object[] msg)
        {
            if (CheckLogLevelOpen(_logLevel))
            {
                Debug.LogError(Concat($"<color=green>[{_logLevel}]</color> ", msg));
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
            if (_logLevel == LogLevel.None)
            {
                if (isOpen)
                {
                    logLevel = LogLevel.None;
                }
                else
                {
                    logLevel = LogLevel.All;
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
}