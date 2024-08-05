namespace Duo1JFramework.World
{
    /// <summary>
    /// 基础世界场景物体
    /// </summary>
    public abstract class BaseWorldItem : MonoRegister
    {
        /// <summary>
        /// 逻辑是否激活
        /// </summary>
        protected bool LogicActive { get; set; } = true;
    }
}