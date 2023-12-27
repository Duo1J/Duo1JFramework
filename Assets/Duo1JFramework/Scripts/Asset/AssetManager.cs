using System;
using System.Collections;
using UnityEditor;
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
            Assert.NotNull(callback, "回调不可为空");
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");

            if (Game.IsEditor)
            {
                callback(LoadSync<T>(assetPath));
            }
            else
            {

            }
        }

        public T LoadSync<T>(string assetPath) where T : UObject
        {
            Assert.NotNull(assetPath, "资源路径为空");

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
        public T LoadResource<T>(string assetPath) where T : UObject
        {
            Assert.NotNull(assetPath, "资源路径不可为空");

            T asset = Resources.Load<T>(assetPath);
            if (asset == null)
            {
                Log.Error($"无法加载到Resources资源: `{assetPath}`");
                return null;
            }
            T ins = Instantiate(asset);
            return ins;
        }

        /// <summary>
        /// 异步加载Resources资源
        /// </summary>
        public void LoadResourceASync<T>(string assetPath, Action<T> callback) where T : UObject
        {
            Assert.NotNull(callback, "回调不可为空");
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");

            ResourceRequest request = Resources.LoadAsync<T>(assetPath);
            Coro.Instance.StartCoro(WaitResourceRequest(request, (asset) =>
            {
                if (asset == null)
                {
                    Log.Error($"无法加载到Resources资源: `{assetPath}`");
                    callback(null);
                    return;
                }
                T ins = Instantiate(asset) as T;
                if (ins == null)
                {
                    Log.Error($"实例化Resources资源失败: `{assetPath}`");
                    callback(null);
                    return;
                }
                callback(ins);
            }));
        }

        /// <summary>
        /// 等待Resources资源加载完毕
        /// </summary>
        private IEnumerator WaitResourceRequest(ResourceRequest request, Action<UObject> callback)
        {
            yield return request;
            callback?.Invoke(request.asset);
        }

        protected override void OnInit()
        {
        }

        protected override void OnDispose()
        {
        }
    }
}