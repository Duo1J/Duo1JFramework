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

        /// <summary>
        /// 构建管线类型
        /// </summary>
        public EABPipelineType PipelineType
        {
            get => pipelineType;
            set => pipelineType = value;
        }

        [Label("构建目标")]
        [SerializeField]
        private BuildTarget buildTarget = BuildTarget.StandaloneWindows64;

        /// <summary>
        /// 构建目标平台
        /// </summary>
        public BuildTarget BuildTarget
        {
            get => buildTarget;
            set => buildTarget = value;
        }

        [Label("构建选项")]
        [SerializeField]
        private BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.ChunkBasedCompression;

        /// <summary>
        /// 构建选项
        /// </summary>
        public BuildAssetBundleOptions BuildOptions
        {
            get => buildOptions;
            set => buildOptions = value;
        }

        [Space]
        [Label("AB包命名方式")]
        [SerializeField]
        private EABNameType abNameType = EABNameType.Hash;

        /// <summary>
        /// AB包命名方式
        /// </summary>
        public EABNameType ABNameType
        {
            get => abNameType;
            set => abNameType = value;
        }

        [Label("构建CRC校验")]
        [SerializeField]
        private bool buildABCRC = true;

        /// <summary>
        /// 是否构建CRC校验
        /// </summary>
        public bool BuildABCRC
        {
            get => buildABCRC;
            set => buildABCRC = value;
        }

        [Header("构建策略")]
        [SerializeField]
        private ABBuildStrategyData[] data;

        /// <summary>
        /// 构建策略数据
        /// </summary>
        public ABBuildStrategyData[] Data => data;

        /// <summary>
        /// 设置ABMapData的数据
        /// </summary>
        public void SetToABMapData(ABMapData abMapData)
        {
            abMapData
                .SetABNameType(ABNameType)
                .SetBuildABCRC(BuildABCRC);
        }
    }

    /// <summary>
    /// 构建策略数据
    /// </summary>
    [Serializable]
    public class ABBuildStrategyData
    {
        [Label("AB包名")]
        public string abName;

        [Label("目标文件夹路径 (Res下)")]
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
