using Duo1JFramework.AnimationAPI;
using Duo1JFramework.FSM;
using Duo1JFramework.ObjectPool;
using Duo1JFramework.World;
using System;
using System.Text;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// Actor控制器
    /// </summary>
    [RequireComponent(typeof(ActorParam), typeof(ActorPoint))]
    public abstract class BaseActorController : BaseWorldItem
    {
        #region Property

        /// <summary>
        /// 角色模型
        /// </summary>
        [SerializeField]
        protected GameObject model;

        /// <summary>
        /// 角色动画控制器
        /// </summary>
        [SerializeField]
        protected Animator animator;

        /// <summary>
        /// 足部IK控制器
        /// </summary>
        [SerializeField]
        protected FootIKController footIKCon;

        /// <summary>
        /// 角色参数
        /// </summary>
        protected ActorParam param;

        /// <summary>
        /// 角色挂点
        /// </summary>
        protected ActorPoint point;

        /// <summary>
        /// 有限状态机
        /// </summary>
        protected StateMachine fsm;

        #endregion Property

        #region Field

        /// <summary>
        /// 角色参数
        /// </summary>
        public ActorParam Param => param;

        /// <summary>
        /// 角色挂点
        /// </summary>
        public ActorPoint Point => point;

        /// <summary>
        /// 角色逻辑
        /// </summary>
        public BaseActor Logic { get; set; }

        /// <summary>
        /// 当前状态机状态
        /// </summary>
        public string CurState { get; private set; }

        /// <summary>
        /// 当前播放的动画名
        /// </summary>
        public string CurAniName { get; private set; }

        #region Transform

        /// <summary>
        /// 模型Go
        /// </summary>
        public GameObject Model => model;

        /// <summary>
        /// 根节点Go
        /// </summary>
        public GameObject Root => gameObjectCache;

        /// <summary>
        /// 旋转对象
        /// </summary>
        public GameObject RotateGo => Model;

        /// <summary>
        /// 目视Forward
        /// </summary>
        public Vector3 EyeForward
        {
            get
            {
                if (CameraBinded)
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
                if (CameraBinded)
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
        /// 与地面相交的法线向量
        /// </summary>
        protected Vector3 Normal { get; private set; } = Vector3.up;

        /// <summary>
        /// 是否着地
        /// </summary>
        public bool Grounded { get; private set; }

        #endregion Transform

        #region Camera

        /// <summary>
        /// 是否绑定了相机
        /// </summary>
        public bool CameraBinded => Logic == null ? false : Logic.CameraBinded;

        /// <summary>
        /// 相机X轴左右偏移
        /// </summary>
        public float CameraOffsetX { get; set; }

        /// <summary>
        /// 相机Y轴上下偏移
        /// </summary>
        public float CameraOffsetY { get; set; }

        /// <summary>
        /// 相机Z轴前后偏移
        /// </summary>
        public float CameraOffsetZ { get; set; }

        #endregion Camera

        #endregion Field

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
        /// 落地状态改变回调
        /// </summary>
        public Action<bool> OnGroundedChange;

        #endregion Callback

        #region Public Method

        #region FSM

        /// <summary>
        /// 初始化状态机
        /// </summary>
        public void InitFSM(string curStateName, params IStateNode[] stateList)
        {
            fsm = StateMachine.Create(ToString(), curStateName, stateList);
            CurState = curStateName;
        }

        /// <summary>
        /// 添加状态节点
        /// </summary>
        public bool AddFSMNode(IStateNode stateNode)
        {
            if (!CheckFSM())
            {
                Log.ErrorForce($"{ToString()} 状态机未初始化");
                return false;
            }

            return fsm.AddNode(stateNode);
        }

        /// <summary>
        /// 移除状态节点
        /// </summary>
        public bool RemoveFSMNode(string stateName)
        {
            if (!CheckFSM())
            {
                Log.ErrorForce($"{ToString()} 状态机未初始化");
                return false;
            }

            return fsm.RemoveNode(stateName);
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        public void SwitchState(string stateName, bool ignoreNextTick = true)
        {
            if (!CheckFSM())
            {
                Log.ErrorForce($"{ToString()} 状态机未初始化");
                return;
            }

            if (fsm.SwitchState(stateName, ignoreNextTick))
            {
                CurState = stateName;
            }
        }

        /// <summary>
        /// 强制切换状态
        /// </summary>
        public void ForceSwitchState(string stateName, bool ignoreNextTick = true)
        {
            if (!CheckFSM())
            {
                Log.ErrorForce($"{ToString()} 状态机未初始化");
                return;
            }

            if (fsm.ForceSwitchState(stateName, ignoreNextTick))
            {
                CurState = stateName;
            }
        }

        /// <summary>
        /// 是否处在状态
        /// </summary>
        public bool InState(string stateName)
        {
            if (!CheckFSM())
            {
                Log.ErrorForce($"{ToString()} 状态机未初始化");
                return false;
            }

            return fsm.InState(stateName);
        }

        /// <summary>
        /// 检查状态机是否初始化
        /// </summary>
        public bool CheckFSM()
        {
            return fsm != null;
        }

        #endregion FSM

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
            return Mathf.Abs(h) > Def.Actor.MIN_HAXIS_MOVE ||
                   Mathf.Abs(v) > Def.Actor.MIN_VAXIS_MOVE;
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
            float x = angle.x - my * param.mouseSpeedY * Time.deltaTime;
            if (x > 180) x -= 360;
            x = Mathf.Clamp(x, param.cameraMinRotate, param.cameraMaxRotate);

            cameraPoint.localRotation = Quaternion.Euler(
                    x,
                    angle.y + mx * param.mouseSpeedX * Time.deltaTime,
                    angle.z
                );
        }

        /// <summary>
        /// 更新相机坐标 (旋转后更新)
        /// </summary>
        public void UpdateCameraPointPos()
        {
            Transform cameraPoint = point.CameraPoint;
            point.CameraPoint.localPosition = (point.OriCameraPointLocPos - cameraPoint.forward * param.cameraToActorLen) +
                cameraPoint.forward * CameraOffsetZ + cameraPoint.right * CameraOffsetX + cameraPoint.up * CameraOffsetY;
        }

        #endregion Camera

        #region Animation

        /// <summary>
        /// 获取Animator
        /// </summary>
        public Animator GetAnimator()
        {
            if (animator == null)
            {
                ErrNoComponent(typeof(Animator));
            }

            return animator;
        }

        /// <summary>
        /// 动画状态转换
        /// </summary>
        public void AniCrossFade(string stateName, float transitionRate = 0.2f, int layer = -1)
        {
            if (!AniCanChangeState(stateName))
                return;
            CurAniName = stateName;
            GetAnimator()?.CrossFade(stateName, transitionRate, layer);
        }

        /// <summary>
        /// 当前是否可以转换为目标动画状态
        /// </summary>
        public bool AniCanChangeState(string stateName)
        {
            Assert.NotNullOrEmpty(stateName, "动画状态名不可为空");
            return !stateName.Equals(CurAniName);
        }

        #region IK

        /// <summary>
        /// 获取足部IK控制器
        /// </summary>
        public FootIKController GetFootIKCon()
        {
            if (footIKCon == null)
            {
                ErrNoComponent(typeof(FootIKController));
            }

            return footIKCon;
        }

        /// <summary>
        /// 设置足部IK权重
        /// </summary>
        public void SetFootIKGoal(float leftGoal, float rightGoal, bool immediately = false)
        {
            GetFootIKCon()?.SetGoal(leftGoal, rightGoal, immediately);
        }

        /// <summary>
        /// 设置左脚权重
        /// </summary>
        public void SetLeftFootIKGoal(float goal, bool immediately = false)
        {
            GetFootIKCon()?.SetLeftGoal(goal, immediately);
        }

        /// <summary>
        /// 设置右脚权重
        /// </summary>
        public void SetRightFootIKGoal(float goal, bool immediately = false)
        {
            GetFootIKCon()?.SetRightGoal(goal, immediately);
        }

        /// <summary>
        /// 通过动画曲线设置足部IK权重
        /// </summary>
        public void SetFootIKGoalByCurve()
        {
            Animator anim = GetAnimator();
            float leftGoal = anim.GetFloat(Def.Anim.LEFT_FOOT_IK_CURVE_PARAM);
            float rightGoal = anim.GetFloat(Def.Anim.RIGHT_FOOT_IK_CURVE_PARAM);
            SetFootIKGoal(leftGoal, rightGoal, true);
        }

        #endregion IK

        /// <summary>
        /// 设置OnAnimatorMove回调
        /// </summary>
        public void SetOnAnimatorMove(Action onAnimatorMove)
        {
            Animator animator = GetAnimator();
            if (animator == null)
            {
                return;
            }

            RootMotionController rootMotionCon = animator.gameObject.GetOrAddComponent<RootMotionController>();
            rootMotionCon.SetOnAnimatorMove(onAnimatorMove);
        }

        public void SetOnAnimatorMove(Action<Animator> onAnimatorMove)
        {
            Animator animator = GetAnimator();
            if (animator == null)
            {
                return;
            }

            RootMotionController rootMotionCon = animator.gameObject.GetOrAddComponent<RootMotionController>();

            if (onAnimatorMove == null)
            {
                rootMotionCon.SetOnAnimatorMove(null);
                return;
            }

            rootMotionCon.SetOnAnimatorMove(() =>
            {
                onAnimatorMove(animator);
            });
        }

        #endregion Animation

        #endregion Public Method

        #region Private Method

        /// <summary>
        /// 初始化Inspector配置数据
        /// </summary>
        private void InitMonoData()
        {
            //ActorParam
            param = GetComponent<ActorParam>();
            if (param == null)
            {
                ErrNoComponent(typeof(ActorParam), "添加默认ActorParam组件");
                param = gameObjectCache.AddComponent<ActorParam>();
            }

            //ActorPoint
            point = GetComponent<ActorPoint>();
            if (point == null)
            {
                ErrNoComponent(typeof(ActorPoint), "添加默认ActorPoint组件");
                point = gameObjectCache.AddComponent<ActorPoint>();
                point.AutoMatch();
            }
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitComponent()
        {
            OnInitComponent();
        }

        protected abstract void OnInitComponent();

        /// <summary>
        /// 下落加速
        /// </summary>
        protected abstract void UpdateFallSpeedUp();

        /// <summary>
        /// 更新是否着地状态
        /// </summary>
        private void UpdateGroundedState()
        {
            if (!UpdateGrounded)
                return;

            bool oldVal = Grounded;
            float rayGroundRadius = param.rayGroundRadius;
            Grounded = Physics.SphereCast(
                point.Root.position + Vector3.up * (rayGroundRadius + param.rayGroundOffsetY),
                rayGroundRadius,
                Vector3.down,
                out RaycastHit hitInfo,
                param.rayGroundLen,
                LayerUtil.OnlyLayer(Def.Layer.WORLD)
            );

            Normal = Grounded ? hitInfo.normal : Vector3.up;

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
            fsm?.Tick();
        }

        #endregion Private Method

        #region Lifecycle

        private void OnUpdate()
        {
            try
            {
                if (FallSpeedUp)
                {
                    UpdateFallSpeedUp();
                }

                UpdateGroundedState();

                OnUpdateSub();

                UpdateFSM();
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e);
            }
        }

        protected virtual void OnUpdateSub()
        {
        }

        private void OnFixedUpdate()
        {
            OnFixedUpdateSub();
        }

        protected virtual void OnFixedUpdateSub()
        {
        }

        protected virtual void Awake()
        {
            InitMonoData();
            InitComponent();

            Register.RegisterUpdate(OnUpdate);
            Register.RegisterFixedUpdate(OnFixedUpdate);
        }

        /// <summary>
        /// 报错未持有组件
        /// </summary>
        protected void ErrNoComponent(Type type, string msg = "")
        {
            Err($"该角色未持有`{type.Name}`组件，name: {gameObjectCache.name}。{msg}", true);
        }

        /// <summary>
        /// 报错
        /// </summary>
        protected void Err(string msg, bool force = true)
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
            return $"<ActorController: {gameObjectCache.name}>";
        }

        #endregion Lifecycle

        #region Gizmos

        protected virtual void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                Vector3 rootPos = point.Root.position;

                //触地检测射线
                if (UpdateGrounded)
                {
                    float rayGroundOffsetY = param.rayGroundOffsetY;
                    float rayGroundRadius = param.rayGroundRadius;
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(
                        rootPos + Vector3.up * (rayGroundRadius + rayGroundOffsetY),
                        rayGroundRadius
                    );
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(
                        rootPos + Vector3.up * rayGroundOffsetY,
                        rootPos + Vector3.up * (rayGroundOffsetY - param.rayGroundLen)
                    );
                }

                //相机挂点
                if (point.CameraPoint != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(point.CameraPoint.position, 0.15f);
                }
            }
        }

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

            if (footIKCon == null)
            {
                footIKCon = GetComponent<FootIKController>() ?? model.GetComponent<FootIKController>();
            }

            try
            {
                OnCollectComponent();
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, "收集组件时发生异常");
            }
        }

        protected abstract void OnCollectComponent();

        /// <summary>
        /// 获取Hierarchy显示信息
        /// </summary>
        public string GetHierarchyInfo()
        {
            return Pool.StringBuilderPool.Using((Func<StringBuilder, object>)((sb) =>
            {
                sb.AppendLine($"状态: {CurState}");
                sb.AppendLine($"动画状态: {CurAniName}");
                sb.AppendLine($"是否触地: {Grounded}");
                sb.AppendLine($"是否绑定相机: {CameraBinded}");

                OnGetHierarchyInfo(sb);

                return sb.ToString();
            })).ToString();
        }

        protected virtual void OnGetHierarchyInfo(StringBuilder sb)
        {
        }

        #endregion Editor
    }
}