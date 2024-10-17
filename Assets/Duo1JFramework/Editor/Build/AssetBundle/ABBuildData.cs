using System.Collections.Generic;
using UnityEditor;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// AssetBundle构建数据
    /// </summary>
    public class ABBuildData
    {
        /// <summary>
        /// AssetBundle名, 全小写
        /// </summary>
        public string ABName => PathUtil.ABNameUnify(abName);

        private string abName;

        /// <summary>
        /// 资源路径列表
        /// </summary>
        public List<string> AssetPathList { get; set; }

        /// <summary>
        /// 转AssetBundleBuild结构
        /// </summary>
        public AssetBundleBuild ToAssetBundleBuild()
        {
            return new AssetBundleBuild()
            {
                assetBundleName = ABName,
                assetNames = AssetPathList == null ? new string[0] : AssetPathList.ToArray()
            };
        }

        /// <summary>
        /// 资源列表是否为空
        /// </summary>
        public bool IsEmpty()
        {
            return AssetPathList == null || AssetPathList.Count == 0;
        }

        public ABBuildData(string abName)
        {
            this.abName = PathUtil.ABNameUnify(abName);
        }
    }
}
