using System;
using System.IO;
using Duo1JFramework.Config;
using log4net;
using log4net.Config;
using UnityEngine;

namespace Duo1JFramework
{
    public static class Log4Net
    {
        private static ILog logger = GetLogger("D");
        public static ILog Logger => logger;

        public static ILog GetLogger(Type t)
        {
            return LogManager.GetLogger(t);
        }

        public static ILog GetLogger(string s)
        {
            return LogManager.GetLogger(s);
        }

        public static readonly string LOG_CONFIG_FOLDER = Path.Combine(Application.streamingAssetsPath, Def.FRAME_WORK_NAME);
        public static readonly string LOG_CONFIG_PATH = Path.Combine(LOG_CONFIG_FOLDER, "log4net.config");
        public static readonly string LOG_FOLDER_PATH = Path.Combine(LOG_CONFIG_FOLDER, "Log");
        public const string LOG_FILE_NAME = "log";

        private const string PROPKEY_FOLDER = "ApplicationLogPath";
        private const string PROPKEY_FILE = "LogFileName";

        private static bool initialized = false;

        public static void Init()
        {
#if UNITY_EDITOR
            if (!GameOption.Editor.useLog4Net)
            {
                return;
            }
#else
            if (!GameOption.Runtime.useLog4Net)
            {
                return;
            }
#endif

            if (initialized)
            {
                Log.ErrorForce("Log4Net 重复初始化");
                return;
            }

            try
            {
                CheckConfigFile();

                GlobalContext.Properties[PROPKEY_FOLDER] = LOG_FOLDER_PATH;
                GlobalContext.Properties[PROPKEY_FILE] = LOG_FILE_NAME;
                XmlConfigurator.ConfigureAndWatch(new FileInfo(LOG_CONFIG_PATH));

                Application.logMessageReceived += OnLogMessageReceived;

                initialized = true;
                Log.Info("Log4Net 初始化成功");
            }
            catch (Exception e)
            {
                initialized = false;
                Log.Info("Log4Net 初始化异常");
                Assert.ExceptHandle(e);
            }
        }

        public static void Shutdown()
        {
            LogManager.Shutdown();
        }

        private static void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Log:
                    logger.Info($"{condition}");
                    break;
                case LogType.Warning:
                    logger.Warn($"{condition}\n{stackTrace}");
                    break;
                case LogType.Error:
                    logger.Error($"{condition}\n{stackTrace}");
                    break;
                case LogType.Assert:
                    logger.Error($"{condition}\n{stackTrace}");
                    break;
                case LogType.Exception:
                    logger.Fatal($"{condition}\n{stackTrace}");
                    break;
                default:
                    logger.Info($"{condition}\n{stackTrace}");
                    break;
            }
        }

        /// <summary>
        /// 检查配置文件
        /// </summary>
        public static void CheckConfigFile()
        {
            if (FileUtil.CheckDir(LOG_CONFIG_FOLDER))
            {
                Log.Info($"创建log4net配置文件夹: {LOG_CONFIG_FOLDER}");
            }

            if (FileUtil.CheckFile(LOG_CONFIG_PATH))
            {
                Log.Info($"创建log4net默认配置文件: {LOG_CONFIG_PATH}");
                FileUtil.WriteAllText(LOG_CONFIG_PATH, ConfigFileContent);
            }
        }

        /// <summary>
        /// 配置文件默认内容
        /// </summary>
        private const string ConfigFileContent = @"
<log4net>
    <appender name=""UnityAppender"" type=""log4net.Appender.FileAppender"">
        <file type=""log4net.Util.PatternString"" value=""%property{ApplicationLogPath}\\%property{LogFileName}.log"" />
        <appendToFile value=""true"" />
        <datePattern value=""yyyy-MM-dd"" />
        <maximumFileSize value=""30MB"" />
        <layout type=""log4net.Layout.PatternLayout"">
            <conversionPattern value=""[%date] (t-%thread) %logger | %-5level: ~ %message%newline"" />
        </layout>
    </appender>
    <root>
        <level value=""DEBUG"" />
        <appender-ref ref=""UnityAppender"" />
    </root>
</log4net>
";
    }
}