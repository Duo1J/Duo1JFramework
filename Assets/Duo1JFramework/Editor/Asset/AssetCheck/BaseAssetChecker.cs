using System;

using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源检查器基类
    /// </summary>
    public abstract class BaseAssetChecker
    {
        /// <summary>
        /// 支持检查的资源类型
        /// </summary>
        public abstract Type AssetType { get; }

        /// <summary>
        /// 检查器优先级，数值越小越优先
        /// </summary>
        public virtual int Priority => 0;

        /// <summary>
        /// 是否可以检查该资源
        /// </summary>
        public virtual bool CanCheck(UObject asset, string assetPath)
        {
            return asset != null && AssetType.IsInstanceOfType(asset);
        }

        /// <summary>
        /// 检查资源
        /// </summary>
        public AssetCheckResult Check(UObject asset, string assetPath)
        {
            AssetCheckResult result = new AssetCheckResult();
            OnCheck(asset, assetPath, result);
            return result;
        }

        /// <summary>
        /// 输出普通信息
        /// </summary>
        protected void OutputInfo(AssetCheckResult result, string reason)
        {
            result?.AddInfo(reason);
        }

        /// <summary>
        /// 输出报错信息
        /// </summary>
        protected void OutputError(AssetCheckResult result, string reason)
        {
            result?.AddError(reason);
        }

        protected abstract void OnCheck(UObject asset, string assetPath, AssetCheckResult result);
    }
}
