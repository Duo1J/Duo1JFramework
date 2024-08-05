namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源加载方式
    /// </summary>
    public enum EAssetLoadType
    {
        /// <summary>
        /// 使用当前设置的资源包类型加载
        /// </summary>
        Bundle,

        /// <summary>
        /// 使用AssetBundle资源包加载
        /// </summary>
        AssetBundle,

        /// <summary>
        /// 使用Addressables资源包加载
        /// </summary>
        Addressables,

        /// <summary>
        /// 使用Resources加载
        /// </summary>
        Resources,
    }
}