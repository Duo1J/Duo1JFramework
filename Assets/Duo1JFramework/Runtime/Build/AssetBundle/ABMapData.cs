using System;
using System.Collections.Generic;
using System.Text;

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
            Assert.NotNull(jsonStr, "ABMapData::Parse参数jsonStr为空");

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

        /// <summary>
        /// 保存到文件
        /// </summary>
        /// <param name="encrypt">是否加密, {Def.Asset.EncryptABMapData}</param>
        /// <param name="path">文件路径, 默认值 {PathUtil.GetABMapDataPath()}</param>
        public static void Save(Dictionary<string, List<string>> _abAssetList, bool encrypt, string path = null)
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
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonStr);
            if (encrypt)
            {
                jsonBytes = CryptoUtil.AesEncrypt(jsonBytes, Def.AesKeyByte);
            }

            FileUtil.WriteAllBytes(path, jsonBytes);
        }

        /// <summary>
        /// 从文件读取
        /// </summary>
        /// <param name="encrypt">是否加密, {Def.Asset.EncryptABMapData}</param>
        /// <param name="path">文件路径, 默认值 {PathUtil.GetABMapDataPath()}</param>
        public static ABMapData Load(bool encrypt, string path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = PathUtil.GetABMapDataPath();
            }

            byte[] jsonBytes = null;
            if (encrypt)
            {
                jsonBytes = CryptoUtil.AesDecrypt(path, Def.AesKeyByte);
            }
            else
            {
                jsonBytes = FileUtil.ReadAllBytes(path);
            }

            string jsonStr = Encoding.UTF8.GetString(jsonBytes);
            return new ABMapData(jsonStr);
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
