using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色相机Rig
    /// </summary>
    public interface IActorCameraRig
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        bool Valid { get; }

        /// <summary>
        /// 相机跟随/注视点
        /// </summary>
        Transform CameraPoint { get; }

        /// <summary>
        /// 相机偏移
        /// </summary>
        Vector3 Offset { get; set; }

        /// <summary>
        /// 旋转相机
        /// </summary>
        void Rotate(Vector2 lookInput);

        /// <summary>
        /// 更新相机位置
        /// </summary>
        void UpdatePosition();
    }
}
