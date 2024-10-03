using UnityEngine;

namespace Duo1JFramework.AnimationAPI
{
    /// <summary>
    /// 足部IK控制器
    /// </summary>
    public class FootIKController : MonoRegister
    {
        [Label("动画控制器")]
        [SerializeField]
        private Animator animator;

        [Label("目标动画层级")]
        [SerializeField]
        private int tarLayerIdx = 0;

        [Label("权重过渡速度")]
        [SerializeField]
        private float goalLerpSpeed = 10;

        [Space]
        [Label("左侧脚TF")]
        [SerializeField]
        private Transform leftFootTF;

        [Label("右侧脚TF")]
        [SerializeField]
        private Transform rightFootTF;

        [Label("目标位置偏移")]
        [SerializeField]
        private Vector3 tarPosOffset = new Vector3(0, 0.1f, 0);

        /// <summary>
        /// 左侧射线起点
        /// </summary>
        private Vector3 LeftRayPos => leftFootTF.position + rayPosOffset;

        /// <summary>
        /// 右侧射线起点
        /// </summary>
        private Vector3 RightRayPos => rightFootTF.position + rayPosOffset;

        [Space]
        [Label("射线检测层")]
        [SerializeField]
        private LayerMask rayLayerMask = 1 << 6;

        [Label("射线位置偏移")]
        [SerializeField]
        private Vector3 rayPosOffset = new Vector3(0, 0.6f, 0);

        [Label("胶囊长度")]
        [SerializeField]
        private float rayCapsuleLen = 0.2f;

        [Label("胶囊半径")]
        [SerializeField]
        private float rayCapsuleRadius = 0.05f;

        [Label("射线最大检测距离")]
        [SerializeField]
        private float rayMaxDistance = 1f;

        /// <summary>
        /// 当前左脚权重
        /// </summary>
        private float leftGoal = 0;

        /// <summary>
        /// 当前右脚权重
        /// </summary>
        private float rightGoal = 0;

        /// <summary>
        /// 目标左脚权重
        /// </summary>
        private float tarLeftGoal = 0;

        /// <summary>
        /// 目标右脚权重
        /// </summary>
        private float tarRightGoal = 0;

        /// <summary>
        /// 左侧命中
        /// </summary>
        private bool leftHit = false;

        /// <summary>
        /// 右侧命中
        /// </summary>
        private bool rightHit = false;

        /// <summary>
        /// 左侧目标位置
        /// </summary>
        private Vector3 leftPos;

        /// <summary>
        /// 右侧目标位置
        /// </summary>
        private Vector3 rightPos;

        /// <summary>
        /// 左侧目标旋转
        /// </summary>
        private Quaternion leftRot;

        /// <summary>
        /// 右侧目标旋转
        /// </summary>
        private Quaternion rightRot;

        /// <summary>
        /// 设置权重
        /// </summary>
        public void SetGoal(float leftGoal, float rightGoal, bool immediately = false)
        {
            SetLeftGoal(leftGoal, immediately);
            SetRightGoal(rightGoal, immediately);
        }

        /// <summary>
        /// 设置左脚权重
        /// </summary>
        public void SetLeftGoal(float goal, bool immediately = false)
        {
            tarLeftGoal = goal;
            if (immediately)
            {
                leftGoal = goal;
            }
        }

