using Duo1JFramework.TimerUpdate;
using System;
using UnityEngine;

using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// AssetBundle资源数据
    /// </summary>
    public class ABAssetData
    {
        private string assetPath;

        private AssetBundle assetBundle;

        private UObject asset;

        /// <summary>
        /// 异步加载资源
        /// </summary>
        public void Load<T>(Action<T> callback) where T : UObject
        {
            if (asset != null)
            {
                callback?.Invoke(asset.Convert<T>());
                return;
            }

            AssetBundleRequest request = assetBundle.LoadAssetAsync(assetPath);
            UpdateManager.Instance.RegisterAsyncRequest(request, (req) =>
            {
                AssetBundleRequest _request = req as AssetBundleRequest;
                if (asset != null)
                {
                    Log.Warn($"{ToString()} 资源已加载，抛弃本次异步结果");
                    _request.asset.DestroyImmediate();
                }
                else
                {
                    asset = _request.asset;
                }

                if (asset == null)
                {
                    Log.ErrorForce($"{ToString()} 资源加载失败");
                }

                callback(asset.Convert<T>());
            });
        }

        /// <summary>
        /// 同步加载资源
        /// </summary>
        public T LoadSync<T>() where T : UObject
        {
            if (asset == null)
            {
                asset = assetBundle.LoadAsset(assetPath);
            }

            return asset.Convert<T>();
        }

        public ABAssetData(string assetPath, AssetBundle assetBundle)
        {
            this.assetPath = assetPath;
            this.assetBundle = assetBundle;
        }
    }
}
