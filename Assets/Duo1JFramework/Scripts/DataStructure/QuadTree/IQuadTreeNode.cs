namespace Duo1JFramework.DataStructure
{
    /// <summary>
    /// 四叉树接口
    /// </summary>
    public interface IQuadTreeNode
    {
        /// <summary>
        /// 添加对象
        /// </summary>
        void AddItem(QuadTreeItem item);

        /// <summary>
        /// 移除对象
        /// </summary>
        bool RemoveItem(QuadTreeItem item);

        /// <summary>
        /// 检测评估
        /// </summary>
        void Evaluate(object param = null);
    }
}