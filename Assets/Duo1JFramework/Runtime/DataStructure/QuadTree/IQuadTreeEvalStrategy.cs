namespace Duo1JFramework.DataStructure
{
    /// <summary>
    /// 四叉树评估策略接口
    /// </summary>
    public interface IQuadTreeEvalStrategy
    {
        /// <summary>
        /// 评估节点是否激活
        /// </summary>
        bool Evaluate(IQuadTreeNode node, object param);
    }
}
