using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 战斗效果系统
    /// </summary>
    public class EffectSystem
    {
        /// <summary>
        /// 归属战斗单位
        /// </summary>
        public CombatUnitController Owner { get; }

        /// <summary>
        /// 生效中的Effect列表
        /// </summary>
        private readonly List<ActiveEffect> actives = new List<ActiveEffect>();

        /// <summary>
        /// 待移除列表
        /// </summary>
        private readonly List<ActiveEffect> pendingRemove = new List<ActiveEffect>();

        /// <summary>
        /// 自增Handle
        /// </summary>
        private long handleInc = 0;

        /// <summary>
        /// 效果生效事件
        /// </summary>
        public event Action<ActiveEffect> OnEffectApplied;

        /// <summary>
        /// 效果移除事件
        /// </summary>
        public event Action<ActiveEffect> OnEffectRemoved;

        /// <summary>
        /// 效果周期触发事件
        /// </summary>
        public event Action<ActiveEffect> OnEffectExecuted;

        public EffectSystem(CombatUnitController owner)
        {
            Owner = owner;
        }

        /// <summary>
        /// 应用效果
        /// </summary>
        public ActiveEffect Apply(EffectConfig effectConfig, CombatUnitController source)
        {
            if (effectConfig == null)
            {
                return null;
            }

            if (!CheckApplyCondition(effectConfig))
            {
                return null;
            }

            if (effectConfig.MaxStack > 1)
            {
                ActiveEffect exist = FindActive(effectConfig.Id);
                if (exist != null)
                {
                    exist.StackCount = Math.Min(exist.StackCount + 1, effectConfig.MaxStack);
                    if (effectConfig.DurationPolicy == EEffectDuration.Duration)
                    {
                        exist.ExpireTime = Time.time + effectConfig.Duration;
                    }
                    return exist;
                }
            }

            ActiveEffect active = new ActiveEffect(effectConfig, source, Owner)
            {
                Handle = ++handleInc
            };

            switch (effectConfig.DurationPolicy)
            {
                case EEffectDuration.Instant:
                    ExecuteModifiers(active, true);
                    OnEffectApplied?.Invoke(active);
                    OnEffectRemoved?.Invoke(active);
                    return active;

                case EEffectDuration.Duration:
                case EEffectDuration.Infinite:
                    active.ExpireTime = effectConfig.DurationPolicy == EEffectDuration.Infinite
                        ? float.PositiveInfinity
                        : Time.time + effectConfig.Duration;
                    active.NextPeriodTime = effectConfig.Period > 0f ? Time.time + effectConfig.Period : float.PositiveInfinity;
                    actives.Add(active);
                    foreach (AbilityTag tag in effectConfig.GrantedTags)
                    {
                        Owner.Tags.Add(tag);
                    }
                    ExecuteModifiers(active, false);
                    OnEffectApplied?.Invoke(active);
                    return active;
            }

            return null;
        }

        /// <summary>
        /// 通过Handle移除
        /// </summary>
        public bool Remove(long handle)
        {
            for (int i = 0; i < actives.Count; i++)
            {
                if (actives[i].Handle == handle)
                {
                    MarkRemove(actives[i]);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 通过Id移除
        /// </summary>
        public int RemoveById(string id)
        {
            int cnt = 0;
            for (int i = 0; i < actives.Count; i++)
            {
                if (actives[i].Config.Id == id)
                {
                    MarkRemove(actives[i]);
                    cnt++;
                }
            }
            return cnt;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Tick()
        {
            float now = Time.time;

            for (int i = 0; i < actives.Count; i++)
            {
                ActiveEffect a = actives[i];
                if (a.Expired)
                {
                    continue;
                }

                if (a.Config.Period > 0f)
                {
                    while (now >= a.NextPeriodTime)
                    {
                        a.NextPeriodTime += a.Config.Period;
                        ExecuteModifiers(a, true);
                        OnEffectExecuted?.Invoke(a);
                        if (a.Expired)
                        {
                            break;
                        }
                    }
                }

                if (a.Config.DurationPolicy == EEffectDuration.Duration)
                {
                    if (now >= a.ExpireTime)
                    {
                        MarkRemove(a);
                    }
                }
            }

            FlushPendingRemove();
        }

        /// <summary>
        /// 清空所有
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < actives.Count; i++)
            {
                MarkRemove(actives[i]);
            }

            FlushPendingRemove();
        }

        /// <summary>
        /// 是否存在指定Id的Effect
        /// </summary>
        public bool HasEffect(string id)
        {
            return FindActive(id) != null;
        }

        private ActiveEffect FindActive(string id)
        {
            for (int i = 0; i < actives.Count; i++)
            {
                if (actives[i].Config.Id == id)
                {
                    return actives[i];
                }
            }
            return null;
        }

        private bool CheckApplyCondition(EffectConfig effectConfig)
        {
            if (effectConfig.BlockedByTags != null && effectConfig.BlockedByTags.Count > 0)
            {
                foreach (AbilityTag tag in effectConfig.BlockedByTags)
                {
                    if (Owner.Tags.HasTag(tag))
                    {
                        return false;
                    }
                }
            }

            if (effectConfig.RequiredTags != null && effectConfig.RequiredTags.Count > 0)
            {
                foreach (AbilityTag tag in effectConfig.RequiredTags)
                {
                    if (!Owner.Tags.HasTag(tag))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void ExecuteModifiers(ActiveEffect active, bool asBase)
        {
            float damageSum = 0f;
            foreach (EffectModifier mod in active.Config.Modifiers)
            {
                AttributeValue av = Owner.Attributes.Get(mod.Attribute);
                if (av == null)
                {
                    continue;
                }

                float magnitude = mod.Magnitude * active.StackCount;
                if (asBase && mod.Op == EAttributeModifierOp.Add)
                {
                    float prev = av.CurrentValue;
                    av.SetBaseValue(av.BaseValue + magnitude);
                    av.SetCurrentValue(prev + magnitude);
                }
                else
                {
                    av.ApplyModifier(mod.Op, magnitude);
                }

                if (mod.AsDamage)
                {
                    damageSum += magnitude;
                }
            }

            if (Math.Abs(damageSum) > 1e-6f)
            {
                Owner.NotifyDamageTaken(new DamageInfo(active.Source, Owner, -damageSum, active.Config.Id));
            }
        }

        private void MarkRemove(ActiveEffect a)
        {
            if (a.Expired)
            {
                return;
            }
            a.Expired = true;
            pendingRemove.Add(a);
        }

        private void FlushPendingRemove()
        {
            if (pendingRemove.Count == 0)
            {
                return;
            }
            for (int i = 0; i < pendingRemove.Count; i++)
            {
                ActiveEffect a = pendingRemove[i];
                actives.Remove(a);

                foreach (AbilityTag tag in a.Config.GrantedTags)
                {
                    Owner.Tags.Remove(tag);
                }

                OnEffectRemoved?.Invoke(a);
            }
            pendingRemove.Clear();
        }
    }
}
