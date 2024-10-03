namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源加载器类型
    /// </summary>
    public enum EAssetLoaderType
    {
        /// <summary>
        /// 使用AssetDatabase加载
        /// </summary>
        AssetDatabase,

        /// <summary>
        /// 使用AssetBundle加载器加载
        /// </summary>
        AssetBundle,

        /// <summary>
        /// 使用Addressables加载器加载
        /// </summary>
        Addressables
    }
}
