using Duo1JFramework.FSM;
using Duo1JFramework.ObjectPool;
using System;
using UnityEngine;

//TODO hlj 虚拟相机根节点
//相机旋转Y限制
//相机切换

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色控制器
    /// </summary>
    [RequireComponent(typeof(ActorParam), typeof(ActorPoint))]
    public class ActorController : MonoRegister
    {
        /// <summary>
        /// 角色模型
        /// </summary>
        [Header("组件")]
        [SerializeField]
        private GameObject model;

        /// <summary>
        /// 角色动画控制器
        /// </summary>
        [SerializeField]
        private Animator animator;

        /// <summary>
        /// 角色刚体
        /// </summary>
        [SerializeField]
        private Rigidbody rigidBody;

        /// <summary>
        /// 角色参数
        /// </summary>
        private ActorParam param;

        /// <summary>
        /// 角色挂点
        /// </summary>
        private ActorPoint point;

        /// <summary>
        /// 有限状态机
        /// </summary>
        private StateMachine fsm;

        /// <summary>
        /// 当前状态机状态
        /// </summary>
        public string CurState { get; private set; }

        /// <summary>
        /// 是否着地
        /// </summary>
        public bool Grounded { get; private set; }

        /// <summary>
        /// 是否绑定了相机
        /// </summary>
        public bool BindCamera { get; set; }

        /// <summary>
        /// 当前播放的动画名
        /// </summary>
        private string curAniName;

        #region Switch

        /// <summary>
        /// 下落速度增加
        /// </summary>
        public bool FallSpeedUp { get; set; } = false;

        /// <summary>
        /// 更新着地状态
        /// </summary>
        public bool UpdateGrounded { get; set; } = true;

        #endregion Switch

        #region Callback

        /// <summary>
        /// 落地状态改变
        /// </summary>
        public Action<bool> OnGroundedChange;

        #endregion Callback

        #region Field

        /// <summary>
        /// 与地面相交的法线向量
        /// </summary>
        private Vector3 normal = Vector3.up;

        #endregion Field

        #region Public Method

        #region Const

        //触地检测射线Y轴偏移
        private const float RayGroundOffsetY = 0.3f;
        //触地检测射线长度
        private const float RayGroundLen = 0.4f;

        #endregion Const

        #region FSM

        /// <summary>
        /// 初始化状态机
        /// </summary>
        public void InitFSM(string curStateName, params StateNode[] stateList)
        {
            fsm = StateMachine.Create(curStateName, stateList);
            CurState = curStateName;
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        public void SwitchState(string stateName, bool ignoreNextTick = true)
        {
            if (fsm.SwitchState(stateName, ignoreNextTick))
            {
                CurState = stateName;
            }
        }

        /// <summary>
        /// 是否处在状态
        /// </summary>
        /// <param name="stateName"></param>
        public bool InState(string stateName)
        {
            return fsm.InState(stateName);
        }

        #endregion FSM

        #region Transform

        /// <summary>
        /// 目视Forward
        /// </summary>
        public Vector3 EyeForward
        {
            get
            {
                if (BindCamera)
                {
                    return Vector3.Cross(point.CameraPoint.right, Vector3.up).normalized;
                }
                return Vector3.forward.normalized;
            }
        }

        /// <summary>
        /// 目视Right
        /// </summary>
        public Vector3 EyeRight
        {
            get
            {
                if (BindCamera)
                {
                    return point.CameraPoint.right.normalized;
                }
                return Vector3.right.normalized;
            }
        }

        /// <summary>
        /// 目视Up
        /// </summary>
        public Vector3 EyeUp
        {
            get
            {
                return Vector3.up.normalized;
            }
        }

        /// <summary>
        /// 旋转对象
        /// </summary>
        public GameObject RotateGo => model;

        #endregion Transform

        #region Helper

        /// <summary>
        /// 通过目视方向获取轴
        /// </summary>
        public Vector3 GetAxisByEye(float h, float v)
        {
            return EyeForward * v + EyeRight * h;
        }

        /// <summary>
        /// 轴盲区检验
        /// </summary>
        public bool CheckAxis(float h, float v)
        {
            return Mathf.Abs(h) > Def.MIN_HAXIS_MOVE ||
                   Mathf.Abs(v) > Def.MIN_VAXIS_MOVE;
        }

        /// <summary>
        /// 轴为0检验
        /// </summary>
        public bool CheckAxisZero(float h, float v)
        {
            return h == 0 && v == 0;
        }

        #endregion Helper

        #region Control

        /// <summary>
        /// 通过轴设置速度 (以目视Forward为参考系)
        /// </summary>
        public void SetMoveSpeedByAxis(float h, float v)
        {
            Vector3 axisByEye = GetAxisByEye(h, v);
            if (normal != Vector3.up)
            {
                axisByEye = Vector3.ProjectOnPlane(axisByEye, normal).normalized;
            }

            Vector3 velocity = axisByEye * param.moveSpeed;
            SetVelocity(new Vector2(velocity.x, velocity.z));

#if UNITY_EDITOR
            if (h != 0 || v != 0)
                editor_moveAxisByEye = axisByEye;
#endif
        }

        /// <summary>
        /// 通过轴设置旋转 (以目视Forward为参考系旋转朝前)
        /// </summary>
        public void RotateByAxis(float h, float v)
        {
            if (CheckAxisZero(h, v))
                return;

            Vector3 forward = RotateGo.transform.forward;
            Vector3 axisByEye = GetAxisByEye(h, v);

            RotateGo.transform.forward = Vector3.Slerp(
                forward,
                axisByEye,
                param.rotateSpeed * Time.deltaTime
            );
        }

        /// <summary>
        /// 跳跃
        /// </summary>
        public void Jump(float h, float v)
        {
            Vector3 jumpDir = Vector3.up + GetAxisByEye(h, v);
            AddForce(jumpDir * param.jumpForce);
        }

        #endregion Control

        #region Camera

        /// <summary>
        /// 旋转相机挂点
        /// </summary>
        public void RotateCameraPoint(float mx, float my)
        {
            if (CheckAxisZero(mx, my))
                return;
            Transform cameraPoint = point.CameraPoint;

            Vector3 angle = cameraPoint.eulerAngles;
            cameraPoint.rotation = Quaternion.Euler(
                    angle.x - my * param.mouseSpeedY * Time.deltaTime,
                    angle.y + mx * param.mouseSpeedX * Time.deltaTime,
                    angle.z
                );
        }

        /// <summary>
        /// 更新相机坐标 (旋转后更新)
        /// </summary>
        public void UpdateCameraPointPos()
        {
            point.CameraPoint.localPosition = point.OriCameraPointLocPos -
                point.CameraPoint.forward * param.cameraToActorLen;
        }

        #endregion Camera

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

        #region Animation

        /// <summary>
        /// 获取Animator
        /// </summary>
        /// <returns></returns>
        private Animator GetAni()
        {
            if (animator == null) ErrNoComponent(typeof(Animator));
            return animator;
        }

        /// <summary>
        /// 动画状态转换
        /// </summary>
        public void AniCrossFade(string stateName, float transitionRate = 0.2f, int layer = -1)
        {
            if (!AniCanStateChange(stateName))
                return;
            curAniName = stateName;
            GetAni()?.CrossFade(stateName, transitionRate, layer);
        }

        /// <summary>
        /// 当前是否可以转换为目标动画状态
        /// </summary>
        public bool AniCanStateChange(string stateName)
        {
            Assert.NotNullOrEmpty(stateName, "动画状态名不可为空");
            return !stateName.Equals(curAniName);
        }

        #endregion Animation

        #region Misc

        /// <summary>
        /// 获取ActorParam
        /// </summary>
        public ActorParam GetActorParam()
        {
            return param;
        }

        /// <summary>
        /// 获取ActorPoint
        /// </summary>
        public ActorPoint GetActorPoint()
        {
            return point;
        }

        #endregion Misc

        #endregion Public Method

        #region Private Method

        /// <summary>
        /// 初始化Inspector配置数据
        /// </summary>
        private void InitActorMonoData()
        {
            //ActorParam
            param = GetComponent<ActorParam>();
            if (param == null)
            {
                ErrNoComponent(typeof(ActorParam), "添加默认ActorParam组件");
                param = gameObject.AddComponent<ActorParam>();
            }

            //ActorPoint
            point = GetComponent<ActorPoint>();
            if (point == null)
            {
                ErrNoComponent(typeof(ActorPoint), "添加默认ActorPoint组件");
                point = gameObject.AddComponent<ActorPoint>();
                point.AutoMatch();
            }
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitComponent()
        {
            //Rigidbody
            if (rigidBody != null)
            {
                rigidBody.isKinematic = false;
                rigidBody.constraints = RigidbodyConstraints.FreezeRotationX |
                                        RigidbodyConstraints.FreezeRotationY |
                                        RigidbodyConstraints.FreezeRotationZ;
            }

            //Animator
            if (animator != null)
            {
                animator.applyRootMotion = false;
            }
        }

        /// <summary>
        /// 下落加速
        /// </summary>
        private void UpdateFallSpeedUp()
        {
            if (!FallSpeedUp)
                return;
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

        /// <summary>
        /// 更新是否着地状态
        /// </summary>
        private void UpdateGroundedState()
        {
            if (!UpdateGrounded)
                return;

            bool oldVal = Grounded;
            Grounded = Physics.Raycast(
                point.Root.position + Vector3.up * RayGroundOffsetY,
                Vector3.down,
                out RaycastHit hitInfo,
                RayGroundLen,
                Layer.OnlyLayer(Layer.WORLD));

            normal = Grounded ? hitInfo.normal : Vector3.up;

            if (oldVal != Grounded)
            {
                OnGroundedChange?.Invoke(Grounded);
            }
        }

        /// <summary>
        /// 更新状态机
        /// </summary>
        private void UpdateFSM()
        {
            if (fsm == null)
                return;
            fsm.Tick();
        }

        #endregion Private Method

        #region Lifecycle

        private void OnUpdate()
        {
            UpdateFallSpeedUp();
            UpdateGroundedState();

            UpdateFSM();
        }

        private void Awake()
        {
            InitActorMonoData();
            InitComponent();

            Register.RegisterUpdate(OnUpdate);
        }

        /// <summary>
        /// 报错未持有组件
        /// </summary>
        private void ErrNoComponent(Type type, string msg = "")
        {
            Err($"该角色未持有{type.Name}组件，name: {gameObject.name}。{msg}", true);
        }

        /// <summary>
        /// Actor报错
        /// </summary>
        private void Err(string msg, bool force = true)
        {
            if (force)
            {
                Log.ErrorForce(ToString(), msg);
            }
            else
            {
                Log.Error(ToString(), msg);
            }
        }

        public override string ToString()
        {
            return $"<Actor: {gameObject.name}>";
        }

        #endregion Lifecycle

        #region Gizmos

#if UNITY_EDITOR

        private Vector3 editor_moveAxisByEye = Vector3.zero;

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Vector3 rootPos = point.Root.position;

                //触地检测射线
                if (UpdateGrounded)
                {
                    Gizmos.color = Color.red;
                    Vector3 rayGroundStartPos = rootPos + Vector3.up * RayGroundOffsetY;
                    Gizmos.DrawLine(
                        rayGroundStartPos,
                        rayGroundStartPos + Vector3.down * RayGroundLen
                    );
                }

                //受轴控制的目视前方
                Gizmos.color = Color.blue;
                Vector3 rayEyeForwardStartPos = rootPos + Vector3.up * 0.5f;
                Gizmos.DrawLine(
                    rayEyeForwardStartPos,
                    rayEyeForwardStartPos + editor_moveAxisByEye.normalized
                );

                //相机挂点
                if (point.CameraPoint != null)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(point.CameraPoint.position, 0.15f);
                }
            }
        }

#endif

        #endregion Gizmos

        #region Editor

        /// <summary>
        /// 收集组件
        /// </summary>
        public void CollectComponent()
        {
            if (model == null)
            {
                Err("该角色未设置模型", true);
                return;
            }
            if (animator == null)
            {
                animator = model.GetComponent<Animator>();
            }
            if (rigidBody == null)
            {
                rigidBody = GetComponent<Rigidbody>();
            }
        }

        /// <summary>
        /// 获取Hierarchy显示信息
        /// </summary>
        public string GetHierarchyInfo()
        {
            return Pool.StringBuilderPool.Using((item) =>
            {
                item.Value.AppendLine($"状态机: {CurState}");
                item.Value.AppendLine($"动画状态: {curAniName}");
                item.Value.AppendLine($"是否触地: {Grounded}");
                item.Value.AppendLine($"是否绑定相机: {BindCamera}");

                return item.Value.ToString();
            }).ToString();
        }

        #endregion Editor
    }
}