using UnityEngine;

namespace Duo1JFramework.AnimationAPI
{
    /// <summary>
    /// 足部IK控制器
    /// </summary>
    public class FootIKController : MonoRegister
    {
        /// <summary>
        /// 单脚IK状态
        /// </summary>
        private class FootIKState
        {
            public AvatarIKGoal IKGoal;
            public Transform FootTF;

            public float CurGoal;
            public float TarGoal;

            public bool Hit;
            public bool HasTarget;

            public Vector3 Pos;
            public Quaternion Rot;
        }

        [Label("动画控制器")]
        [SerializeField]
        private Animator animator;

        [Label("目标动画层级")]
        [SerializeField]
        private int tarLayerIdx = 0;

        [Label("权重过渡速度")]
        [SerializeField]
        private float goalLerpSpeed = 10;

        [Label("目标位置平滑速度")]
        [SerializeField]
        private float posLerpSpeed = 20;

        [Label("目标旋转平滑速度")]
        [SerializeField]
        private float rotLerpSpeed = 20;

        [Label("最大地面坡度")]
        [SerializeField]
        private float maxGroundAngle = 60;

        [Label("应用脚部旋转")]
        [SerializeField]
        private bool applyFootRotation = true;

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
        /// 左脚IK状态
        /// </summary>
        private FootIKState leftFoot;

        /// <summary>
        /// 右脚IK状态
        /// </summary>
        private FootIKState rightFoot;

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
            SetGoal(leftFoot, goal, immediately);
        }

        /// <summary>
        /// 设置右脚权重
        /// </summary>
        public void SetRightGoal(float goal, bool immediately = false)
        {
            SetGoal(rightFoot, goal, immediately);
        }

