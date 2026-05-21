using System;
using UnityEngine.LowLevel;

namespace Duo1JFramework.Scheduling
{
    /// <summary>
    /// 用户循环体
    /// </summary>
    public class Loop : IDispose
    {
        /// <summary>
        /// 循环更新方法
        /// </summary>
        private PlayerLoopSystem.UpdateFunction updateFunction;

        /// <summary>
        /// 循环类型
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// 原始更新方法
        /// </summary>
        public PlayerLoopSystem.UpdateFunction UpdateFunction => updateFunction;

        /// <summary>
        /// 注入到PlayerLoop的更新方法
        /// </summary>
        public PlayerLoopSystem.UpdateFunction InjectedFunction => Run;

        /// <summary>
        /// 是否已销毁
        /// </summary>
        public bool Disposed { get; private set; }

        public Loop(Type type, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            Assert.NotNullArg(type, "type");
            Assert.NotNullArg(updateFunction, "updateFunction");

            Type = type;
            this.updateFunction = updateFunction;
        }

        public Loop(PlayerLoopSystem.UpdateFunction updateFunction) : this()
        {
            Assert.NotNullArg(updateFunction, "updateFunction");
            this.updateFunction = updateFunction;
        }

        private Loop()
        {
        }

        /// <summary>
        /// 循环运行
        /// </summary>
        public void Run()
        {
            updateFunction?.Invoke();
        }

        public void Dispose()
        {
            Disposed = true;
            updateFunction = null;
            Type = null;
        }
    }
}