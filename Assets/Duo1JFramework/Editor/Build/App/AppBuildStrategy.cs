using Duo1JFramework.Asset;
using System;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// App构建策略
    /// </summary>
    public class AppBuildStrategy : EditorConfigSO<AppBuildStrategy>
    {
        [SerializeField]
        private AppBuildStrategyData data;

        /// <summary>
        /// App构建策略数据
        /// </summary>
        public AppBuildStrategyData Data => data;
    }

    [Serializable]
    public class AppBuildStrategyData
    {
        [Label("构建目标")]
        public BuildTarget buildTarget = BuildTarget.StandaloneWindows64;

        [Label("构建选项")]
        public BuildOptions buildOptions;

        [Label("构建资源")]
        public bool buildAsset = true;

        [Label("拷贝资源到运行时目录")]
        public bool copyAsset = true;

        [Label("删除Manifest文件")]
        public bool deleteManifest = true;

        [Label("资源加载器类型 (与GameOption对应)")]
        public EAssetLoaderType assetLoaderType = EAssetLoaderType.AssetBundle;
    }
}
