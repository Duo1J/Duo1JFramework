using Duo1JFramework.Build;
using Duo1JFramework.Config;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// AssetBundle管理器
    /// </summary>
    public class ABManager : MonoSingleton<ABManager>, IEditorDrawer
    {
        private AssetBundle mainAB;
        private AssetBundleManifest manifest;

        private ABMapData abMapData;
        private Dictionary<string, ABData> abDataDict;

        /// <summary>
        /// 是否使用AssetBundle加载
        /// </summary>
        public bool UseAssetBundle
        {
            get
            {
                return GameOption.AssetLoaderType == EAssetLoaderType.AssetBundle;
            }
        }

        /// <summary>
        /// 通过资源路径获取对应的ABData
        /// </summary>
        public ABData GetABDataByAsset(string assetPath)
        {
            string abName = GetABNameByAsset(assetPath);
            return GetABDataByName(abName);
        }

        /// <summary>
        /// 通过AssetBundle名获取ABData
        /// </summary>
        public ABData GetABDataByName(string assetBundleName)
        {
            if (string.IsNullOrEmpty(assetBundleName))
            {
                return null;
            }

            if (!abDataDict.TryGetValue(assetBundleName, out ABData abData))
            {
                abData = new ABData(assetBundleName);
                abDataDict.Add(assetBundleName, abData);
            }
            return abData;
        }

        /// <summary>
        /// 通过资源路径获取对应AssetBunble名
        /// </summary>
        public string GetABNameByAsset(string assetPath)
        {
            return abMapData.GetABNameByAsset(assetPath);
        }

        /// <summary>
        /// 通过AssetBundle名获取Hash字符串
        /// </summary>
        public string GetHashStrByABName(string abName)
        {
            return abMapData.GetHashByABName(abName);
        }

        /// <summary>
        /// 通过AssetBundle名获取CRC
        /// </summary>
        public uint GetCRCByABName(string abName)
        {
            return abMapData.GetCRCByABName(abName);
        }

        /// <summary>
        /// 通过AssetBundle名获取MD5
        /// </summary>
        public string GetMD5ByABName(string abName)
        {
            return abMapData.GetMD5ByABName(abName);
        }

        /// <summary>
        /// 获取依赖引用的ABData列表
        /// </summary>
        public List<ABData> GetRefABDataList(string assetBundleName)
        {
            List<ABData> abDataList = new List<ABData>();

            string[] dependencies = manifest.GetAllDependencies(assetBundleName);
            foreach (string dependency in dependencies)
            {
                abDataList.Add(GetABDataByName(dependency));
            }

            return abDataList;
        }

        public void GC()
        {
            foreach (KeyValuePair<string, ABData> kv in abDataDict)
            {
                kv.Value.TryUnload();
            }
        }

        protected override void OnDispose()
        {
        }

        protected override void OnInit()
        {
            abMapData = ABMapData.LoadFromFile(Def.Asset.EncryptABMapData);
            abDataDict = new Dictionary<string, ABData>();
            InitMainAssetBundle();

            Register.RegisterLateUpdate(OnLateUpdate);
        }

        private void OnLateUpdate()
        {
            foreach (KeyValuePair<string, ABData> kv in abDataDict)
            {
                kv.Value.Tick();
            }
        }

        private void InitMainAssetBundle()
        {
            string mainAssetBundlePath = PathUtil.GetAssetBundlePath(Def.Path.ASSET_BUNDLE_MAIN_NAME);
            mainAB = AssetBundle.LoadFromFile(mainAssetBundlePath);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        public void DrawEditorInfo()
        {
            if (!UseAssetBundle)
            {
                ED.HelpBox("当前未使用AssetBundle加载");
                return;
            }

            if (abDataDict == null || abDataDict.Count == 0)
            {
                ED.HelpBox("abDataDict为空");
                return;
            }

            ES.SetRichText();
            foreach (KeyValuePair<string, ABData> kv in abDataDict)
            {
                GUILayout.Space(20);
                ED.Vertical(kv.Value.DrawEditorInfo, "box");
            }
        }
    }
}
