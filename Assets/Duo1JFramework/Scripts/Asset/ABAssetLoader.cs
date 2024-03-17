using Duo1JFramework.Config;
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
#if UNITY_EDITOR
            if (!CheckEditorUseABCfgOn())
            {
                callback(null);
                return;
            }
#endif

            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            Assert.NotNull(callback, "回调不可为空");

            ABData abData = ABManager.Instance.GetABDataByAsset(assetPath);
            if (abData == null)
            {
                Log.ErrorForce($"加载资源`{assetPath}`时，无法获取其对应的ABData");
                callback(null);
                return;
            }

            abData.Load<T>(assetPath, callback);
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public override T LoadSync<T>(string assetPath)
        {
#if UNITY_EDITOR
            if (!CheckEditorUseABCfgOn())
            {
                return null;
            }
#endif

            Assert.NotNull(assetPath, "资源路径不可为空");

            ABData abData = ABManager.Instance.GetABDataByAsset(assetPath);
            if (abData == null)
            {
                Log.ErrorForce($"加载资源`{assetPath}`时，无法获取其对应的ABData");
                return null;
            }

            return abData.LoadSync<T>(assetPath);
        }

        /// <summary>
        /// 异步加载实例
        /// </summary>
        public override void LoadIns<T>(string assetPath, Action<T> callback)
        {
#if UNITY_EDITOR
            if (!CheckEditorUseABCfgOn())
            {
                callback(null);
                return;
            }
#endif
            base.LoadIns<T>(assetPath, callback);
        }

        /// <summary>
        /// 同步加载实例
        /// </summary>
        public override T LoadInsSync<T>(string assetPath)
        {
#if UNITY_EDITOR
            if (!CheckEditorUseABCfgOn())
            {
                return null;
            }
#endif
            return base.LoadInsSync<T>(assetPath);
        }

        /// <summary>
        /// 检查UseAB选项是否开启
        /// </summary>
        private bool CheckEditorUseABCfgOn()
        {
#if UNITY_EDITOR
            if (!GameConfig.Instance.EditorUseAB)
            {
                Log.EditorError("编辑器下使用ABAssetLoader请勾选GameConfig.EditorUseAB");
                return false;
            }
            return true;
#else
                return true;
#endif
        }
    }
}
