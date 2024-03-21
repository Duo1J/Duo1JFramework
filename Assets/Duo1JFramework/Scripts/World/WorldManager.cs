using UnityEngine;

namespace Duo1JFramework.World
{
    /// <summary>
    /// 世界场景管理器
    /// </summary>
    public class WorldManager : MonoSingleton<WorldManager>
    {
        private Transform actorRoot;

        /// <summary>
        /// 获取Actor根节点
        /// </summary>
        public Transform GetActorRoot()
        {
            if (actorRoot == null)
            {
                GameObject go = new GameObject("ActorRoot");
                go.ResetSRT();
                actorRoot = go.transform;
            }
            return actorRoot;
        }

        protected override void OnDispose()
        {
        }

        protected override void OnInit()
        {
        }
    }
}