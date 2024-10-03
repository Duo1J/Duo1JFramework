using Duo1JFramework.Asset;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.Config
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    public class ConfigManager : MonoSingleton<ConfigManager>
    {
        #region ScriptableObject

        /// <summary>
        /// ScriptableObject缓存字典
        /// </summary>
        private Dictionary<string, ScriptableObject> soDict;

        /// <summary>
        /// 从缓存获取ScriptableObject
        /// </summary>
        public T GetSO<T>(string assetPath, EAssetLoadType loadType = EAssetLoadType.Bundle) where T : ScriptableObject
        {
            if (!soDict.TryGetValue(assetPath, out ScriptableObject so))
            {
                so = AssetManager.Instance.LoadInsByTypeSync<ScriptableObject>(assetPath, loadType);
                if (so == null)
                {
                    return null;
                }

                soDict.Add(assetPath, so);
            }

            return so as T;
        }

        /// <summary>
        /// 从缓存获取ScriptableObject
        /// </summary>
        public ScriptableObject GetSO(string assetPath, EAssetLoadType loadType = EAssetLoadType.Bundle)
        {
            return GetSO<ScriptableObject>(assetPath, loadType);
        }

        #endregion ScriptableObject

        protected override void OnInit()
        {
            soDict = new Dictionary<string, ScriptableObject>();
        }

        protected override void OnDispose()
        {
            if (soDict != null)
            {
                soDict.Clear();
                soDict = null;
            }
        }
    }
}
