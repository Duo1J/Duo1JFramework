namespace Duo1JFramework.World
{
    /// <summary>
    /// 世界物体生命周期接口
    /// </summary>
    public interface IWorldItemLifecycle
    {
        /// <summary>
        /// 进入世界
        /// </summary>
        void OnWorldEnter(BaseWorldController controller);

        /// <summary>
        /// 退出世界
        /// </summary>
        void OnWorldExit(BaseWorldController controller);

        /// <summary>
        /// 世界暂停
        /// </summary>
        void OnWorldPause(BaseWorldController controller);

        /// <summary>
        /// 世界恢复
        /// </summary>
        void OnWorldResume(BaseWorldController controller);

        /// <summary>
        /// 逻辑激活状态改变
        /// </summary>
        void OnLogicActiveChanged(bool active);
    }
}
