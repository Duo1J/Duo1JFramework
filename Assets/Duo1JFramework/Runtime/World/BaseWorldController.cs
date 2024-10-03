using UnityEngine;

namespace Duo1JFramework.World
{
    /// <summary>
    /// 世界场景控制器基类
    /// </summary>
    public abstract class BaseWorldController : BaseMono
    {
        /// <summary>
        /// 世界场景数据
        /// </summary>
        public WorldData WorldData { get; private set; }

        /// <summary>
        /// 世界场景预制体
        /// </summary>
        public GameObject Asset
        {
            get
            {
                if (asset == null)
                {
                    asset = gameObject;
                }
                return asset;
            }
            private set
            {
                asset = value;
            }
        }
        private GameObject asset;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(WorldData worldData)
        {
            WorldData = worldData;

            OnSubInit();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Destroy()
        {
            Asset?.DestroyImmediate();
            Asset = null;

            OnSubDestroy();
        }

        /// <summary>
        /// 子类初始化
        /// </summary>
        public abstract void OnSubInit();

        /// <summary>
        /// 子类销毁
        /// </summary>
        public abstract void OnSubDestroy();
    }
}
