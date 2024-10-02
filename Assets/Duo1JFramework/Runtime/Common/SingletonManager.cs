using System.Collections.Generic;

namespace Duo1JFramework
{
    /// <summary>
    /// 单例管理器
    /// </summary>
    /// <see cref="Singleton{T}"/>
    /// <see cref="MonoSingleton{T}"/>
    public static class SingletonManager
    {
        /// <summary>
        /// 单例集合
        /// </summary>
        private static HashSet<ISingleton> singletonSet = new HashSet<ISingleton>();

        /// <summary>
        /// Mono单例集合
        /// </summary>
        private static HashSet<ISingleton> monoSingletonSet = new HashSet<ISingleton>();

        /// <summary>
        /// 集合中添加单例
        /// </summary>
        public static void AddSingleton(ISingleton singleton)
        {
            if (singleton == null)
            {
                return;
            }

            singletonSet.Add(singleton);
        }

        /// <summary>
        /// 集合中移除单例
        /// </summary>
        public static void RemoveSingleton(ISingleton singleton)
        {
            if (singleton == null)
            {
                return;
            }

            singletonSet.Remove(singleton);
        }

        /// <summary>
        /// 集合中添加Mono单例
        /// </summary>
        public static void AddMonoSingleton(ISingleton MonoSingleton)
        {
            if (MonoSingleton == null)
            {
                return;
            }

            monoSingletonSet.Add(MonoSingleton);
        }

        /// <summary>
        /// 集合中移除Mono单例
        /// </summary>
        public static void RemoveMonoSingleton(ISingleton MonoSingleton)
        {
            if (MonoSingleton == null)
            {
                return;
            }

            monoSingletonSet.Remove(MonoSingleton);
        }

        /// <summary>
        /// 触发内部单例类
        /// </summary>
        public static void TriggerInner()
        {
            Log.Info("触发内部单例");

            GameManager.Instance.Trigger();
        }

        /// <summary>
        /// 停止所有单例类
        /// </summary>
        public static void DisposeAll()
        {
            Log.Info("停止所有单例");

            if (singletonSet != null)
            {
                foreach (ISingleton singleton in singletonSet)
                {
                    singleton.Dispose();
                }

                singletonSet.Clear();
            }

            if (monoSingletonSet != null)
            {
                foreach (ISingleton monoSingleton in monoSingletonSet)
                {
                    monoSingleton.Dispose();
                }

                monoSingletonSet.Clear();
            }
        }
    }
}
