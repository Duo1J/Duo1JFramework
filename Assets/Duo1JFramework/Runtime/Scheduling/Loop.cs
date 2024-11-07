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
        /// 是否已销毁
        /// </summary>
        public bool Disposed { get; private set; }

        public Loop(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            Assert.NotNullArg(updateFunction, "updateFunction");
            this.updateFunction = updateFunction;
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
        }
    }
}