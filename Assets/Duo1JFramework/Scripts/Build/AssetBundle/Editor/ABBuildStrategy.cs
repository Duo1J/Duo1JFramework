using System;
using System.IO;
using UnityEngine;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// AssetBundle构建策略
    /// </summary>
    public class ABBuildStrategy : EditorConfigSO<ABBuildStrategy>
    {
        [SerializeField]
        private ABBuildStrategyData[] data;

        public ABBuildStrategyData[] Data => data;
    }

    [Serializable]
    public class ABBuildStrategyData
    {
        [Label("AB包名")]
        public string abName;

        [Label("目标路径 (Res下)")]
        public string[] pathList;

        /// <summary>
        /// 检查是否有效
        /// </summary>
        public bool CheckValiad()
        {
            return !string.IsNullOrEmpty(abName) &&
                    pathList != null &&
                    pathList.Length != 0;
        }
    }
}