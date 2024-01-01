using System.Collections.Generic;

namespace Duo1JFramework.FSM
{
    /// <summary>
    /// 有限状态机
    /// </summary>
    public class StateMachine
    {
        /// <summary>
        /// 状态列表
        /// </summary>
        private Dictionary<string, StateNode> stateDict;

        /// <summary>
        /// 当前状态
        /// </summary>
        private StateNode curState;

        public static StateMachine Create(string curStateName, params StateNode[] stateList)
        {
            StateMachine fsm = new StateMachine();
            fsm.Init(curStateName, stateList);
            return fsm;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(string curStateName, params StateNode[] stateList)
        {
            curState = null;
            stateDict = new Dictionary<string, StateNode>();
            foreach (StateNode node in stateList)
            {
                if (string.IsNullOrEmpty(node.StateName))
                {
                    Log.ErrorForce($"FSM不可使用空的状态名");
                    Dispose();
                    return;
                }
                if (stateDict.ContainsKey(node.StateName))
                {
                    Log.ErrorForce($"FSM不可使用相同的状态名: {node.StateName}");
                    Dispose();
                    return;
                }
                if (node.StateName.Equals(curStateName))
                {
                    curState = node;
                }
                stateDict.Add(node.StateName, node);
            }
            if (curState == null)
            {
                Log.ErrorForce($"FSM未找到当前执行状态: {curStateName}");
                Dispose();
                return;
            }

            curState.StateEnter();
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        public bool SwitchState(string stateName)
        {
            if (stateDict.TryGetValue(stateName, out StateNode state))
            {
                curState.StateExit();
                curState = state;
                curState.StateEnter();
                return true;
            }
            else
            {
                Log.ErrorForce($"FSM未找到状态{stateName}，无法切换");
                return false;
            }
        }

        /// <summary>
        /// 是否处在状态
        /// </summary>
        public bool InState(string stateName)
        {
            return curState != null && curState.StateName.Equals(stateName);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Tick()
        {
            if (curState == null)
                return;
            curState.StateTick();
        }

        public void Dispose()
        {
            curState = null;
            stateDict = null;
        }

        ~StateMachine()
        {
            Dispose();
        }
    }
}