        /// <summary>
        /// 设置右脚权重
        /// </summary>
        public void SetRightGoal(float goal, bool immediately = false)
        {
            tarRightGoal = goal;
            if (immediately)
            {
                rightGoal = goal;
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (layerIndex == tarLayerIdx)
            {
                RaycastUpdate();

                if (leftHit)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftGoal);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftGoal);
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftPos);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftRot);
                }
                else
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
                }

                if (rightHit)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightGoal);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightGoal);
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, rightPos);
                    animator.SetIKRotation(AvatarIKGoal.RightFoot, rightRot);
                }
                else
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
                }
            }
        }

        /// <summary>
        /// 射线更新
        /// </summary>
        private void RaycastUpdate()
        {
            leftHit = Physics.CapsuleCast(LeftRayPos, LeftRayPos + leftFootTF.forward * rayCapsuleLen, rayCapsuleRadius,
                Vector3.down, out RaycastHit hitInfoLeft, rayMaxDistance, rayLayerMask);

            if (leftHit)
            {
                Plane plane = new Plane(hitInfoLeft.normal, hitInfoLeft.point);
                leftPos = plane.ClosestPointOnPlane(leftFootTF.position) + tarPosOffset;
                leftRot = Quaternion.FromToRotation(leftFootTF.up, hitInfoLeft.normal) * leftFootTF.rotation;
            }

            rightHit = Physics.CapsuleCast(RightRayPos, RightRayPos + rightFootTF.forward * rayCapsuleLen, rayCapsuleRadius,
                Vector3.down, out RaycastHit hitInfoRight, rayMaxDistance, rayLayerMask);
            if (rightHit)
            {
                Plane plane = new Plane(hitInfoRight.normal, hitInfoRight.point);
                rightPos = plane.ClosestPointOnPlane(rightFootTF.position) + tarPosOffset;
                rightRot = Quaternion.FromToRotation(rightFootTF.up, hitInfoRight.normal) * rightFootTF.rotation;
            }
        }

        private void OnPreUpdate()
        {
            if (Mathf.Abs(leftGoal - tarLeftGoal) < 0.01f)
            {
                leftGoal = tarLeftGoal;
            }
            else
            {
                leftGoal = Mathf.Lerp(leftGoal, tarLeftGoal, goalLerpSpeed * Time.deltaTime);
            }

            if (Mathf.Abs(rightGoal - tarRightGoal) < 0.01f)
            {
                rightGoal = tarRightGoal;
            }
            else
            {
                rightGoal = Mathf.Lerp(rightGoal, tarRightGoal, goalLerpSpeed * Time.deltaTime);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(leftPos, 0.1f);
            Gizmos.DrawWireSphere(rightPos, 0.1f);

            Gizmos.color = Color.yellow;

            if (leftFootTF != null)
            {
                Vector3 leftPos1 = LeftRayPos;
                Vector3 leftPos2 = LeftRayPos + leftFootTF.forward * rayCapsuleLen;
                GizmosUtil.DrawWireCapsule(leftPos1, leftPos2, rayCapsuleRadius);

                Vector3 leftPos3 = LeftRayPos + Vector3.down * rayMaxDistance;
                Vector3 leftPos4 = LeftRayPos + leftFootTF.forward * rayCapsuleLen + Vector3.down * rayMaxDistance;
                GizmosUtil.DrawWireCapsule(leftPos3, leftPos4, rayCapsuleRadius);

                Gizmos.DrawLine(leftPos1, leftPos3);
                Gizmos.DrawLine(leftPos2, leftPos4);
            }

            if (rightFootTF != null)
            {
                Vector3 rightPos1 = RightRayPos;
                Vector3 rightPos2 = RightRayPos + rightFootTF.forward * rayCapsuleLen;
                GizmosUtil.DrawWireCapsule(rightPos1, rightPos2, rayCapsuleRadius);

                Vector3 rightPos3 = RightRayPos + Vector3.down * rayMaxDistance;
                Vector3 rightPos4 = RightRayPos + rightFootTF.forward * rayCapsuleLen + Vector3.down * rayMaxDistance;
                GizmosUtil.DrawWireCapsule(rightPos3, rightPos4, rayCapsuleRadius);

                Gizmos.DrawLine(rightPos1, rightPos3);
                Gizmos.DrawLine(rightPos2, rightPos4);
            }
        }

        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            if (animator == null || leftFootTF == null || rightFootTF == null)
            {
                Log.ErrorForce($"`{ToString()}` 缺少必要组件");
                enabled = false;
                return;
            }

            Register.RegisterPreUpdate(OnPreUpdate);
        }
    }
}
