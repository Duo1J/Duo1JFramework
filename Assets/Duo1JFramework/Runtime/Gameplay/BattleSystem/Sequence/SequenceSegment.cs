using System;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 序列片段
    /// </summary>
    [Serializable]
    public abstract class SequenceSegment
    {
        /// <summary>
        /// 片段名
        /// </summary>
        [SerializeField]
        public string Name;

        /// <summary>
        /// 起始时间(s)
        /// </summary>
        [SerializeField]
        public float StartTime;

        /// <summary>
        /// 结束时间(s), <=StartTime时为瞬时
        /// </summary>
        [SerializeField]
        public float EndTime;

        /// <summary>
        /// 是否已进入
        /// </summary>
        [NonSerialized]
        public bool Entered;

        /// <summary>
        /// 持续时长
        /// </summary>
        public float Duration => Mathf.Max(0f, EndTime - StartTime);

        /// <summary>
        /// 是否为瞬时片段
        /// </summary>
        public bool IsInstant => EndTime <= StartTime;

        /// <summary>
        /// 重置运行时状态
        /// </summary>
        public virtual void Reset()
        {
            Entered = false;
        }

        /// <summary>
        /// 进入片段
        /// </summary>
        public virtual void OnEnter(SkillContext ctx)
        {
        }

        /// <summary>
        /// 更新, localTime相对片段起始时间
        /// </summary>
        public virtual void OnUpdate(SkillContext ctx, float localTime)
        {
        }

        /// <summary>
        /// 退出片段
        /// </summary>
        public virtual void OnExit(SkillContext ctx)
        {
        }
    }
}
