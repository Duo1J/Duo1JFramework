using System.Collections.Generic;

namespace Duo1JFramework.FSM
{
    /// <summary>
    /// 有限状态机
    /// </summary>
    public class StateMachine
    {
        /// <summary>
        /// 状态机名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态列表
        /// </summary>
        private Dictionary<string, StateNode> stateDict;

        /// <summary>
        /// 当前状态
        /// </summary>
        private StateNode curState;

        /// <summary>
        /// 忽略下次Tick
        /// </summary>
        private bool ignoreNextTick = false;

        public static StateMachine Create(string fsmName, string curStateName, params StateNode[] stateNodeList)
        {
            StateMachine fsm = new StateMachine();
            fsm.Init(fsmName, curStateName, stateNodeList);
            return fsm;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(string fsmName, string curStateName, params StateNode[] stateNodeList)
        {
            Name = fsmName;

            curState = null;
            stateDict = new Dictionary<string, StateNode>();

            foreach (StateNode stateNode in stateNodeList)
            {
                if (!AddNode(stateNode))
                {
                    Dispose();
                    return;
                }

                if (stateNode.StateName.Equals(curStateName))
                {
                    curState = stateNode;
                }
            }
            if (curState == null)
            {
                Log.ErrorForce($"{ToString()} 未找到当前执行状态: {curStateName}");
                Dispose();
                return;
            }

            curState.StateEnter();
        }

        /// <summary>
        /// 添加状态节点
        /// </summary>
        public bool AddNode(StateNode stateNode)
        {
            if (string.IsNullOrEmpty(stateNode.StateName))
            {
                Log.ErrorForce($"{ToString()} 不可使用空的状态名");
                return false;
            }

            if (stateDict.ContainsKey(stateNode.StateName))
            {
                Log.ErrorForce($"{ToString()} 不可使用相同的状态名: {stateNode.StateName}");
                return false;
            }

            stateNode.FSM = this;
            stateDict.Add(stateNode.StateName, stateNode);
            return true;
        }

        /// <summary>
        /// 移除状态节点
        /// </summary>
        public bool RemoveNode(string stateName)
        {
            if (!stateDict.ContainsKey(stateName))
            {
                Log.ErrorForce($"{ToString()} 未包含状态`{stateName}`，无法移除");
                return false;
            }

            if (InState(stateName))
            {
                Log.ErrorForce($"{ToString()} 处在状态`{stateName}`中，无法移除");
                return false;
            }

            return stateDict.Remove(stateName);
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        public bool SwitchState(string stateName, bool ignoreNextTick = true)
        {
            Assert.NotNullOrEmpty(stateName, $"{ToString()} 状态名不可为空");

            if (!curState.CanSwitchTo(stateName))
                return false;

            if (!curState.CheckSwitchCon())
                return false;

            return ForceSwitchState(stateName, ignoreNextTick);
        }

        /// <summary>
        /// 强制切换状态
        /// </summary>
        public bool ForceSwitchState(string stateName, bool ignoreNextTick = true)
        {
            Assert.NotNullOrEmpty(stateName, $"{ToString()} 状态名不可为空");

            if (InState(stateName))
                return false;

            if (stateDict.TryGetValue(stateName, out StateNode state))
            {
                curState.StateExit();
                curState = state;
                curState.StateEnter();
                if (ignoreNextTick)
                    this.ignoreNextTick = true;
                return true;
            }
            else
            {
                Log.ErrorForce($"{ToString()} 未找到状态{stateName}，无法切换");
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
            if (ignoreNextTick)
            {
                ignoreNextTick = false;
                return;
            }
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

        public override string ToString()
        {
            return $"<FSM: {Name}>";
        }
    }
}