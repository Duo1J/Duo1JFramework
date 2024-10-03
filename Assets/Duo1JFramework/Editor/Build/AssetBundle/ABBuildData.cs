using System.Collections.Generic;
using UnityEditor;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// AssetBundle构建数据
    /// </summary>
    public class ABBuildData
    {
        public string abName;

        public List<string> assetPathList;

        public AssetBundleBuild ToAssetBundleBuild()
        {
            return new AssetBundleBuild()
            {
                assetBundleName = abName,
                assetNames = assetPathList == null ? new string[0] : assetPathList.ToArray()
            };
        }

        public ABBuildData(string abName)
        {
            this.abName = abName;
        }
    }
}
