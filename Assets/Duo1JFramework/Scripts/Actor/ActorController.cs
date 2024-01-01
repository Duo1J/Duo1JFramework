using Duo1JFramework.FSM;
using System;
using UnityEngine;

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

        [Space]
        [Header("状态")]
        public string curState;

        /// <summary>
        /// 角色是否着地
        /// </summary>
        public bool Grounded
        {
            get => grounded;
            private set => grounded = value;
        }
        public bool grounded = false;

        /// <summary>
        /// 当前播放的动画名
        /// </summary>
        [Space]
        [Header("动画")]
        public string curAniName;

        #region Switch

        /// <summary>
        /// 下落速度增加
        /// </summary>
        public bool FallSpeedUp { get; set; }

        /// <summary>
        /// 更新着地状态
        /// </summary>
        public bool UpdateGrounded { get; set; }

        #endregion Switch

        #region Callback

        /// <summary>
        /// 落地状态改变
        /// </summary>
        public Action<bool> OnGroundedChange;

        #endregion Callback

        #region Public Method

        #region FSM

        /// <summary>
        /// 初始化状态机
        /// </summary>
        public void InitFSM(string curStateName, params StateNode[] stateList)
        {
            fsm = StateMachine.Create(curStateName, stateList);
            curState = curStateName;
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        public void SwitchState(string stateName)
        {
            if (fsm.SwitchState(stateName))
            {
                curState = stateName;
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
            Vector3 velocity = axisByEye * param.moveSpeed;

            SetVelocity(new Vector2(velocity.x, velocity.z));
        }

        /// <summary>
        /// 通过轴设置旋转 (以目视Forward为参考系旋转朝前)
        /// </summary>
        public void RotateByAxis(float h, float v)
        {
            if (CheckAxisZero(h, v))
                return;

            Vector3 forward = transform.forward;
            Vector3 axisByEye = GetAxisByEye(h, v);

            transform.forward = Vector3.Slerp(
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
            Grounded = Physics.Raycast(point.root.position + Vector3.up * 5, Vector3.down, 5.1f, Layer.OnlyLayer(Layer.WORLD));
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
            UpdateFSM();
            UpdateFallSpeedUp();
            UpdateGroundedState();
        }

        private void Awake()
        {
            InitActorMonoData();
            InitComponent();

            Register.RegisterUpdate(OnUpdate);
        }

        #endregion Lifecycle

        /// <summary>
        /// 报错未持有组件
        /// </summary>
        private void ErrNoComponent(Type type, string msg = "")
        {
            Log.ErrorForce($"该角色未持有{type.Name}组件，name: {gameObject.name}。", msg);
        }
    }
}