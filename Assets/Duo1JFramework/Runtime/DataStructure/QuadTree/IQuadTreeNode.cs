using UnityEngine;

namespace Duo1JFramework.DataStructure
{
    /// <summary>
    /// 四叉树节点接口
    /// </summary>
    public interface IQuadTreeNode
    {
        /// <summary>
        /// 包围盒
        /// </summary>
        Bounds Bounds { get; }

        /// <summary>
        /// 添加对象
        /// </summary>
        void AddItem(IQuadTreeItem item);

        /// <summary>
        /// 移除对象
        /// </summary>
        bool RemoveItem(IQuadTreeItem item);

        /// <summary>
        /// 更新对象
        /// </summary>
        void UpdateItem(IQuadTreeItem item);

        /// <summary>
        /// 检测评估
        /// </summary>
        void Evaluate(object param = null);
    }
}
