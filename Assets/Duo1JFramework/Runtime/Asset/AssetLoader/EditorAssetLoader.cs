using System;
using UnityEditor;
using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 编辑器下资源加载器
    /// </summary>
    public class EditorAssetLoader : BaseAssetLoader
    {
        /// <summary>
        /// 异步加载
        /// </summary>
        public override void Load<T>(string assetPath, Action<IAssetHandle<T>> callback)
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            Assert.NotNullArg(callback, "callback");

#if UNITY_EDITOR
            callback(LoadSync<T>(assetPath));
#else
            Assert.GuardEditor("EditorAssetLoader::Load<T>()");
            callback(null);
#endif
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public override IAssetHandle<T> LoadSync<T>(string assetPath)
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");

#if UNITY_EDITOR
            string targetPath = assetPath;
            T asset = AssetDatabase.LoadAssetAtPath<T>(targetPath);
            if (asset == null)
            {
                Log.ErrorForce($"无法加载到资源`{targetPath}`");
                return null;
            }

            ADAssetHandle<T> handle = ADAssetHandle<T>.Create(asset);
            return handle;
#else
            Assert.GuardEditor("EditorAssetLoader::LoadSync<T>()");
            return null;
#endif
        }
    }
}
