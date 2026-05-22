namespace Duo1JFramework.World
{
    /// <summary>
    /// 世界场景物体基类
    /// </summary>
    public abstract class BaseWorldItem : MonoRegister, IWorldItemLifecycle
    {
        /// <summary>
        /// 逻辑是否激活
        /// </summary>
        protected bool LogicActive
        {
            get => logicActive;
            set
            {
                if (logicActive == value)
                {
                    return;
                }

                logicActive = value;
                OnLogicActiveChanged(logicActive);
            }
        }

        private bool logicActive = true;

        /// <summary>
        /// 进入世界
        /// </summary>
        public virtual void OnWorldEnter(BaseWorldController controller)
        {
        }

        /// <summary>
        /// 退出世界
        /// </summary>
        public virtual void OnWorldExit(BaseWorldController controller)
        {
        }

        /// <summary>
        /// 世界暂停
        /// </summary>
        public virtual void OnWorldPause(BaseWorldController controller)
        {
        }

        /// <summary>
        /// 世界恢复
        /// </summary>
        public virtual void OnWorldResume(BaseWorldController controller)
        {
        }

        /// <summary>
        /// 逻辑激活状态改变
        /// </summary>
        public virtual void OnLogicActiveChanged(bool active)
        {
        }
    }
}
