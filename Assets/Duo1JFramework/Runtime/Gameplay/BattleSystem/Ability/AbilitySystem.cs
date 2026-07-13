using System;
using System.Collections.Generic;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 战斗能力系统
    /// </summary>
    public class AbilitySystem
    {
        /// <summary>
        /// 归属战斗单位
        /// </summary>
        public CombatUnitController Owner { get; }

        /// <summary>
        /// 获取所有能力
        /// </summary>
        public IReadOnlyList<Ability> Abilities => abilities;

        /// <summary>
        /// 已授予的能力列表
        /// </summary>
        private readonly List<Ability> abilities = new List<Ability>();

        /// <summary>
        /// 能力激活事件
        /// </summary>
        public event Action<Ability> OnAbilityActivated;

        /// <summary>
        /// 能力结束事件
        /// </summary>
        public event Action<Ability> OnAbilityEnded;

        public AbilitySystem(CombatUnitController owner)
        {
            Owner = owner;
        }

        /// <summary>
        /// 授予能力
        /// </summary>
        public Ability Grant(Ability ability)
        {
            if (ability == null)
            {
                return null;
            }

            if (Find(ability.Id) != null)
            {
                Log.Warn($"[AbilitySystem] 重复授予能力 `{ability.Id}`");
                return null;
            }

            ability.System = this;
            abilities.Add(ability);
            return ability;
        }

        /// <summary>
        /// 移除能力
        /// </summary>
        public bool Revoke(string id)
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                if (abilities[i].Id == id)
                {
                    Ability a = abilities[i];
                    a.CancelAbility();
                    a.System = null;
                    abilities.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 通过Id激活能力
        /// </summary>
        public bool Activate(string id, object param = null)
        {
            Ability a = Find(id);
            if (a == null)
            {
                return false;
            }

            return DoActivate(a, param);
        }

        /// <summary>
        /// 通过输入槽激活能力
        /// </summary>
        public bool ActivateByInput(EAbilityInputId inputId, object param = null)
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                if (abilities[i].InputId == inputId)
                {
                    if (DoActivate(abilities[i], param))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 打断能力
        /// </summary>
        public bool Cancel(string id)
        {
            Ability a = Find(id);
            if (a == null)
            {
                return false;
            }

            a.CancelAbility();
            return true;
        }

        /// <summary>
        /// 打断当前激活中的所有能力
        /// </summary>
        public void CancelAllActive()
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                if (abilities[i].State == EAbilityState.Active)
                {
                    abilities[i].CancelAbility();
                }
            }
        }

        /// <summary>
        /// 查询能力
        /// </summary>
        public Ability Find(string id)
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                if (abilities[i].Id == id)
                {
                    return abilities[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Tick()
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                Ability a = abilities[i];
                EAbilityState pre = a.State;
                a.Tick();

                if (pre == EAbilityState.Active && a.State != EAbilityState.Active)
                {
                    OnAbilityEnded?.Invoke(a);
                }
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                abilities[i].CancelAbility();
                abilities[i].System = null;
            }
            abilities.Clear();
        }

        private bool DoActivate(Ability a, object param)
        {
            if (a.Activate(param))
            {
                OnAbilityActivated?.Invoke(a);
                return true;
            }
            return false;
        }
    }
}