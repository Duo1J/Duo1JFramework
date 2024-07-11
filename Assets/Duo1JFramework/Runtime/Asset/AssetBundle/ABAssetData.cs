using Duo1JFramework.TimerUpdate;
using System;
using UnityEngine;

using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// AssetBundle资源数据
    /// </summary>
    public class ABAssetData : IEditorDrawer
    {
        /// <summary>
        /// 资源路径
        /// </summary>
        private string assetPath;

        /// <summary>
        /// 引用的ABData
        /// </summary>
        private ABData abData;

        /// <summary>
        /// 加载出来的资源
        /// </summary>
        private UObject asset;

        /// <summary>
        /// 引用计数
        /// </summary>
        private int refCnt;

        /// <summary>
        /// 是否异步加载中
        /// </summary>
        private bool loading = false;

        /// <summary>
        /// 异步加载完成回调
        /// </summary>
        private Action asyncLoadedCallback;

        /// <summary>
        /// 引用的AssetBundle
        /// </summary>
        private AssetBundle assetBundle
        {
            get
            {
                AssetBundle assetBundle = abData.AB;
                Assert.NotNull(assetBundle, "访问AssetBundle时，其值为空");
                return assetBundle;
            }
        }

        /// <summary>
        /// 资源是否已加载
        /// </summary>
        public bool AssetLoaded => asset != null;

        /// <summary>
        /// 异步加载资源
        /// </summary>
        public void Load<T>(Action<T> callback) where T : UObject
        {
            Assert.NotNull(callback, "回调不可为空");

            if (AssetLoaded)
            {
                callback(Alloc<T>());
                return;
            }

            asyncLoadedCallback += () =>
            {
                callback(Alloc<T>());
            };

            if (loading)
            {
                return;
            }

            loading = true;
            AssetBundleRequest request = assetBundle.LoadAssetAsync(assetPath);
            UpdateManager.Instance.RegisterAsyncRequest(request, (req) =>
            {
                loading = false;
                AssetBundleRequest _request = req as AssetBundleRequest;
                if (!AssetLoaded)
                {
                    asset = _request.asset;
                }
                else
                {
                    Log.Warn($"{ToString()} 资源已加载，抛弃本次异步结果");
                }

                if (!AssetLoaded)
                {
                    Log.ErrorForce($"{ToString()} 资源加载失败");
                }

                asyncLoadedCallback?.Invoke();
                asyncLoadedCallback = null;
            });
        }

        /// <summary>
        /// 同步加载资源
        /// </summary>
        public T LoadSync<T>() where T : UObject
        {
            if (!AssetLoaded)
            {
                asset = assetBundle.LoadAsset(assetPath);
                asyncLoadedCallback?.Invoke();
                asyncLoadedCallback = null;
            }

            return Alloc<T>();
        }

        private T Alloc<T>() where T : UObject
        {
            if (!AssetLoaded)
            {
                Log.ErrorForce($"{ToString()} 资源为空，无法分配");
                return null;
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
                refCnt = 0;
            }
        }

        /// <summary>
        /// 是否可卸载
        /// </summary>
        public bool CanUnload()
        {
            if (!AssetLoaded)
            {
                return false;
            }

            if (loading)
            {
                return false;
            }

            return refCnt <= 0;
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        public bool Unload(bool force = false)
        {
            if (force || CanUnload())
            {
                refCnt = 0;
                loading = false;
                asyncLoadedCallback = null;

                Resources.UnloadAsset(asset);
                asset = null;
                return true;
            }

            return false;
        }

        public ABAssetData(ABData abData, string assetPath)
        {
            this.abData = abData;
            this.assetPath = assetPath;
        }

        public override string ToString()
        {
            return $"<{assetPath}>";
        }

        public void DrawEditorInfo()
        {
            ED.Vertical(() =>
            {
                GUILayout.Label($"资源路径: {assetPath}");
                GUILayout.Label($"已加载: {AssetLoaded}{ED.S4}加载中: {loading}");
                GUILayout.Label($"引用计数: {refCnt}");
                GUILayout.Label($"引用AB: {(abData == null ? "NULL" : abData.ToString())}");
            });
        }
    }
}
