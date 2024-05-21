namespace Duo1JFramework.FSM
{
    /// <summary>
    /// 状态机节点接口
    /// </summary>
    public interface IStateNode
    {
        /// <summary>
        /// 归属状态机
        /// </summary>
        StateMachine FSM { get; set; }

        /// <summary>
        /// 状态名
        /// </summary>
        string StateName { get; }

        /// <summary>
        /// 状态进入
        /// </summary>
        void StateEnter(object param);

        /// <summary>
        /// 状态退出
        /// </summary>
        void StateExit(object param);

        /// <summary>
        /// 状态更新
        /// </summary>
        void StateTick();

        /// <summary>
        /// 是否可切换状态至
        /// </summary>
        bool CanSwitchTo(string tarStateName);

        /// <summary>
        /// 检查是否已满足可切换条件
        /// </summary>
        bool CheckSwitchCon();
    }
}