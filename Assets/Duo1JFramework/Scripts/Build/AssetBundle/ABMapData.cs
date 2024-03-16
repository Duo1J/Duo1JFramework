using Duo1JFramework.Asset;
using System;
using System.Collections.Generic;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// Assetbundle映射数据
    /// </summary>
    public class ABMapData
    {
        /// <summary>
        /// AssetBundle下的资源列表映射
        /// </summary>
        public Dictionary<string, List<string>> abAssetList;

        /// <summary>
        /// 资源与AssetBundle映射
        /// </summary>
        private Dictionary<string, string> assetMap;

        /// <summary>
        /// 通过资源路径获取对应AssetBunble名
        /// </summary>
        public string GetAssetBundleNameByAsset(string assetPath)
        {
            if (assetMap.TryGetValue(assetPath, out string abName))
            {
                return abName;
            }

            Log.ErrorForce($"未在assetMap中找到资源 `{assetPath}`");
            return null;
        }

        /// <summary>
        /// 解析Json
        /// </summary>
        public void Parse(string jsonStr)
        {
            abAssetList = JsonUtil.ToObject<Dictionary<string, List<string>>>(jsonStr);
            if (assetMap == null)
            {
                assetMap = new Dictionary<string, string>();
            }
            else
            {
                assetMap.Clear();
            }

            foreach (KeyValuePair<string, List<string>> kv in abAssetList)
            {
                if (kv.Value != null)
                {
                    string key = kv.Key;
                    foreach (string asset in kv.Value)
                    {
#if UNITY_EDITOR
                        if (assetMap.ContainsKey(asset))
                        {
                            Log.Error($"assetMap中已包含`{asset}`, 其值为`{assetMap[asset]}`");
                            continue;
                        }
#endif
                        try
                        {
                            assetMap.Add(asset, key);
                        }
                        catch (Exception e)
                        {
                            Assert.ExceptHandle(e);
                        }
                    }
                }
            }
        }

        public void Save(string path = null)
        {
            Assert.GuardEditor("非Editor下不可保存ABMapData");

            if (abAssetList == null)
            {
                Log.ErrorForce("abAssetList为空，无法保存");
                return;
            }

            if (string.IsNullOrEmpty(path))
            {
                path = Path.GetABMapDataPath();
            }

            string jsonStr = JsonUtil.ToJson(abAssetList);
            FileUtil.WriteAllText(path, jsonStr);
        }

        public static ABMapData Load(string path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Path.GetABMapDataPath();
            }

            string cfg = FileUtil.ReadAllText(path);
            return new ABMapData(cfg);
        }

        public ABMapData()
        {
            abAssetList = new Dictionary<string, List<string>>();
            assetMap = new Dictionary<string, string>();
        }

        public ABMapData(string jsonStr)
        {
            Parse(jsonStr);
        }
    }
}