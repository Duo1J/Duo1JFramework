using System;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 动画片段
    /// </summary>
    [Serializable]
    public class AnimationSegment : SequenceSegment
    {
        /// <summary>
        /// 动画状态名
        /// </summary>
        public string StateName;

        /// <summary>
        /// 淡入时间
        /// </summary>
        public float CrossFade = 0.1f;

        /// <summary>
        /// 层级
        /// </summary>
        public int Layer = -1;

        public override void OnEnter(SkillContext ctx)
        {
            if (ctx == null || ctx.Owner == null || string.IsNullOrEmpty(StateName))
            {
                return;
            }

            Animator anim = ctx.Owner.Animator;
            if (anim == null)
            {
                return;
            }

            anim.CrossFade(StateName, CrossFade, Layer);
        }
    }
}
