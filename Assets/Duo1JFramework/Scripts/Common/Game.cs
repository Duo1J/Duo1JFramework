using Duo1JFramework.Asset;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 游戏状态静态类
    /// </summary>
    public static class Game
    {
        /// <summary>
        /// 是否在编辑器下
        /// </summary>
        public static bool IsEditor => Application.isEditor;

        /// <summary>
        /// 是否游戏准备退出
        /// </summary>
        public static bool IsQuit { get; set; }

        /// <summary>
        /// 是否游戏运行中
        /// </summary>
        public static bool IsPlaying => Application.isPlaying;

        /// <summary>
        /// 是否处于调试模式
        /// </summary>
        public static bool IsDebug => IsEditor;

        /// <summary>
        /// 触发单例类
        /// </summary>
        public static void TriggerSingleton()
        {
            GameManager.Instance.Trigger();

            Log.Info("Singleton has been triggered.");
        }

        /// <summary>
        /// 内存清理
        /// </summary>
        public static void GC()
        {
            AssetManager.Instance.GC();
            System.GC.Collect();

            Log.Info("GC called.");
        }
    }
}