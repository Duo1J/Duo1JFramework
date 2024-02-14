using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 角色Inspector面板控制参数
    /// </summary>
    public class ActorParam : MonoBehaviour
    {
        [Header("运动数值")]
        [Label("跑动速度")]
        public float moveSpeed = 3;
        [Label("步行速度")]
        public float walkSpeed = 1;
        [Label("旋转速度")]
        public float rotateSpeed = 5;
        [Label("跳跃力度")]
        public float jumpForce = 250;
        [Label("跳跃高度")]
        public float jumpHeight = 2;
        [Label("爬坡限制角度")]
        public float maxSlopeAngle = 50;

        [Space]
        [Header("控制数值")]
        [Label("鼠标X轴灵敏度")]
        public float mouseSpeedX = 300;
        [Label("鼠标Y轴灵敏度")]
        public float mouseSpeedY = 300;
        [Label("坠落速度增益")]
        public float fallSpeedUp = 7;

        [Space]
        [Header("相机数值")]
        [Label("相机到Actor长度")]
        public float cameraToActorLen = 5;
        [Label("相机最大Y旋转")]
        public float cameraMaxRotate = 70;
        [Label("相机最小Y旋转")]
        public float cameraMinRotate = -50;

        [Space]
        [Header("动画")]
        public string idleAniName = "Idle";
        public string walkAniName = "Walking";
        public string runAniName = "Running";
        public string jumpAniName = "Jump";

        [Space]
        [Header("射线")]
        [Label("触地检测射线长度")]
        public float rayGroundLen = 0.1f;
        [Label("触地检测射线半径")]
        public float rayGroundRadius = 0.3f;
        [Label("触地检测射线Y偏移")]
        public float rayGroundOffsetY = 0.05f;
    }
}