using System;
using UnityEngine;

namespace Duo1JFramework.AnimationAPI
{
    /// <summary>
    /// Root Motion数据
    /// </summary>
    public struct RootMotionData
    {
        public Vector3 DeltaPosition;

        public Quaternion DeltaRotation;

        public Vector3 RootPosition;

        public Quaternion RootRotation;

        public RootMotionData(Animator animator)
        {
            DeltaPosition = animator.deltaPosition;
            DeltaRotation = animator.deltaRotation;
            RootPosition = animator.rootPosition;
            RootRotation = animator.rootRotation;
        }
    }

    /// <summary>
    /// Root Motion控制器
    /// </summary>
    [DisallowMultipleComponent]
    public class RootMotionController : BaseMono
    {
        private Animator animator;
        private Action onAnimatorMove;
        private Action<RootMotionData> onRootMotion;

        /// <summary>
        /// 是否启用Root Motion回调
        /// </summary>
        public bool Enable { get; set; } = true;

        private Animator Animator
        {
            get
            {
                if (animator == null)
                {
                    animator = GetComponent<Animator>();
                }

                return animator;
            }
        }

        /// <summary>
        /// 设置OnAnimatorMove回调
        /// </summary>
        public void SetOnAnimatorMove(Action onAnimatorMove)
        {
            this.onAnimatorMove = onAnimatorMove;
        }

        /// <summary>
        /// 设置Root Motion回调
        /// </summary>
        public void SetOnRootMotion(Action<RootMotionData> onRootMotion)
        {
            this.onRootMotion = onRootMotion;
        }

        /// <summary>
        /// 清理OnAnimatorMove回调
        /// </summary>
        public void ClearOnAnimatorMove()
        {
            onAnimatorMove = null;
        }

        /// <summary>
        /// 清理Root Motion回调
        /// </summary>
        public void ClearOnRootMotion()
        {
            onRootMotion = null;
        }

        private void OnAnimatorMove()
        {
            if (!Enable)
            {
                return;
            }

            onAnimatorMove?.Invoke();

            if (onRootMotion != null && Animator != null)
            {
                onRootMotion.Invoke(new RootMotionData(Animator));
            }
        }
    }
}
