using System.Text;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色控制器基类 - 子类实现
    /// </summary>
    public abstract partial class BaseActorController
    {
        /// <summary>
        /// 子类实现初始化组件
        /// </summary>
        protected abstract void OnInitComponent();

        /// <summary>
        /// 子类实现收集组件
        /// </summary>
        protected abstract void OnCollectComponent();

        /// <summary>
        /// 子类实现Update
        /// </summary>
        protected virtual void OnUpdateSub()
        {
        }

        /// <summary>
        /// 子类实现FixedUpdate
        /// </summary>
        protected virtual void OnFixedUpdateSub()
        {
        }

        /// <summary>
        /// 子类实现获取Hierarchy显示信息
        /// </summary>
        protected virtual void GetHierarchyInfoSub(StringBuilder sb)
        {
        }
    }
}
