namespace Duo1JFramework.FSM
{
    /// <summary>
    /// 基础有限状态机节点
    /// </summary>
    public abstract class BaseStateNode : IStateNode
    {
        /// <summary>
        /// 归属状态机
        /// </summary>
        public StateMachine FSM { get; set; }

        /// <summary>
        /// 状态名
        /// </summary>
        public abstract string StateName { get; }

        /// <summary>
        /// 状态进入
        /// </summary>
        public virtual void StateEnter(object param)
        {
        }

        /// <summary>
        /// 状态退出
        /// </summary>
        public virtual void StateExit(object param)
        {
        }

        /// <summary>
        /// 状态更新
        /// </summary>
        public virtual void StateTick()
        {
        }

        /// <summary>
        /// 是否可切换状态至
        /// </summary>
        public virtual bool CanSwitchTo(string tarStateName)
        {
            return true;
        }

        /// <summary>
        /// 检查是否已满足可切换条件
        /// </summary>
        public virtual bool CheckSwitchCon()
        {
            return true;
        }
    }
}
