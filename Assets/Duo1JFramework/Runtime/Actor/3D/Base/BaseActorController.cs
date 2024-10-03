using Duo1JFramework.AnimationAPI;
using Duo1JFramework.ObjectPool;
using Duo1JFramework.World;
using System;
using System.Text;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色控制器基类
    /// </summary>
    [RequireComponent(typeof(ActorParam), typeof(ActorPoint))]
    public abstract partial class BaseActorController : WorldItem
    {
        /// <summary>
        /// 角色参数
        /// </summary>
        public ActorParam Param => param;
        private ActorParam param;

        /// <summary>
        /// 角色挂点
        /// </summary>
        public ActorPoint Point => point;
        private ActorPoint point;

        /// <summary>
        /// 角色逻辑
        /// </summary>
        public BaseActor Logic { get; set; }

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

        private void OnFixedUpdate()
        {
            OnFixedUpdateSub();
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

                GetHierarchyInfoSub(sb);

                return sb.ToString();
            })).ToString();
        }

        #endregion Editor
    }
}
