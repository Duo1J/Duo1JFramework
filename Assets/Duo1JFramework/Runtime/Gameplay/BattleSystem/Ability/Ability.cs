using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 战斗能力基类
    /// </summary>
    public abstract class Ability
    {
        /// <summary>
        /// 唯一Id
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// 显示名
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 输入槽
        /// </summary>
        public EAbilityInputId InputId { get; protected set; } = EAbilityInputId.None;

        /// <summary>
        /// 消耗法力
        /// </summary>
        public float CostMana { get; protected set; }

        /// <summary>
        /// 冷却时间(s)
        /// </summary>
        public float Cooldown { get; protected set; }

        /// <summary>
        /// 需要目标持有的标签
        /// </summary>
        public List<AbilityTag> ActivationRequiredTags { get; } = new List<AbilityTag>();

        /// <summary>
        /// 被以下任一标签阻止激活
        /// </summary>
        public List<AbilityTag> ActivationBlockedTags { get; } = new List<AbilityTag>();

        /// <summary>
        /// 激活时给予的标签 (激活期间存在)
        /// </summary>
        public List<AbilityTag> ActivationOwnedTags { get; } = new List<AbilityTag>();

        /// <summary>
        /// 归属能力系统
        /// </summary>
        public AbilitySystem System { get; internal set; }

        /// <summary>
        /// 归属角色
        /// </summary>
        public CombatUnitController Owner => System == null ? null : System.Owner;

        /// <summary>
        /// 当前状态
        /// </summary>
        public EAbilityState State { get; internal set; } = EAbilityState.Idle;

        /// <summary>
        /// 冷却结束时的时间戳(s)
        /// </summary>
        public float CooldownEndTime { get; internal set; }

        /// <summary>
        /// 激活开始时的时间戳(s)
        /// </summary>
        public float ActiveStartTime { get; internal set; }

        /// <summary>
        /// 冷却剩余(s)
        /// </summary>
        public float CooldownRemain
        {
            get
            {
                if (State != EAbilityState.Cooldown)
                {
                    return 0f;
                }

                float remain = CooldownEndTime - Time.time;
                return remain > 0f ? remain : 0f;
            }
        }

        /// <summary>
        /// 激活运行时长(s)
        /// </summary>
        public float ActiveTime
        {
            get
            {
                if (State != EAbilityState.Active)
                {
                    return 0f;
                }

                return Time.time - ActiveStartTime;
            }
        }

        /// <summary>
        /// 校验是否可激活
        /// </summary>
        public virtual bool CanActivate()
        {
            if (State != EAbilityState.Idle)
            {
                return false;
            }

            if (Owner == null || !Owner.IsAlive)
            {
                return false;
            }

            if (CostMana > 0f && Owner.Attributes.GetValue(EAttribute.Mana) < CostMana)
            {
                return false;
            }

            foreach (AbilityTag tag in ActivationBlockedTags)
            {
                if (Owner.Tags.HasTag(tag))
                {
                    return false;
                }
            }

            foreach (AbilityTag tag in ActivationRequiredTags)
            {
                if (!Owner.Tags.HasTag(tag))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 激活能力
        /// </summary>
        public bool Activate(object param = null)
        {
            if (!CanActivate())
            {
                return false;
            }

            if (CostMana > 0f)
            {
                Owner.Attributes.Modify(EAttribute.Mana, EAttributeModifierOp.Add, -CostMana);
            }

            State = EAbilityState.Active;
            ActiveStartTime = Time.time;

            foreach (AbilityTag tag in ActivationOwnedTags)
            {
                Owner.Tags.Add(tag);
            }

            try
            {
                OnActivate(param);
            }
            catch (System.Exception e)
            {
                Assert.ExceptHandle(e, $"Ability激活异常: {Id}");
                EndAbility();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Tick()
        {
            if (State == EAbilityState.Cooldown)
            {
                if (Time.time >= CooldownEndTime)
                {
                    State = EAbilityState.Idle;
                }
            }

            if (State == EAbilityState.Active)
            {
                OnTick();
            }
        }

        /// <summary>
        /// 结束能力
        /// </summary>
        public void EndAbility()
        {
            if (State != EAbilityState.Active)
            {
                return;
            }

            try
            {
                OnEnd();
            }
            catch (System.Exception e)
            {
                Assert.ExceptHandle(e, $"Ability结束异常: {Id}");
            }

            foreach (AbilityTag tag in ActivationOwnedTags)
            {
                Owner.Tags.Remove(tag);
            }

            if (Cooldown > 0f)
            {
                CooldownEndTime = Time.time + Cooldown;
                State = EAbilityState.Cooldown;
            }
            else
            {
                State = EAbilityState.Idle;
            }
        }

        /// <summary>
        /// 打断能力
        /// </summary>
        public void CancelAbility()
        {
            if (State != EAbilityState.Active)
            {
                return;
            }

            try
            {
                OnCancel();
            }
            catch (System.Exception e)
            {
                Assert.ExceptHandle(e, $"Ability打断异常: {Id}");
            }

            EndAbility();
        }

        /// <summary>
        /// 激活时执行
        /// </summary>
        protected abstract void OnActivate(object param);

        /// <summary>
        /// 每帧执行
        /// </summary>
        protected virtual void OnTick()
        {
        }

        /// <summary>
        /// 正常结束
        /// </summary>
        protected virtual void OnEnd()
        {
        }

        /// <summary>
        /// 强制打断
        /// </summary>
        protected virtual void OnCancel()
        {
        }

        public override string ToString()
        {
            return $"<Ability-{Id}-{State}>";
        }
    }
}