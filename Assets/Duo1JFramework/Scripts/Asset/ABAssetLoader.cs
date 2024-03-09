using System;

using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 运行时AssetBundle加载器
    /// </summary>
    public class ABAssetLoader : BaseAssetLoader
    {
        /// <summary>
        /// 异步加载
        /// </summary>
        public override void Load<T>(string assetPath, Action<T> callback)
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            Assert.NotNull(callback, "回调不可为空");
            throw new NotImplementedException();
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public override T LoadSync<T>(string assetPath)
        {
            Assert.NotNull(assetPath, "资源路径不可为空");
            throw new NotImplementedException();
        }

        /// <summary>
        /// 异步加载实例
        /// </summary>
        public override void LoadIns<T>(string assetPath, Action<T> callback)
        {
#if !UNITY_EDITOR
            Assert.GuardRuntime("RuntimeAssetLoader::LoadIns<T>()");
            callback(null);
#else
            base.LoadIns<T>(assetPath, callback);
#endif
        }

        /// <summary>
        /// 同步加载实例
        /// </summary>
        public override T LoadInsSync<T>(string assetPath)
        {
#if !UNITY_EDITOR
            Assert.GuardRuntime("RuntimeAssetLoader::LoadInsSync<T>()");
            return null;
#else
            return base.LoadInsSync<T>(assetPath);
#endif
        }
    }
}
