using System;
using System.Linq;

namespace Duo1JFramework.FSM
{
    /// <summary>
    /// 有限状态机状态节点
    /// </summary>
    public class StateNode
    {
        /// <summary>
        /// 状态名
        /// </summary>
        public string StateName { get; set; }

        private Action stateEnter;
        private Action stateTick;
        private Action stateExit;

        /// <summary>
        /// 可切换状态列表
        /// </summary>
        private string[] switchList;

        /// <summary>
        /// 创建
        /// </summary>
        public static StateNode Create(string stateName, Action stateEnter, Action stateTick, Action stateExit)
        {
            return new StateNode(stateName, stateEnter, stateTick, stateExit);
        }

        /// <summary>
        /// 设置可切换状态列表，为空则表示可任意切换
        /// </summary>
        public StateNode SetSwitchList(params string[] switchList)
        {
            this.switchList = switchList;
            return this;
        }

        /// <summary>
        /// 是否可切换状态至
        /// </summary>
        public bool CanSwitchTo(string tarStateName)
        {
            if (switchList == null || switchList.Length == 0)
                return true;
            return switchList.Contains(tarStateName);
        }

        /// <summary>
        /// 状态进入
        /// </summary>
        public void StateEnter()
        {
            stateEnter?.Invoke();
        }

        /// <summary>
        /// 状态更新
        /// </summary>
        public void StateTick()
        {
            stateTick?.Invoke();
        }

        /// <summary>
        /// 状态退出
        /// </summary>
        public void StateExit()
        {
            stateExit?.Invoke();
        }

        public StateNode(string stateName, Action stateEnter, Action stateTick, Action stateExit)
        {
            this.StateName = stateName;
            this.stateEnter = stateEnter;
            this.stateTick = stateTick;
            this.stateExit = stateExit;
        }
    }
}