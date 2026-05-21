using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色输入源
    /// </summary>
    public interface IActorInput
    {
        /// <summary>
        /// 移动输入
        /// </summary>
        Vector2 Move { get; }

        /// <summary>
        /// 视角输入
        /// </summary>
        Vector2 Look { get; }

        /// <summary>
        /// 是否步行
        /// </summary>
        bool Walk { get; }

        /// <summary>
        /// 跳跃按下
        /// </summary>
        bool JumpDown { get; }
    }
}
