using System;
using System.Collections.Generic;
using Duo1JFramework.Event;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 战斗系统全局管理器
    /// </summary>
    public class BattleSystem : MonoSingleton<BattleSystem>
    {
        /// <summary>
        /// 技能配置集合
        /// </summary>
        public SkillConfigLib SkillLib { get; private set; }

        /// <summary>
        /// 效果配置集合
        /// </summary>

        public EffectConfigLib EffectLib { get; private set; }

        /// <summary>
        /// 全局暂停 (为true时抑制战斗系统的Tick, 但不阻止外部接口调用)
        /// </summary>
        public bool IsPaused { get; set; }

        /// <summary>
        /// 战斗单位加入事件
        /// </summary>
        public event Action<CombatUnitController> OnUnitRegistered;

        /// <summary>
        /// 战斗单位离开事件
        /// </summary>
        public event Action<CombatUnitController> OnUnitUnregistered;

        /// <summary>
        /// 单位受到伤害事件 (由DamageEvent转发, 便于集中订阅)
        /// </summary>
        public event Action<DamageInfo> OnDamage;

        /// <summary>
        /// 单位死亡事件
        /// </summary>
        public event Action<CombatUnitController, CombatUnitController> OnDeath;

        /// <summary>
        /// 所有已注册的战斗单位
        /// </summary>
        public IReadOnlyList<CombatUnitController> Units => units;

        /// <summary>
        /// 已注册的战斗单位
        /// </summary>
        private readonly List<CombatUnitController> units = new List<CombatUnitController>();

        protected override void OnInit()
        {
            if (EventManager.TryGetInstance(out EventManager eventManager))
            {
                eventManager.RegisterType<CombatUnitCreatedEvent>(HandleUnitCreated);
                eventManager.RegisterType<CombatUnitDestroyedEvent>(HandleUnitDestroyed);
                eventManager.RegisterType<DamageEvent>(HandleDamage);
                eventManager.RegisterType<DeathEvent>(HandleDeath);
            }
        }

        protected override void OnDispose()
        {
            if (EventManager.TryGetInstance(out EventManager eventManager))
            {
                eventManager.UnRegisterType<CombatUnitCreatedEvent>(HandleUnitCreated);
                eventManager.UnRegisterType<CombatUnitDestroyedEvent>(HandleUnitDestroyed);
                eventManager.UnRegisterType<DamageEvent>(HandleDamage);
                eventManager.UnRegisterType<DeathEvent>(HandleDeath);
            }

            units.Clear();

            SkillLib = null;
            EffectLib = null;
            OnUnitRegistered = null;
            OnUnitUnregistered = null;
            OnDamage = null;
            OnDeath = null;
        }

        #region Config

        /// <summary>
        /// 设置技能配置集合
        /// </summary>
        public void SetSkillConfigLib(SkillConfigLib lib)
        {
            SkillLib = lib;
        }

        /// <summary>
        /// 设置效果配置集合
        /// </summary>
        public void SetEffectConfigLib(EffectConfigLib lib)
        {
            EffectLib = lib;
        }

        /// <summary>
        /// 查找技能配置
        /// </summary>
        public SkillConfig FindSkill(string id)
        {
            return SkillLib == null ? null : SkillLib.Find(id);
        }

        /// <summary>
        /// 查找效果配置
        /// </summary>
        public EffectConfig FindEffect(string id)
        {
            return EffectLib == null ? null : EffectLib.Find(id);
        }

        #endregion Config

        #region Unit

        /// <summary>
        /// 手动注册战斗单位 (CombatUnitController在OnEnable时会通过事件自动注册)
        /// </summary>
        public void RegisterUnit(CombatUnitController unit)
        {
            if (unit == null || units.Contains(unit))
            {
                return;
            }

            units.Add(unit);
            OnUnitRegistered?.Invoke(unit);
        }

        /// <summary>
        /// 手动注销战斗单位
        /// </summary>
        public void UnregisterUnit(CombatUnitController unit)
        {
            if (unit == null)
            {
                return;
            }

            if (units.Remove(unit))
            {
                OnUnitUnregistered?.Invoke(unit);
            }
        }

        /// <summary>
        /// 按阵营筛选
        /// </summary>
        public List<CombatUnitController> GetUnitsByFaction(int faction)
        {
            List<CombatUnitController> result = new List<CombatUnitController>();

            for (int i = 0; i < units.Count; i++)
            {
                if (units[i] != null && units[i].Faction == faction)
                {
                    result.Add(units[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取所有敌对单位
        /// </summary>
        public List<CombatUnitController> GetHostilesOf(CombatUnitController self)
        {
            List<CombatUnitController> result = new List<CombatUnitController>();

            if (self == null)
            {
                return result;
            }

            for (int i = 0; i < units.Count; i++)
            {
                if (units[i] != null && self.IsHostile(units[i]) && units[i].IsAlive)
                {
                    result.Add(units[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取所有友军
        /// </summary>
        public List<CombatUnitController> GetAlliesOf(CombatUnitController self, bool includeSelf = false)
        {
            List<CombatUnitController> result = new List<CombatUnitController>();

            if (self == null)
            {
                return result;
            }

            for (int i = 0; i < units.Count; i++)
            {
                CombatUnitController u = units[i];
                if (u == null || !u.IsAlive)
                {
                    continue;
                }

                if (u == self && !includeSelf)
                {
                    continue;
                }

                if (u.Faction == self.Faction)
                {
                    result.Add(u);
                }
            }

            return result;
        }

        /// <summary>
        /// 寻找距离self最近的敌对单位
        /// </summary>
        public CombatUnitController FindNearestHostile(CombatUnitController self, float maxDistance = float.MaxValue)
        {
            if (self == null)
            {
                return null;
            }

            CombatUnitController best = null;
            float bestSqr = maxDistance * maxDistance;
            UnityEngine.Vector3 origin = self.transform.position;

            for (int i = 0; i < units.Count; i++)
            {
                CombatUnitController u = units[i];

                if (u == null || !u.IsAlive)
                {
                    continue;
                }
                if (!self.IsHostile(u))
                {
                    continue;
                }

                float sqr = (u.transform.position - origin).sqrMagnitude;
                if (sqr < bestSqr)
                {
                    bestSqr = sqr;
                    best = u;
                }
            }

            return best;
        }

        #endregion Unit

        #region API

        /// <summary>
        /// 对目标施加效果
        /// </summary>
        public ActiveEffect ApplyEffectById(CombatUnitController target, string effectId, CombatUnitController source = null)
        {
            if (target == null)
            {
                return null;
            }

            EffectConfig effectConfig = FindEffect(effectId);
            if (effectConfig == null)
            {
                Log.Warn($"[BattleSystem] 找不到效果 `{effectId}`");
                return null;
            }

            return ApplyEffect(target, effectConfig, source);
        }

        /// <summary>
        /// 对目标施加效果
        /// </summary>
        public ActiveEffect ApplyEffect(CombatUnitController target, EffectConfig effectConfig, CombatUnitController source = null)
        {
            if (target == null || effectConfig == null)
            {
                return null;
            }

            return target.Effects.Apply(effectConfig, source);
        }

        /// <summary>
        /// 让指定单位激活能力
        /// </summary>
        public bool ActivateAbility(CombatUnitController caster, string abilityId, object param = null)
        {
            if (caster == null)
            {
                return false;
            }

            return caster.Abilities.Activate(abilityId, param);
        }

        /// <summary>
        /// 让指定单位按输入槽激活能力
        /// </summary>
        public bool ActivateAbilityByInput(CombatUnitController caster, EAbilityInputId inputId, object param = null)
        {
            if (caster == null)
            {
                return false;
            }

            return caster.Abilities.ActivateByInput(inputId, param);
        }

        /// <summary>
        /// 直接对目标造成伤害
        /// </summary>
        public void DealDamage(CombatUnitController target, float damage, CombatUnitController source = null, string sourceId = null, bool isCritical = false)
        {
            if (target == null)
            {
                return;
            }

            target.TakeDamage(damage, source, sourceId ?? BattleDef.ExternalDamageSourceId, isCritical);
        }

        #endregion API

        #region Event Handler

        private void HandleUnitCreated(CombatUnitCreatedEvent e)
        {
            if (e == null || e.Unit == null)
            {
                return;
            }

            if (!units.Contains(e.Unit))
            {
                units.Add(e.Unit);
                OnUnitRegistered?.Invoke(e.Unit);
            }
        }

        private void HandleUnitDestroyed(CombatUnitDestroyedEvent e)
        {
            if (e == null || e.Unit == null)
            {
                return;
            }

            if (units.Remove(e.Unit))
            {
                OnUnitUnregistered?.Invoke(e.Unit);
            }
        }

        private void HandleDamage(DamageEvent e)
        {
            if (e != null)
            {
                OnDamage?.Invoke(e.Info);
            }
        }

        private void HandleDeath(DeathEvent e)
        {
            if (e != null)
            {
                OnDeath?.Invoke(e.Unit, e.Killer);
            }
        }

        #endregion Event Handler
    }
}