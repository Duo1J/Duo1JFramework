using System;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// CharactorController角色控制器
    /// </summary>
    public class CcActorController : BaseActorController
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
        /// 更新速度委托
        /// </summary>
        private Func<Vector3, Vector3> OnUpdateVelocity;

        /// <summary>
        /// 注册更新速度委托
        /// </summary>
        public void RegisterOnUpdateVelocity(Func<Vector3, Vector3> handle)
        {
            OnUpdateVelocity = handle;
        }

        /// <summary>
        /// 获取通过轴移动的方向 (以目视Forward为参考系)
        /// </summary>
        public Vector3 GetMoveDirByAxis(float h, float v)
        {
            Vector3 axisByEye = GetAxisByEye(h, v);
            axisByEye = Vector3.ProjectOnPlane(axisByEye, Normal).normalized;
            return axisByEye;
        }

        /// <summary>
        /// 获取通过高度数值跳跃的速度
        /// </summary>
        public float GetJumpVeloByHeight()
        {
            return Convert.ToSingle(Math.Sqrt(-2 * param.jumpHeight * RateGravity));
        }

        #endregion Control

        #region CharacterController

        public CharacterController GetCC()
        {
            if (cc == null) ErrNoComponent(typeof(CharacterController));
            return cc;
        }

        /// <summary>
        /// 设置速度
        /// </summary>
        public CollisionFlags SetVelocity(Vector3 velocity)
        {
            CharacterController cc = GetCC();
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
            CharacterController cc = GetCC();
            if (cc)
            {
                return cc.velocity;
            }
            return Vector3.zero;
        }

        #endregion CharacterController

        #region Override

        protected void UpdateVelocity()
        {
            Vector3 velocity = Vector3.zero;

            //重力速度
            if (Grounded)
            {
                gravityVelocity = 0;
            }
            else
            {
                gravityVelocity += RateGravity * Time.deltaTime;
                velocity.y += gravityVelocity;
            }

            if (velocity.y != 0)
            {
                velocity.y -= param.fallSpeedUp * Time.deltaTime;
            }

            if (OnUpdateVelocity != null)
            {
                velocity = OnUpdateVelocity.Invoke(velocity);
            }

            SetVelocity(velocity);
        }

        protected override void OnUpdateSub()
        {
            base.OnUpdateSub();
            UpdateVelocity();
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