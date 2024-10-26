using System;
using System.Diagnostics;
using System.IO;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// Assetbundle映射资源数据
    /// </summary>
    [Serializable]
    public class ABMapAssetData
    {
        /// <summary>
        /// 资源路径
        /// </summary>
        public string AssetPath { get; private set; }

        /// <summary>
        /// 所属AssetBundle名
        /// </summary>
        public string ABName { get; private set; }

        /// <summary>
        /// 资源MD5
        /// </summary>
        public string MD5 { get; private set; }

        public ABMapAssetData(string assetPath, string abName)
        {
            AssetPath = assetPath;
            ABName = abName;

            UpdateAssetInfo();
        }

        /// <summary>
        /// 更新资源信息
        /// </summary>
        [Conditional("UNITY_EDITOR")]
        public void UpdateAssetInfo()
        {
            FileInfo fileInfo = new FileInfo(AssetPath);
            if (!fileInfo.Exists)
            {
                Log.ErrorForce($"更新资源信息时, 未找到资源文件: `{AssetPath}`");
                MD5 = null;
                return;
            }

            using (FileStream stream = fileInfo.OpenRead())
            {
                MD5 = CryptoUtil.MD5ComputeHashStr(stream);
            }
        }
    }
}
