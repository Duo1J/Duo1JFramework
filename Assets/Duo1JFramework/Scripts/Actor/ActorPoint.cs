using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色挂点
    /// </summary>
    public class ActorPoint : MonoBehaviour
    {
        /// <summary>
        /// 根位置
        /// </summary>
        public Transform root;

        /// <summary>
        /// 自动匹配节点
        /// </summary>
        public void AutoMatch()
        {
            root = transform;
        }

        private void Awake()
        {
            if (root == null)
                root = transform;
        }
    }
}