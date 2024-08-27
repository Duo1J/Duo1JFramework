using UnityEngine;

namespace Duo1JFramework.DataStructure
{
    /// <summary>
    /// 四叉树管理对象接口
    /// </summary>
    public interface IQuadTreeItem
    {
        /// <summary>
        /// 包围盒
        /// </summary>
        Bounds Bounds { get; }

        /// <summary>
        /// 四叉树节点是否激活
        /// </summary>
        bool QuadActive { get; }

        /// <summary>
        /// 设置四叉树节点状态
        /// </summary>
        void SetQuadState(bool quadActive);

        /// <summary>
        /// 四叉树节点触发
        /// </summary>
        void TriggerQuad();
    }
}