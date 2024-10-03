namespace Duo1JFramework.World
{
    /// <summary>
    /// 世界场景物体基类
    /// </summary>
    public abstract class BaseWorldItem : MonoRegister
    {
        /// <summary>
        /// 逻辑是否激活
        /// </summary>
        protected bool LogicActive { get; set; } = true;
    }
}
