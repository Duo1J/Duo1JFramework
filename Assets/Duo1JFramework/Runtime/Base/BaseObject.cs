namespace Duo1JFramework
{
    /// <summary>
    /// 所有非Monobehaviour类的基类
    /// </summary>
    public class BaseObject
    {
        /// <summary>
        /// 资源加载集合
        /// </summary>
        public AssetCollection Asset
        {
            get
            {
                if (assetCollection == null)
                {
                    assetCollection = new AssetCollection();
                }

                if (IsSingleton)
                {
                    Log.Warn("单例使用 `AssetManager` 进行资源加载");
                }

                return assetCollection;
            }
        }

        private AssetCollection assetCollection;

        /// <summary>
        /// 是否是单例
        /// </summary>
        public virtual bool IsSingleton => false;

        ~BaseObject()
        {
            if (assetCollection != null)
            {
                assetCollection.Dispose();
                assetCollection = null;
            }
        }
    }
}
