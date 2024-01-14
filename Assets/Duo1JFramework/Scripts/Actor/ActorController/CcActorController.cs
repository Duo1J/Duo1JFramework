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

        [SerializeField]
        protected float gravity = -9.81f;

        #region Control

        /// <summary>
        /// 通过轴移动 (以目视Forward为参考系)
        /// </summary>
        public void MoveByAxis(float h, float v)
        {
            Vector3 axisByEye = GetAxisByEye(h, v);
            Vector3 velocity = axisByEye * param.moveSpeed;
            Move(velocity);
        }

        /// <summary>
        /// 通过高度数值跳跃
        /// </summary>
        public void JumpByHeight()
        {
            float velocityY = Convert.ToSingle(Math.Sqrt(-2 * param.jumpHeight * gravity));
            Move(new Vector3(0, velocityY, 0));
        }

        #endregion Control

        #region CharacterController

        protected CharacterController GetCc()
        {
            if (cc == null) ErrNoComponent(typeof(CharacterController));
            return cc;
        }

        /// <summary>
        /// 移动
        /// </summary>
        public CollisionFlags Move(Vector3 motion)
        {
            CharacterController cc = GetCc();
            if (cc)
            {
                return cc.Move(motion);
            }
            return CollisionFlags.None;
        }

        #endregion CharacterController

        #region Override

        protected override void OnUpdateSub()
        {
            base.OnUpdateSub();
            //TODO hlj 重力下坠
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