using Duo1JFramework.GamerInput;
using System;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色控制器基类 - Transform
    /// </summary>
    public abstract partial class BaseActorController
    {
        /// <summary>
        /// 角色模型
        /// </summary>
        public GameObject Model => model;

        [SerializeField]
        private GameObject model;

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
                if (CameraBinded && point != null && point.TryGetCameraPoint(out Transform cameraPoint))
                {
                    return Vector3.Cross(cameraPoint.right, Vector3.up).normalized;
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
                if (CameraBinded && point != null && point.TryGetCameraPoint(out Transform cameraPoint))
                {
                    return cameraPoint.right.normalized;
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

        /// <summary>
        /// 是否更新着地状态
        /// </summary>
        public bool UpdateGrounded { get; set; } = true;

        /// <summary>
        /// 落地状态改变回调
        /// </summary>
        public Action<bool> OnGroundedChange;

        /// <summary>
        /// 是否下落速度增加
        /// </summary>
        public bool FallSpeedUp { get; set; } = false;

        /// <summary>
        /// 子类实现下落加速
        /// </summary>
        protected abstract void UpdateFallSpeedUp();

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
        public bool CheckAxisBlind(float h, float v)
        {
            return InputManager.CheckAxisBlind(h, v);
        }

        /// <summary>
        /// 轴为0检验
        /// </summary>
        public bool CheckAxisZero(float h, float v)
        {
            return h == 0 && v == 0;
        }

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
                LayerUtil.OnlyOpenLayer(Def.Layer.WORLD)
            );

            Normal = Grounded ? hitInfo.normal : Vector3.up;

            if (oldVal != Grounded)
            {
                OnGroundedChange?.Invoke(Grounded);
            }
        }

        /// <summary>
        /// 通过轴设置旋转 (以目视Forward为参考系旋转朝前)
        /// </summary>
        public void RotateByAxis(float h, float v)
        {
            if (CheckAxisZero(h, v))
            {
                return;
            }

            Vector3 forward = RotateGo.transform.forward;
            Vector3 axisByEye = GetAxisByEye(h, v);

            RotateGo.transform.forward = Vector3.Slerp(
                forward,
                axisByEye,
                param.rotateSpeed * Time.deltaTime
            );
        }
    }
}
