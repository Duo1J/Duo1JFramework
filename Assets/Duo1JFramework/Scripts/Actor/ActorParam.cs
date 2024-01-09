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

        [Space]
        [Header("控制数值")]
        public float mouseSpeedX = 300;
        public float mouseSpeedY = 300;
        public float fallSpeedUp = 7;

        [Space]
        [Header("相机数值")]
        public float cameraToActorLen = 5;

        [Space]
        [Header("动画")]
        public string idleAniName = "Idle";
        public string runAniName = "Running";
        public string jumpAniName = "Jump";
    }
}