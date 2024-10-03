using Duo1JFramework.FSM;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色控制器基类 - FSM
    /// </summary>
    public abstract partial class BaseActorController
    {
        /// <summary>
        /// 有限状态机
        /// </summary>
        protected StateMachine fsm;

        /// <summary>
        /// 当前状态机状态
        /// </summary>
        public string CurState { get; private set; }

        /// <summary>
        /// 初始化状态机
        /// </summary>
        public void InitFSM(string curStateName, params IStateNode[] stateList)
        {
            fsm = StateMachine.Create(ToString(), curStateName, stateList);
            CurState = curStateName;
        }

        /// <summary>
        /// 添加状态节点
        /// </summary>
        public bool AddFSMNode(IStateNode stateNode)
        {
            if (!CheckFSM())
            {
                Log.ErrorForce($"{ToString()} 状态机未初始化");
                return false;
            }

            return fsm.AddNode(stateNode);
        }

        /// <summary>
        /// 移除状态节点
        /// </summary>
        public bool RemoveFSMNode(string stateName)
        {
            if (!CheckFSM())
            {
                Log.ErrorForce($"{ToString()} 状态机未初始化");
                return false;
            }

            return fsm.RemoveNode(stateName);
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        public void SwitchState(string stateName, bool ignoreNextTick = true)
        {
            if (!CheckFSM())
            {
                Log.ErrorForce($"{ToString()} 状态机未初始化");
                return;
            }

            if (fsm.SwitchState(stateName, ignoreNextTick))
            {
                CurState = stateName;
            }
        }

        /// <summary>
        /// 强制切换状态
        /// </summary>
        public void ForceSwitchState(string stateName, bool ignoreNextTick = true)
        {
            if (!CheckFSM())
            {
                Log.ErrorForce($"{ToString()} 状态机未初始化");
                return;
            }

            if (fsm.ForceSwitchState(stateName, ignoreNextTick))
            {
                CurState = stateName;
            }
        }

        /// <summary>
        /// 是否处在状态
        /// </summary>
        public bool InState(string stateName)
        {
            if (!CheckFSM())
            {
                Log.ErrorForce($"{ToString()} 状态机未初始化");
                return false;
            }

            return fsm.InState(stateName);
        }

        /// <summary>
        /// 检查状态机是否初始化
        /// </summary>
        public bool CheckFSM()
        {
            return fsm != null;
        }

        /// <summary>
        /// 更新状态机
        /// </summary>
        private void UpdateFSM()
        {
            fsm?.Tick();
        }
    }
}
