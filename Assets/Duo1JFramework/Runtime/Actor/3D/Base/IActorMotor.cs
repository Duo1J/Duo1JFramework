using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色移动马达
    /// </summary>
    public interface IActorMotor
    {
        /// <summary>
        /// 当前速度
        /// </summary>
        Vector3 Velocity { get; }

        /// <summary>
        /// 是否着地
        /// </summary>
        bool Grounded { get; }

        /// <summary>
        /// 设置移动
        /// </summary>
        void Move(Vector3 moveDir, float speed);

        /// <summary>
        /// 跳跃
        /// </summary>
        void Jump(float height);

        /// <summary>
        /// 停止移动
        /// </summary>
        void Stop();
    }
}
