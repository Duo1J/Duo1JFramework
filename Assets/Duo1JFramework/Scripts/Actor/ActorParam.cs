using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 角色Inspector面板控制参数
    /// </summary>
    public class ActorParam : MonoBehaviour
    {
        [Header("运动数值")]
        public float moveSpeed = 3;
        public float jumpForce = 250;
        public float rotateSpeed = 5;
        public float jumpHeight = 2;

        [Space]
        [Header("控制数值")]
        public float mouseSpeedX = 300;
        public float mouseSpeedY = 300;
        public float fallSpeedUp = 7;

        [Space]
        [Header("相机数值")]
        public float cameraToActorLen = 5;
        public float cameraMaxRotate = 70;
        public float cameraMinRotate = -50;

        [Space]
        [Header("动画")]
        public string idleAniName = "Idle";
        public string runAniName = "Running";
        public string jumpAniName = "Jump";

        [Space]
        [Header("射线")]
        [Header("触地检测射线长度")]
        public float rayGroundLen = 0.1f;
        [Header("触地检测射线半径")]
        public float rayGroundRadius = 0.3f;
        [Header("触地检测射线Y偏移")]
        public float rayGroundOffsetY = 0.05f;
    }
}