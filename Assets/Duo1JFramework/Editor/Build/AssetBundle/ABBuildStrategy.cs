using System;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// AssetBundle构建策略
    /// </summary>
    public class ABBuildStrategy : EditorConfigSO<ABBuildStrategy>
    {
        [Label("管线类型")]
        [SerializeField]
        private EABPipelineType pipelineType = EABPipelineType.Builtin;
        public EABPipelineType PipelineType { get => pipelineType; set => pipelineType = value; }

        [Label("构建目标")]
        [SerializeField]
        private BuildTarget buildTarget = BuildTarget.StandaloneWindows64;
        public BuildTarget BuildTarget { get => buildTarget; set => buildTarget = value; }

        [Label("构建选项")]
        [SerializeField]
        private BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.ChunkBasedCompression;
        public BuildAssetBundleOptions BuildOptions { get => buildOptions; set => buildOptions = value; }

        [SerializeField]
        private ABBuildStrategyData[] data;

        /// <summary>
        /// AssetBundle构建策略数据
        /// </summary>
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
