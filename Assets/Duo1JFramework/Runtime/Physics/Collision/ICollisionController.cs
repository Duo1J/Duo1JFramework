namespace Duo1JFramework.PhysicsAPI
{
    /// <summary>
    /// 碰撞控制器接口
    /// </summary>
    public interface ICollisionController : IEditorDrawer
    {
        /// <summary>
        /// 获取Go实例ID
        /// </summary>
        int GetInstanceID();

        /// <summary>
        /// 设置是否可用
        /// </summary>
        void SetEnable(bool enable);

        /// <summary>
        /// 设置碰撞、触发类型
        /// </summary>
        void SetCollisionType(CollisionType collisionType);
    }
}
