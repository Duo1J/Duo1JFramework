using System;
using System.Linq;
using UnityEngine;

namespace Duo1JFramework.FSM
{
    /// <summary>
    /// 有限状态机状态节点
    /// </summary>
    public class StateNode
    {
        /// <summary>
        /// 归属状态机
        /// </summary>
        public StateMachine FSM { get; set; }

        /// <summary>
        /// 状态名
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public float StartTime { get; private set; }

        /// <summary>
        /// 最大运行时间
        /// </summary>
        public float MaxTime { get; private set; } = -1;

        /// <summary>
        /// 最小运行时间
        /// </summary>
        public float MinTime { get; private set; } = 0;

        /// <summary>
        /// 最大时间转换状态名
        /// </summary>
        public string MaxTimeChgStateName { get; private set; }

        private Action stateEnter;
        private Action stateTick;
        private Action stateExit;

        /// <summary>
        /// 可切换状态列表
        /// </summary>
        private string[] switchList;

        /// <summary>
        /// 内部计算时间获取
        /// </summary>
        protected float CurTime => Time.time;

        /// <summary>
        /// 创建
        /// </summary>
        public static StateNode Create(string stateName, Action stateEnter = null, Action stateTick = null, Action stateExit = null)
        {
            return new StateNode(stateName, stateEnter, stateTick, stateExit);
        }

        /// <summary>
        /// 运行X时间(s)后切换至其他状态
        /// </summary>
        public StateNode TimeToState(float maxTime, string stateName)
        {
            Assert.NotNullOrEmpty(stateName, $"{ToString()} 状态名不可为空");

            if (!CanSwitchTo(stateName))
            {
                Log.ErrorForce($"{ToString()} 不可切换至状态: {stateName}");
                return this;
            }

            MaxTime = maxTime;
            MaxTimeChgStateName = stateName;
            return this;
        }

        /// <summary>
        /// 最少保持X时间(s)
        /// </summary>
        public StateNode TimeHold(float minTime)
        {
            MinTime = minTime;
            return this;
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
        /// 获取可切换状态列表
        /// </summary>
        public string[] GetSwitchList()
        {
            if (switchList == null)
            {
                return new string[0];
            }

            return switchList;
        }

        /// <summary>
        /// 检查是否已满足可切换条件
        /// </summary>
        public bool CheckSwitchCon()
        {
            if ((CurTime - StartTime) < MinTime)
            {
                return false;
            }

            return true;
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

        #region Inner

        /// <summary>
        /// 状态进入
        /// </summary>
        public void StateEnter()
        {
            StartTime = CurTime;
            stateEnter?.Invoke();
        }

        /// <summary>
        /// 状态更新
        /// </summary>
        public void StateTick()
        {
            stateTick?.Invoke();

            if (MaxTime > 0 && (CurTime - StartTime) >= MaxTime)
            {
                FSM.SwitchState(MaxTimeChgStateName, true);
            }
        }

        /// <summary>
        /// 状态退出
        /// </summary>
        public void StateExit()
        {
            stateExit?.Invoke();
        }

        #endregion Inner

        public StateNode(string stateName, Action stateEnter = null, Action stateTick = null, Action stateExit = null)
        {
            this.StateName = stateName;
            this.stateEnter = stateEnter;
            this.stateTick = stateTick;
            this.stateExit = stateExit;
        }

        public override string ToString()
        {
            return $"{FSM}[State: {StateName}]";
        }
    }
}