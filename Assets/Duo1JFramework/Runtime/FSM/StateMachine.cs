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
        /// 状态字典
        /// </summary>
        private Dictionary<string, IStateNode> stateDict;

        /// <summary>
        /// 当前状态
        /// </summary>
        private IStateNode curState;

        /// <summary>
        /// 忽略下次Tick
        /// </summary>
        private bool ignoreNextTick = false;

        /// <summary>
        /// 创建
        /// </summary>
        public static StateMachine Create(string fsmName, string curStateName, params IStateNode[] stateNodeList)
        {
            StateMachine fsm = new StateMachine();
            fsm.Init(fsmName, curStateName, stateNodeList);
            return fsm;
        }

        /// <summary>
        /// 创建 (无默认状态)
        /// </summary>
        public static StateMachine Create(string fsmName, params IStateNode[] stateNodeList)
        {
            return Create(fsmName, null, stateNodeList);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(string fsmName, string curStateName, params IStateNode[] stateNodeList)
        {
            Name = fsmName;

            curState = null;
            stateDict = new Dictionary<string, IStateNode>();

            foreach (IStateNode stateNode in stateNodeList)
            {
                if (!AddNode(stateNode))
                {
                    Dispose();
                    return;
                }

                if (curStateName != null && stateNode.StateName.Equals(curStateName))
                {
                    curState = stateNode;
                }
            }
            if (curStateName != null && curState == null)
            {
                Log.ErrorForce($"{ToString()} 未找到默认执行状态: `{curStateName}`");
                Dispose();
                return;
            }

            curState?.StateEnter(null);
        }

        /// <summary>
        /// 添加状态节点
        /// </summary>
        public bool AddNode(IStateNode stateNode)
        {
            if (string.IsNullOrEmpty(stateNode.StateName))
            {
                Log.ErrorForce($"{ToString()} 不可使用空的状态名");
                return false;
            }

            if (stateDict.ContainsKey(stateNode.StateName))
            {
                Log.ErrorForce($"{ToString()} 状态名重复: `{stateNode.StateName}`");
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
        public bool SwitchState(string stateName, object param = null, bool ignoreNextTick = true)
        {
            Assert.NotNullOrEmpty(stateName, $"{ToString()} 状态名不可为空");

            if (curState != null)
            {
                if (!curState.CanSwitchTo(stateName))
                {
                    return false;
                }
                if (!curState.CheckSwitchCondition())
                {
                    return false;
                }
            }

            return ForceSwitchState(stateName, param, ignoreNextTick);
        }

        /// <summary>
        /// 强制切换状态
        /// </summary>
        public bool ForceSwitchState(string stateName, object param = null, bool ignoreNextTick = true)
        {
            Assert.NotNullOrEmpty(stateName, $"{ToString()} 状态名不可为空");

            if (InState(stateName))
                return false;

            if (stateDict.TryGetValue(stateName, out IStateNode state))
            {
                curState?.StateExit(param);
                curState = state;
                curState.StateEnter(param);
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
            {
                return;
            }

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

        private StateMachine()
        {
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
