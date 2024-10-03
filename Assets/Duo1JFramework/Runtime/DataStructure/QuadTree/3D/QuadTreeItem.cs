using Duo1JFramework.World;
using UnityEngine;

namespace Duo1JFramework.DataStructure
{
    /// <summary>
    /// 3D四叉树管理对象基类
    /// </summary>
    public abstract class QuadTreeItem : WorldItem, IQuadTreeItem
    {
        /// <summary>
        /// 四叉树节点是否激活
        /// </summary>
        public bool QuadActive
        {
            get => quadActive;
            protected set => quadActive = value;
        }

        [Label("四叉树状态")]
        [SerializeField]
        private bool quadActive;

        /// <summary>
        /// 包围盒
        /// </summary>
        public abstract Bounds Bounds { get; }

        /// <summary>
        /// 设置四叉树节点状态
        /// </summary>
        public void SetQuadState(bool quadActive)
        {
            QuadActive = quadActive;
        }

        /// <summary>
        /// 四叉树节点触发
        /// </summary>
        public abstract void TriggerQuad();
    }
}
