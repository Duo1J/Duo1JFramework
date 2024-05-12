using Duo1JFramework.World;
using UnityEngine;

namespace Duo1JFramework.DataStructure
{
    /// <summary>
    /// 四叉树管理对象抽象
    /// </summary>
    public abstract class QuadTreeItem : BaseWorldItem
    {
        /// <summary>
        /// 是否激活
        /// </summary>
        protected bool active;

        /// <summary>
        /// 位置
        /// </summary>
        public abstract Vector3 Pos { get; }

        /// <summary>
        /// 设置状态
        /// </summary>
        public void SetState(bool active)
        {
            this.active = active;
        }

        /// <summary>
        /// 触发
        /// </summary>
        public abstract void Trigger();
    }
}