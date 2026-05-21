using UnityEngine;

namespace Duo1JFramework.Bake
{
    /// <summary>
    /// 烘焙策略
    /// </summary>
    public class BakeStrategy : EditorConfigSO<BakeStrategy>
    {
        [Label("清理烘焙数据")]
        [SerializeField]
        private bool clearBakeData = true;

        /// <summary>
        /// 是否清理烘焙数据
        /// </summary>
        public bool ClearBakeData => clearBakeData;

        [Header("烘焙场景")]
        [SerializeField]
        private BakeSceneData[] sceneDatas;

        /// <summary>
        /// 烘焙场景数据
        /// </summary>
        public BakeSceneData[] SceneDatas => sceneDatas;
    }
}
