namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色逻辑基类 - 子类实现
    /// </summary>
    public abstract partial class BaseActor
    {
        /// <summary>
        /// 子类初始化
        /// </summary>
        protected virtual void OnInit()
        {
        }

        /// <summary>
        /// 创建前
        /// </summary>
        protected virtual void BeforeCreate()
        {
        }

        /// <summary>
        /// 创建完成后
        /// </summary>
        protected virtual void OnCreated()
        {
        }

        /// <summary>
        /// 卸载资源前
        /// </summary>
        protected virtual void BeforeUnLoadAsset()
        {
        }

        /// <summary>
        /// 卸载资源后
        /// </summary>
        protected virtual void AfterUnLoadAsset()
        {
        }

        /// <summary>
        /// 销毁
        /// </summary>
        protected override void OnDispose()
        {
            UnLoadAsset();
        }
    }
}