using Duo1JFramework.AnimationAPI;
using System;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色控制器基类 - Animation
    /// </summary>
    public abstract partial class BaseActorController
    {
        /// <summary>
        /// 角色动画控制器
        /// </summary>
        [SerializeField]
        protected Animator animator;

        /// <summary>
        /// 当前播放的动画名
        /// </summary>
        public string CurAniName { get; private set; }

        /// <summary>
        /// 足部IK控制器
        /// </summary>
        [SerializeField]
        protected FootIKController footIKCon;

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
        public void AnimCrossFade(string stateName, float transitionRate = 0.2f, int layer = -1)
        {
            if (!AnimCanChangeState(stateName))
            {
                return;
            }

            CurAniName = stateName;
            GetAnimator()?.CrossFade(stateName, transitionRate, layer);
        }

        /// <summary>
        /// 当前是否可以转换为目标动画状态
        /// </summary>
        public bool AnimCanChangeState(string stateName)
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

        /// <summary>
        /// 设置OnAnimatorMove回调
        /// </summary>
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
    }
}
