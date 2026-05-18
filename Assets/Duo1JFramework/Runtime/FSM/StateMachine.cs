using System.Collections.Generic;
using System.Linq;

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
        /// 当前状态名
        /// </summary>
        public string CurrentStateName => curState?.StateName;

        /// <summary>
        /// 是否已释放
        /// </summary>
        public bool Disposed { get; private set; }

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
            Disposed = false;

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

            SafeStateEnter(curState, null);
        }

        /// <summary>
        /// 添加状态节点
        /// </summary>
        public bool AddNode(IStateNode stateNode)
        {
            if (CheckDisposed())
            {
                return false;
            }

            if (stateNode == null)
            {
                Log.ErrorForce($"{ToString()} 不可添加空状态节点");
                return false;
            }

            if (string.IsNullOrEmpty(stateNode.StateName))
            {
                Log.ErrorForce($"{ToString()} 不可使用空状态名");
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
            if (CheckDisposed())
            {
                return false;
            }

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

            if (CheckDisposed())
            {
                return false;
            }

            if (curState != null)
            {
                if (!curState.CanSwitchTo(stateName))
                {
                    Log.Warn($"{ToString()} 当前状态 `{curState.StateName}` 不允许切换到 `{stateName}`，可用状态: {GetStateNameListText()}");
                    return false;
                }
                if (!curState.CheckSwitchCondition())
                {
                    Log.Warn($"{ToString()} 当前状态 `{curState.StateName}` 未满足切换到 `{stateName}` 的条件");
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

            if (CheckDisposed())
            {
                return false;
            }

            if (InState(stateName))
                return false;

            if (stateDict.TryGetValue(stateName, out IStateNode state))
            {
                SafeStateExit(curState, param);
                curState = state;
                SafeStateEnter(curState, param);
                if (ignoreNextTick)
                    this.ignoreNextTick = true;
                return true;
            }
            else
            {
                Log.ErrorForce($"{ToString()} 未找到状态 `{stateName}`，无法切换。当前状态: `{CurrentStateName}`，可用状态: {GetStateNameListText()}");
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
            if (Disposed || curState == null)
            {
                return;
            }

            if (ignoreNextTick)
            {
                ignoreNextTick = false;
                return;
            }

            SafeStateTick(curState);
        }

        public void Dispose()
        {
            if (Disposed)
            {
                return;
            }

            Disposed = true;
            curState = null;
            stateDict = null;
        }

        private bool CheckDisposed()
        {
            if (!Disposed)
            {
                return false;
            }

            Log.ErrorForce($"{ToString()} 已释放");
            return true;
        }

        private string GetStateNameListText()
        {
            if (stateDict == null || stateDict.Count == 0)
            {
                return "<Empty>";
            }

            return string.Join(", ", stateDict.Keys.ToArray());
        }

        private void SafeStateEnter(IStateNode stateNode, object param)
        {
            try
            {
                stateNode?.StateEnter(param);
            }
            catch (System.Exception e)
            {
                Assert.ExceptHandle(e, $"{ToString()} 状态 `{stateNode?.StateName}` 进入时异常");
            }
        }

        private void SafeStateTick(IStateNode stateNode)
        {
            try
            {
                stateNode?.StateTick();
            }
            catch (System.Exception e)
            {
                Assert.ExceptHandle(e, $"{ToString()} 状态 `{stateNode?.StateName}` 更新时异常");
            }
        }

        private void SafeStateExit(IStateNode stateNode, object param)
        {
            try
            {
                stateNode?.StateExit(param);
            }
            catch (System.Exception e)
            {
                Assert.ExceptHandle(e, $"{ToString()} 状态 `{stateNode?.StateName}` 退出时异常");
            }
        }

        private StateMachine()
        {
        }

        public override string ToString()
        {
            return $"<FSM: {Name}>";
        }
    }
}
