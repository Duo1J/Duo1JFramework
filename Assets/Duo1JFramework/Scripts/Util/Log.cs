using System.Text;
using UnityEngine;

namespace Duo1JFramework
{
    public static class Log
    {
        private static LogLevel logLevel = LogLevel.All;
        public static StringBuilder sb = new StringBuilder();

        public static void Info(params string[] msg)
        {
            if (CheckLogLevelOpen(LogLevel.Info))
            {
                Debug.Log(Concat("[Info] ", msg));
            }
        }

        public static void Warn(params string[] msg)
        {
            if (CheckLogLevelOpen(LogLevel.Warn))
            {
                Debug.LogWarning(Concat("[Warn] ", msg));
            }
        }

        public static void Error(params string[] msg)
        {
            if (CheckLogLevelOpen(LogLevel.Error))
            {
                Debug.LogError(Concat("[Error] ", msg));
            }
        }

        public static void EditorInfo(params string[] msg)
        {
            if (Game.IsEditor)
            {
                Info(msg);
            }
        }

        public static void EditorWarn(params string[] msg)
        {
            if (Game.IsEditor)
            {
                Warn(msg);
            }
        }

        public static void Level(LogLevel _logLevel, params string[] msg)
        {
            if (CheckLogLevelOpen(_logLevel))
            {
                Debug.Log(Concat($"[{_logLevel}] ", msg));
            }
        }

        public static void SetLogLevel(LogLevel _logLevel)
        {
            logLevel = _logLevel;
        }

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
        public static string Concat(string tag, params string[] msg)
        {
            sb.Clear();

            bool appended = false;
            if (tag != null)
            {
                appended = true;
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

    public enum LogLevel
    {
        None = 0,
        Info = 1,
        Warn = 1 << 1,
        Error = 1 << 2,
        All = 1 << 30
    }
}