        /// <summary>
        /// 设置单脚权重
        /// </summary>
        private void SetGoal(FootIKState foot, float goal, bool immediately)
        {
            if (foot == null)
            {
                return;
            }

            goal = Mathf.Clamp01(goal);
            foot.TarGoal = goal;
            if (immediately)
            {
                foot.CurGoal = goal;
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (layerIndex != tarLayerIdx)
            {
                return;
            }

            RaycastUpdate(leftFoot);
            RaycastUpdate(rightFoot);
            ApplyIK(leftFoot);
            ApplyIK(rightFoot);
        }

        /// <summary>
        /// 射线更新
        /// </summary>
        private void RaycastUpdate(FootIKState foot)
        {
            if (foot?.FootTF == null)
            {
                return;
            }

            Vector3 rayPos = GetRayPos(foot.FootTF);
            foot.Hit = Physics.CapsuleCast(rayPos, rayPos + foot.FootTF.forward * rayCapsuleLen, rayCapsuleRadius,
                Vector3.down, out RaycastHit hitInfo, rayMaxDistance, rayLayerMask);

            if (!foot.Hit)
            {
                return;
            }

            float groundAngle = Vector3.Angle(Vector3.up, hitInfo.normal);
            if (groundAngle > maxGroundAngle)
            {
                foot.Hit = false;
                return;
            }

            Plane plane = new Plane(hitInfo.normal, hitInfo.point);
            Vector3 tarPos = plane.ClosestPointOnPlane(foot.FootTF.position) + tarPosOffset;
            Quaternion tarRot = applyFootRotation
                ? Quaternion.FromToRotation(foot.FootTF.up, hitInfo.normal) * foot.FootTF.rotation
                : foot.FootTF.rotation;

            SmoothTarget(foot, tarPos, tarRot);
        }

        /// <summary>
        /// 应用单脚IK
        /// </summary>
        private void ApplyIK(FootIKState foot)
        {
            if (foot == null)
            {
                return;
            }

            if (foot.Hit)
            {
                animator.SetIKPositionWeight(foot.IKGoal, foot.CurGoal);
                animator.SetIKRotationWeight(foot.IKGoal, applyFootRotation ? foot.CurGoal : 0);
                animator.SetIKPosition(foot.IKGoal, foot.Pos);
                animator.SetIKRotation(foot.IKGoal, foot.Rot);
            }
            else
            {
                animator.SetIKPositionWeight(foot.IKGoal, 0);
                animator.SetIKRotationWeight(foot.IKGoal, 0);
            }
        }

        /// <summary>
        /// 平滑目标位置和旋转
        /// </summary>
        private void SmoothTarget(FootIKState foot, Vector3 tarPos, Quaternion tarRot)
        {
            if (!foot.HasTarget)
            {
                foot.Pos = tarPos;
                foot.Rot = tarRot;
                foot.HasTarget = true;
                return;
            }

            float posLerpT = GetLerpT(posLerpSpeed);
            float rotLerpT = GetLerpT(rotLerpSpeed);
            foot.Pos = Vector3.Lerp(foot.Pos, tarPos, posLerpT);
            foot.Rot = Quaternion.Slerp(foot.Rot, tarRot, rotLerpT);
        }

        private void OnPreUpdate()
        {
            UpdateGoal(leftFoot);
            UpdateGoal(rightFoot);
        }

        /// <summary>
        /// 更新单脚权重
        /// </summary>
        private void UpdateGoal(FootIKState foot)
        {
            if (foot == null)
            {
                return;
            }

            if (Mathf.Abs(foot.CurGoal - foot.TarGoal) < 0.01f)
            {
                foot.CurGoal = foot.TarGoal;
            }
            else
            {
                foot.CurGoal = Mathf.Lerp(foot.CurGoal, foot.TarGoal, GetLerpT(goalLerpSpeed));
            }
        }

        /// <summary>
        /// 获取插值比例
        /// </summary>
        private float GetLerpT(float lerpSpeed)
        {
            if (lerpSpeed <= 0)
            {
                return 1;
            }

            return Mathf.Clamp01(lerpSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 获取射线起点
        /// </summary>
        private Vector3 GetRayStartPos(Transform footTF)
        {
            return footTF.position + rayPosOffset;
        }

        private void OnDrawGizmosSelected()
        {
            DrawFootGizmos(leftFootTF, leftFoot);
            DrawFootGizmos(rightFootTF, rightFoot);
        }

        /// <summary>
        /// 绘制单脚Gizmos
        /// </summary>
        private void DrawFootGizmos(Transform footTF, FootIKState foot)
        {
            if (foot != null && foot.HasTarget)
            {
                Gizmos.color = foot.Hit ? Color.magenta : Color.gray;
                Gizmos.DrawWireSphere(foot.Pos, 0.1f);
            }

            if (footTF == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            Vector3 rayStartPos = GetRayStartPos(footTF);
            Vector3 rayStartPos2 = rayStartPos + footTF.forward * rayCapsuleLen;
            GizmosUtil.DrawWireCapsule(rayStartPos, rayStartPos2, rayCapsuleRadius);

            Vector3 rayEndPos = rayStartPos + Vector3.down * rayMaxDistance;
            Vector3 rayEndPos2 = rayStartPos2 + Vector3.down * rayMaxDistance;
            GizmosUtil.DrawWireCapsule(rayEndPos, rayEndPos2, rayCapsuleRadius);

            Gizmos.DrawLine(rayStartPos, rayEndPos);
            Gizmos.DrawLine(rayStartPos2, rayEndPos2);
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

            leftFoot = new FootIKState()
            {
                IKGoal = AvatarIKGoal.LeftFoot,
                FootTF = leftFootTF,
                Rot = leftFootTF.rotation,
            };
            rightFoot = new FootIKState()
            {
                IKGoal = AvatarIKGoal.RightFoot,
                FootTF = rightFootTF,
                Rot = rightFootTF.rotation,
            };

            Reg.RegisterPreUpdate(OnPreUpdate);
        }
    }
}
