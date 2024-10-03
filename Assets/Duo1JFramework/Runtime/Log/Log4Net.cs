using Duo1JFramework.Asset;
using Duo1JFramework.Config;
using Duo1JFramework.ObjectPool;
using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using TextAsset = UnityEngine.TextAsset;

namespace Duo1JFramework
{
    public static class Log4Net
    {
        private static ILog logger = GetLogger("D");

        /// <summary>
        /// 默认Logger
        /// </summary>
        public static ILog Logger => logger;

        /// <summary>
        /// 通过类型获取Logger
        /// </summary>
        public static ILog GetLogger(Type t)
        {
            return LogManager.GetLogger(t);
        }

        /// <summary>
        /// 通过字符串获取Logger
        /// </summary>
        public static ILog GetLogger(string s)
        {
            return LogManager.GetLogger(s);
        }

        /// <summary>
        /// 配置文件夹
        /// </summary>
        public static readonly string LOG_CONFIG_FOLDER = Path.Combine(Def.Path.RES_PATH_PREFIX, "Log4Net");

        /// <summary>
        /// 配置文件路径
        /// </summary>
        public static readonly string LOG_CONFIG_PATH = Path.Combine(LOG_CONFIG_FOLDER, "log4net");

        /// <summary>
        /// 覆写配置文件夹
        /// </summary>
        public static readonly string LOG_CONFIG_OVERRIDE_FOLDER = Path.Combine(Def.Path.Streaming, Def.FRAME_WORK_NAME);

        /// <summary>
        /// 覆写配置文件路径
        /// </summary>
        public static readonly string LOG_CONFIG_OVERRIDE_PATH = Path.Combine(LOG_CONFIG_OVERRIDE_FOLDER, "log4net.cfg");

        /// <summary>
        /// 输出文件夹
        /// </summary>
        public static readonly string LOG_FOLDER_PATH = Path.Combine(Def.Path.Persistent, Def.FRAME_WORK_NAME, "Log");

        /// <summary>
        /// 输出文件名
        /// </summary>
        public const string LOG_FILE_NAME = "log";

        private const string PROPKEY_FOLDER = "ApplicationLogPath";
        private const string PROPKEY_FILE = "LogFileName";

        private static bool initialized = false;

        /// <summary>
        /// Log4Net初始化
        /// </summary>
        public static void Init()
        {
            if (!GameOption.UseLog4Net)
            {
                return;
            }

            if (initialized)
            {
                Log.ErrorForce("Log4Net 重复初始化");
                return;
            }

            try
            {
                InitLog4NetConfig();

                Log.Info("Log4Net 初始化成功");
                Application.logMessageReceived += OnLogMessageReceived;

                PrintSpaceLineToLogFile(2);
                initialized = true;
            }
            catch (Exception e)
            {
                initialized = false;
                Assert.ExceptHandle(e, "Log4Net 初始化异常");
            }
        }

        /// <summary>
        /// Log4Net关闭
        /// </summary>
        public static void Shutdown()
        {
            //PrintSpaceLineToLogFile(2);
            initialized = false;

            LogManager.Shutdown();
            Application.logMessageReceived -= OnLogMessageReceived;

            Log.Info("Log4Net 关闭");
        }

        /// <summary>
        /// UnityLog接收
        /// </summary>
        private static void OnLogMessageReceived(string msg, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Log:
                    logger.Info(msg);
                    break;
                case LogType.Warning:
                    logger.Warn($"{msg}\n{stackTrace}");
                    break;
                case LogType.Error:
                    logger.Error($"{msg}\n{stackTrace}");
                    break;
                case LogType.Assert:
                    logger.Error($"{msg}\n{stackTrace}");
                    break;
                case LogType.Exception:
                    logger.Fatal($"{msg}\n{stackTrace}");
                    break;
                default:
                    logger.Info($"{msg}\n{stackTrace}");
                    break;
            }
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        private static void InitLog4NetConfig()
        {
            GlobalContext.Properties[PROPKEY_FILE] = LOG_FILE_NAME;
            GlobalContext.Properties[PROPKEY_FOLDER] = LOG_FOLDER_PATH;

            Stream stream = null;
            if (File.Exists(LOG_CONFIG_OVERRIDE_PATH))
            {
                Log.Info("Log4Net使用Streaming配置");
                stream = new FileStream(LOG_CONFIG_OVERRIDE_PATH, FileMode.OpenOrCreate);
            }
            else
            {
                TextAsset textAsset = AssetManager.Instance.LoadResourceSync<TextAsset>(LOG_CONFIG_PATH);
                if (textAsset != null)
                {
                    Log.Info("Log4Net使用Resources配置");
                    stream = new MemoryStream(Encoding.UTF8.GetBytes(textAsset.text));
                }
            }

            if (stream == null)
            {
                Log.Info("Log4Net使用默认配置");
                stream = new MemoryStream(Encoding.UTF8.GetBytes(ConfigFileContent));
            }

            XmlConfigurator.Configure(stream);
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


        /// <summary>
        /// 向日志文件打印空行
        /// </summary>
        /// <param name="num"></param>
        public static void PrintSpaceLineToLogFile(int num = 1)
        {
            if (num <= 0)
            {
                return;
            }

            Pool.StringBuilderPool.Using((sb) =>
            {
                for (int i = 0; i < num; ++i)
                {
                    sb.AppendLine();
                }

                logger.Info(sb.ToString());
            });
        }
    }
}
