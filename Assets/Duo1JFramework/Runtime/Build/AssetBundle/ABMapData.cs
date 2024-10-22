using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// Assetbundle映射数据
    /// </summary>
    public class ABMapData
    {
        /// <summary>
        /// 资源与AssetBundle映射
        /// </summary>
        [JsonProperty]
        private Dictionary<string, string> asset2ABMap;

        /// <summary>
        /// AssetBundle与Hash映射
        /// </summary>
        [JsonProperty]
        private Dictionary<string, string> ab2HashMap;

        /// <summary>
        /// AssetBundle与CRC映射
        /// </summary>
        [JsonProperty]
        private Dictionary<string, uint> ab2CrcMap;

        /// <summary>
        /// AssetBundle与MD5映射
        /// </summary>
        [JsonProperty]
        private Dictionary<string, string> ab2MD5Map;

        /// <summary>
        /// 通过资源路径获取对应AssetBunble名
        /// </summary>
        public string GetABNameByAsset(string assetPath)
        {
            if (asset2ABMap.TryGetValue(assetPath, out string abName))
            {
                return abName;
            }

            Log.ErrorForce($"未在asset2ABMap中找到资源 `{assetPath}`");
            return null;
        }

        /// <summary>
        /// 通过AssetBundle名获取Hash
        /// </summary>
        public string GetHashByABName(string abName)
        {
            if (ab2HashMap == null)
            {
                return null;
            }

            if (ab2HashMap.TryGetValue(PathUtil.ABNameUnify(abName), out string hash))
            {
                return hash;
            }
            else
            {
                Log.ErrorForce($"无法获取到Hash, abName: `{PathUtil.ABNameUnify(abName)}`");
                return null;
            }
        }

        /// <summary>
        /// 通过AssetBundle名获取CRC
        /// </summary>
        public uint GetCRCByABName(string abName)
        {
            if (!Def.Asset.BuildABCRC || ab2CrcMap == null)
            {
                return 0;
            }

            if (ab2CrcMap.TryGetValue(PathUtil.ABNameUnify(abName), out uint crc))
            {
                return crc;
            }
            else
            {
                Log.ErrorForce($"无法获取到CRC, abName: `{PathUtil.ABNameUnify(abName)}`");
                return 0;
            }
        }

        /// <summary>
        /// 通过AssetBundle名获取MD5
        /// </summary>
        public string GetMD5ByABName(string abName)
        {
            if (ab2MD5Map == null)
            {
                return null;
            }

            if (ab2MD5Map.TryGetValue(PathUtil.ABNameUnify(abName), out string md5))
            {
                return md5;
            }
            else
            {
                Log.ErrorForce($"无法获取到MD5, abName: `{PathUtil.ABNameUnify(abName)}`");
                return null;
            }
        }

        /// <summary>
        /// 解析Json为ABMapData对象
        /// </summary>
        public static ABMapData Parse(string jsonStr)
        {
            Assert.NotNullArg(jsonStr, "jsonStr");
            return JsonUtil.ToObject<ABMapData>(jsonStr);
        }

        /// <summary>
        /// 创建ABMapData对象
        /// </summary>
        public static ABMapData Create(
            Dictionary<string, List<string>> ab2AssetMap,
            Dictionary<string, string> ab2HashMap = null,
            Dictionary<string, uint> ab2CrcMap = null,
            Dictionary<string, string> ab2MD5Map = null
            )
        {
            Assert.NotNullArg(ab2AssetMap, "ab2AssetMap");

            ABMapData abMapData = new ABMapData(ab2AssetMap);
            abMapData.ab2HashMap = ab2HashMap;
            abMapData.ab2CrcMap = ab2CrcMap;
            abMapData.ab2MD5Map = ab2MD5Map;

            return abMapData;
        }

        /// <summary>
        /// 保存ABMapData对象到文件
        /// </summary>
        /// <param name="encrypt">是否加密, {Def.Asset.EncryptABMapData}</param>
        /// <param name="path">文件路径, 默认值 {PathUtil.GetABMapDataPath()}</param>
        public void SaveToFile(bool encrypt, string path = null)
        {
            Assert.GuardEditor("非Editor下不可保存ABMapData");

            if (string.IsNullOrEmpty(path))
            {
                path = PathUtil.GetABMapDataPath();
            }

            string jsonStr = JsonUtil.ToJson(this);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonStr);
            if (encrypt)
            {
                jsonBytes = CryptoUtil.AesEncrypt(jsonBytes, Def.AesKeyByte);
            }

            FileUtil.WriteAllBytes(path, jsonBytes);
        }

        /// <summary>
        /// 从文件读取ABMapData对象
        /// </summary>
        /// <param name="encrypt">是否加密, {Def.Asset.EncryptABMapData}</param>
        /// <param name="path">文件路径, 默认值 {PathUtil.GetABMapDataPath()}</param>
        public static ABMapData LoadFromFile(bool encrypt, string path = null)
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
            return Parse(jsonStr);
        }

        private ABMapData(Dictionary<string, List<string>> ab2AssetMap)
        {
            asset2ABMap = new Dictionary<string, string>();
            foreach (KeyValuePair<string, List<string>> kv in ab2AssetMap)
            {
                if (kv.Value == null)
                {
                    continue;
                }

                string key = kv.Key;
                foreach (string asset in kv.Value)
                {
                    try
                    {
#if UNITY_EDITOR
                        if (asset2ABMap.ContainsKey(asset))
                        {
                            Log.Error($"asset2ABMap中已包含`{asset}`, 其值为`{asset2ABMap[asset]}`");
                            continue;
                        }
#endif

                        asset2ABMap.Add(asset, key);
                    }
                    catch (Exception e)
                    {
                        Assert.ExceptHandle(e);
                    }
                }
            }
        }

        private ABMapData()
        {
        }
    }
}
