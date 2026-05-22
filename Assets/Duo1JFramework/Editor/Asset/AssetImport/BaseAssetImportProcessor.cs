using System;
using UnityEditor;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源导入处理器基类
    /// </summary>
    public abstract class BaseAssetImportProcessor
    {
        /// <summary>
        /// 支持处理的导入器类型
        /// </summary>
        public abstract Type ImporterType { get; }

        /// <summary>
        /// 处理器优先级，数值越小越优先
        /// </summary>
        public virtual int Priority => 0;

        /// <summary>
        /// 是否可以处理该资源导入
        /// </summary>
        public virtual bool CanProcess(AssetImporter importer, string assetPath)
        {
            return importer != null && ImporterType.IsInstanceOfType(importer);
        }

        /// <summary>
        /// 处理资源导入
        /// </summary>
        public void Process(AssetImporter importer, string assetPath)
        {
            OnProcess(importer, assetPath);
        }

        protected abstract void OnProcess(AssetImporter importer, string assetPath);
    }
}
