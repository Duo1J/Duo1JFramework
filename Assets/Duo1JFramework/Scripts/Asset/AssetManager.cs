using System;
using UnityEditor;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public class AssetManager : MonoSingleton<AssetManager>
    {
        public void Load<T>(string assetPath, Action<T> callback) where T : UnityEngine.Object
        {
            if (Game.IsEditor)
            {
                callback?.Invoke(LoadSync<T>(assetPath));
            }
            else
            {

            }
        }

        public T LoadSync<T>(string assetPath) where T : UnityEngine.Object
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

        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}