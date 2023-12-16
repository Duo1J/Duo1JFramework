using System;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public class AssetManager : MonoSingleton<AssetManager>
    {
        public void Load<T>(string assetPath, Action<T> callback) where T : UObject
        {
            if (Game.IsEditor)
            {
                callback?.Invoke(LoadSync<T>(assetPath));
            }
            else
            {

            }
        }

        public T LoadSync<T>(string assetPath) where T : UObject
        {
            string targetPath = Path.ASSET_PATH_PREFIX + assetPath;
            if (Game.IsEditor)
            {
                T asset = AssetDatabase.LoadAssetAtPath<T>(targetPath);
                if (asset == null)
                {
                    Log.Error($"无法加载到资源`{targetPath}`");
                    return null;
                }
                T ins = Instantiate(asset);
                return ins;
            }
            else
            {

            }
            return default(T);
        }

        /// <summary>
        /// 同步加载Resources资源
        /// </summary>
        public T LoadResource<T>(string targetPath) where T : UObject
        {
            T asset = Resources.Load<T>(targetPath);
            if (asset == null)
            {
                Log.Error($"无法加载到Resources资源`{targetPath}`");
                return null;
            }
            T ins = Instantiate(asset);
            return ins;
        }

        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}