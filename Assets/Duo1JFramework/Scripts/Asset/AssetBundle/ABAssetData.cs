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
        /// <summary>
        /// 资源路径
        /// </summary>
        private string assetPath;

        /// <summary>
        /// 引用的AssetBundle
        /// </summary>
        private AssetBundle assetBundle;

        /// <summary>
        /// 加载出来的资源
        /// </summary>
        private UObject asset;

        /// <summary>
        /// 引用计数
        /// </summary>
        private int refCnt;

        /// <summary>
        /// 异步加载资源
        /// </summary>
        public void Load<T>(Action<T> callback) where T : UObject
        {
            if (asset != null)
            {
                callback?.Invoke(asset.Convert<T>());
                AddRef();
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

                AddRef();
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

            AddRef();
            return asset.Convert<T>();
        }

        /// <summary>
        /// 添加引用计数
        /// </summary>
        private void AddRef() => ++refCnt;

        /// <summary>
        /// 减少引用计数
        /// </summary>
        public void RemoveRef()
        {
            --refCnt;
            if (refCnt < 0)
            {
                Log.ErrorForce($"{ToString()} 资源引用计数异常小于0");
            }
        }

        /// <summary>
        /// 是否可卸载
        /// </summary>
        public bool CanUnload()
        {
            return refCnt <= 0;
        }

        public ABAssetData(string assetPath, AssetBundle assetBundle)
        {
            this.assetPath = assetPath;
            this.assetBundle = assetBundle;
        }

        public override string ToString()
        {
            return $"<{assetPath}>";
        }
    }
}
