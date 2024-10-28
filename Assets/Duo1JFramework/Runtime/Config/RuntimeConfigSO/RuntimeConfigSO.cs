using Duo1JFramework.Asset;
using System.IO;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 运行时配置ScriptableObject
    /// </summary>
    public abstract class RuntimeConfigSO<T> : ScriptableObject where T : ScriptableObject
    {
        private static T instance;

        /// <summary>
        /// 配置实例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    IAssetHandle<T> handle = AssetManager.Instance.LoadResourceSync<T>(AssetLoadPath);
                    if (handle != null)
                    {
                        instance = handle.Instantiate();
                    }

                    if (instance == null)
                    {
                        instance = CreateInstance<T>();
#if UNITY_EDITOR
                        Log.ErrorForce($"编辑器下创建默认运行时配置: `{AssetSavePath}`");
                        UnityEditor.AssetDatabase.CreateAsset(instance, AssetSavePath);
                        UnityEditor.EditorUtility.SetDirty(instance);
                        UnityEditor.AssetDatabase.SaveAssets();
                        UnityEditor.AssetDatabase.Refresh();
#else
                        Log.ErrorForce($"加载运行时配置失败: `{AssetLoadPath}`");
#endif
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// 配置实例加载路径
        /// </summary>
        public static string AssetLoadPath => Path.Combine(Def.Path.RUNTIME_CONFIG_LOAD_PATH_PREFIX, typeof(T).Name);

        /// <summary>
        /// 配置实例保存路径
        /// </summary>
        public static string AssetSavePath => Path.Combine(Def.Path.RUNTIME_CONFIG_SAVE_PATH_PREFIX.CheckDir(), $"{typeof(T).Name}.asset");

        /// <summary>
        /// 实例触发
        /// </summary>
        public void Trigger()
        {
        }

        public override string ToString()
        {
            return $"<{AssetSavePath}>";
        }
    }
}
