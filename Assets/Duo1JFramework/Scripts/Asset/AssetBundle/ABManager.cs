using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// AssetBundle管理器
    /// </summary>
    public class ABManager : MonoSingleton<ABManager>
    {
        private AssetBundle mainAB;
        private AssetBundleManifest manifest;

        private Dictionary<string, ABData> abDataDict;

        /// <summary>
        /// 获取依赖引用的ABData列表
        /// </summary>
        public List<ABData> GetRefABDataList(string assetBundleName)
        {
            List<ABData> abDataList = new List<ABData>();

            string[] dependencies = manifest.GetAllDependencies(assetBundleName);
            foreach (string dependency in dependencies)
            {
                abDataList.Add(GetABData(dependency));
            }

            return abDataList;
        }

        /// <summary>
        /// 通过AssetBundle名获取数据类
        /// </summary>
        private ABData GetABData(string assetBundleName)
        {
            if (!abDataDict.TryGetValue(assetBundleName, out ABData abData))
            {
                abData = new ABData(assetBundleName);
                abDataDict.Add(assetBundleName, abData);
            }
            return abData;
        }

        protected override void OnDispose()
        {
        }

        protected override void OnInit()
        {
            abDataDict = new Dictionary<string, ABData>();
            InitMainAssetBundle();
        }

        private void InitMainAssetBundle()
        {
            string mainAssetBundlePath = Path.GetAssetBundlePath("AssetBundle");
            mainAB = AssetBundle.LoadFromFile(mainAssetBundlePath);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
    }
}
