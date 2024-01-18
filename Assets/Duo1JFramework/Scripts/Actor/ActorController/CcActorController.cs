using System;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// CharactorController Actor控制器
    /// </summary>
    public class CcActorController : ActorController
    {
        /// <summary>
        /// 角色控制器
        /// </summary>
        [SerializeField]
        private CharacterController cc;

        /// <summary>
        /// 重力比率
        /// </summary>
        [SerializeField]
        protected float gravityRate = 1;

        /// <summary>
        /// CC组件重力
        /// </summary>
        public static float Gravity { get; set; } = -9.81f;

        /// <summary>
        /// 乘以比率后的重力
        /// </summary>
        public float RateGravity => Gravity * gravityRate;

        /// <summary>
        /// 当前重力速度
        /// </summary>
        private float gravityVelocity;

        #region Control

        /// <summary>
        /// 获取通过轴移动的速度 (以目视Forward为参考系)
        /// </summary>
        public Vector3 GetMoveVeloByAxis(float h, float v)
        {
            Vector3 axisByEye = GetAxisByEye(h, v);
            axisByEye = Vector3.ProjectOnPlane(axisByEye, normal).normalized;
            return axisByEye * param.moveSpeed;
        }

        /// <summary>
        /// 获取通过高度数值跳跃的速度
        /// </summary>
        public float GetJumpVeloByHeight()
        {
            float velocityY = Convert.ToSingle(Math.Sqrt(-2 * param.jumpHeight * RateGravity));
            return velocityY;
        }

        #endregion Control

        #region CharacterController

        protected CharacterController GetCc()
        {
            if (cc == null) ErrNoComponent(typeof(CharacterController));
            return cc;
        }

        /// <summary>
        /// 设置速度
        /// </summary>
        public CollisionFlags SetVelocity(Vector3 velocity)
        {
            CharacterController cc = GetCc();
            if (cc)
            {
                return cc.Move(velocity * Time.deltaTime);
            }
            return CollisionFlags.None;
        }

        /// <summary>
        /// 获取当前速度
        /// </summary>
        public Vector3 GetVelocity()
        {
            CharacterController cc = GetCc();
            if (cc)
            {
                return cc.velocity;
            }
            return Vector3.zero;
        }

        #endregion CharacterController

        #region Override

        protected override void OnUpdateSub()
        {
            base.OnUpdateSub();

            //todo hlj 抽出Veclocity在LateUpdate结算

            Vector3 velocity = GetVelocity();

            //重力速度
            if (Grounded)
            {
                gravityVelocity = 0;
            }
            else
            {
                gravityVelocity += RateGravity * Time.deltaTime;
                SetVelocity(GetVelocity() + Vector3.up * gravityVelocity);
            }

            SetVelocity(velocity);
        }

        protected override void OnCollectComponent()
        {
            if (cc == null)
            {
                cc = gameObject.GetAndAssertComponent<CharacterController>();
            }
        }

        protected override void OnInitComponent()
        {
        }

        protected override void UpdateFallSpeedUp()
        {
        }

        #endregion  Override
    }
}