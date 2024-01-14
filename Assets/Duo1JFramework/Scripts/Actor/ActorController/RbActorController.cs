using System;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// Rigidbody Actor控制器
    /// </summary>
    public class RbActorController : ActorController
    {
        /// <summary>
        /// 角色刚体
        /// </summary>
        [SerializeField]
        private Rigidbody rigidBody;

        #region Control

        /// <summary>
        /// 通过轴设置速度 (以目视Forward为参考系)
        /// </summary>
        public void SetMoveSpeedByAxis(float h, float v)
        {
            Vector3 axisByEye = GetAxisByEye(h, v);
            if (normal != Vector3.up)
            {
                Vector3 projectVec = Vector3.ProjectOnPlane(axisByEye, normal).normalized;
                if ((projectVec.y - axisByEye.y) > 0 && Vector3.Angle(Vector3.up, normal) > param.maxSlopeAngle)
                {
                    SetVelocity(new Vector2(0, 0));
                    return;
                }
                axisByEye = projectVec;
            }

#if UNITY_EDITOR
            if (h != 0 || v != 0)
                editor_moveAxisByEye = axisByEye;
#endif

            Vector3 velocity = axisByEye * param.moveSpeed;
            SetVelocity(new Vector2(velocity.x, velocity.z));
        }

        /// <summary>
        /// 通过力数值跳跃
        /// </summary>
        public void JumpByForce(float h, float v)
        {
            Vector3 jumpDir = Vector3.up + GetAxisByEye(h, v);
            AddForce(jumpDir * param.jumpForce);
        }

        /// <summary>
        /// 通过高度数值跳跃
        /// </summary>
        public void JumpByHeight()
        {
            float velocityY = Convert.ToSingle(Math.Sqrt(-2 * param.jumpHeight * Physics.gravity.y));
            SetVelocityY(velocityY);
        }

        #endregion Control

        #region Physic

        /// <summary>
        /// 获取刚体组件
        /// </summary>
        public Rigidbody GetRb()
        {
            if (rigidBody == null) ErrNoComponent(typeof(Rigidbody));
            return rigidBody;
        }

        /// <summary>
        /// 获取当前速度
        /// </summary>
        public Vector3 GetVelocity()
        {
            Rigidbody rb = GetRb();
            if (rb)
            {
                return rb.velocity;
            }
            return Vector3.zero;
        }

        /// <summary>
        /// 设置当前速度
        /// </summary>
        /// <param name="velocity"></param>
        public void SetVelocity(Vector3 velocity)
        {
            Rigidbody rb = GetRb();
            if (rb)
            {
                rb.velocity = velocity;
            }
        }

        /// <summary>
        /// 设置当前平面速度
        /// </summary>
        /// <param name="velocityPlane"></param>
        public void SetVelocity(Vector2 velocityPlane)
        {
            Rigidbody rb = GetRb();
            if (rb)
            {
                Vector3 v = GetVelocity();
                SetVelocity(new Vector3(velocityPlane.x, v.y, velocityPlane.y));
            }
        }

        /// <summary>
        /// 设置当前Y轴速度
        /// </summary>
        public void SetVelocityY(float velocityY)
        {
            Vector3 velocity = GetVelocity();
            SetVelocity(new Vector3(velocity.x, velocityY, velocity.z));
        }

        /// <summary>
        /// 添加力
        /// </summary>
        public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Force)
        {
            Rigidbody rb = GetRb();
            if (rb)
            {
                rb.AddForce(force, forceMode);
            }
        }

        #endregion Physic

        #region Override

        /// <summary>
        /// 下落加速
        /// </summary>
        protected override void UpdateFallSpeedUp()
        {
            if (rigidBody == null)
                return;

            Vector3 veloticy = rigidBody.velocity;
            if (veloticy.y != 0)
            {
                rigidBody.velocity = new Vector3(
                    veloticy.x,
                    veloticy.y - param.fallSpeedUp * Time.deltaTime,
                    veloticy.z
                );
            }
        }

        protected override void OnInitComponent()
        {
            //Rigidbody
            if (rigidBody != null)
            {
                rigidBody.isKinematic = false;
                rigidBody.constraints = RigidbodyConstraints.FreezeRotationX |
                                        RigidbodyConstraints.FreezeRotationY |
                                        RigidbodyConstraints.FreezeRotationZ;
            }
        }

        protected override void OnCollectComponent()
        {
            if (rigidBody == null)
            {
                rigidBody = gameObject.GetAndAssertComponent<Rigidbody>();
            }
        }

        #endregion Override

        #region Gizmos

        protected Vector3 editor_moveAxisByEye = Vector3.zero;

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (Application.isPlaying)
            {
                Vector3 rootPos = point.Root.position;

                //受轴控制的目视前方
                Gizmos.color = Color.blue;
                Vector3 rayEyeForwardStartPos = rootPos + Vector3.up * 0.5f;
                Gizmos.DrawLine(
                    rayEyeForwardStartPos,
                    rayEyeForwardStartPos + editor_moveAxisByEye.normalized
                );
            }
        }

        #endregion Gizmos
    }
}