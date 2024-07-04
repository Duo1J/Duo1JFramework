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
        /// AssetBundle与资源列表映射
        /// </summary>
        private Dictionary<string, List<string>> ab2AssetMap;

        /// <summary>
        /// 资源与AssetBundle映射
        /// </summary>
        private Dictionary<string, string> asset2ABMap;

        /// <summary>
        /// 通过资源路径获取对应AssetBunble名
        /// </summary>
        public string GetAssetBundleNameByAsset(string assetPath)
        {
            if (asset2ABMap.TryGetValue(assetPath, out string abName))
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
            ab2AssetMap = JsonUtil.ToObject<Dictionary<string, List<string>>>(jsonStr);
            if (asset2ABMap == null)
            {
                asset2ABMap = new Dictionary<string, string>();
            }
            else
            {
                asset2ABMap.Clear();
            }

            foreach (KeyValuePair<string, List<string>> kv in ab2AssetMap)
            {
                if (kv.Value != null)
                {
                    string key = kv.Key;
                    foreach (string asset in kv.Value)
                    {
#if UNITY_EDITOR
                        if (asset2ABMap.ContainsKey(asset))
                        {
                            Log.Error($"assetMap中已包含`{asset}`, 其值为`{asset2ABMap[asset]}`");
                            continue;
                        }
#endif
                        try
                        {
                            asset2ABMap.Add(asset, key);
                        }
                        catch (Exception e)
                        {
                            Assert.ExceptHandle(e);
                        }
                    }
                }
            }
        }

        public static void Save(Dictionary<string, List<string>> _abAssetList, string path = null)
        {
            Assert.GuardEditor("非Editor下不可保存ABMapData");

            if (_abAssetList == null || _abAssetList.Count == 0)
            {
                Log.ErrorForce("参数_abAssetList为空，无法保存");
                return;
            }

            if (string.IsNullOrEmpty(path))
            {
                path = PathUtil.GetABMapDataPath();
            }

            string jsonStr = JsonUtil.ToJson(_abAssetList);
            FileUtil.WriteAllText(path, jsonStr);
        }

        public static ABMapData Load(string path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = PathUtil.GetABMapDataPath();
            }

            string cfg = FileUtil.ReadAllText(path);
            return new ABMapData(cfg);
        }

        public ABMapData()
        {
            ab2AssetMap = new Dictionary<string, List<string>>();
            asset2ABMap = new Dictionary<string, string>();
        }

        public ABMapData(string jsonStr)
        {
            Parse(jsonStr);
        }
    }
}