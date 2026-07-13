using System.Collections.Generic;
using Duo1JFramework.Actor;
using Duo1JFramework.Event;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 战斗单位控制器
    /// </summary>
    [DisallowMultipleComponent]
    public class CombatUnitController : MonoRegister
    {
        /// <summary>
        /// 阵营Id
        /// </summary>
        [SerializeField]
        private int faction = 0;

        /// <summary>
        /// 初始属性配置
        /// </summary>
        [SerializeField]
        private List<CombatAttributeEntry> initialAttributes = new List<CombatAttributeEntry>();

        /// <summary>
        /// 起始技能配置
        /// </summary>
        [SerializeField]
        private List<SkillConfig> initialSkills = new List<SkillConfig>();

        /// <summary>
        /// 阵营
        /// </summary>
        public int Faction => faction;

        /// <summary>
        /// 是否存活
        /// </summary>
        public bool IsAlive => Attributes != null && Attributes.GetValue(EAttribute.Health) > 0f;

        /// <summary>
        /// 属性集
        /// </summary>
        public AttributeSet Attributes { get; private set; }

        /// <summary>
        /// 标签容器
        /// </summary>
        public AbilityTagContainer Tags { get; private set; }

        /// <summary>
        /// 能力系统
        /// </summary>
        public AbilitySystem Abilities { get; private set; }

        /// <summary>
        /// 效果系统
        /// </summary>
        public EffectSystem Effects { get; private set; }

        /// <summary>
        /// 关联的ActorController
        /// </summary>
        public BaseActorController ActorController { get; private set; }

        /// <summary>
        /// 获取Animator
        /// </summary>
        public Animator Animator => ActorController != null ? ActorController.GetAnimator() : null;

        protected virtual void Awake()
        {
            ActorController = GetComponent<BaseActorController>();
            if (ActorController == null)
            {
                ActorController = GetComponentInChildren<BaseActorController>();
                if (ActorController == null)
                {
                    Log.Error($"[CombatUnitController] ActorController not found in gameObject: {gameObject.name}");
                }
            }

            Attributes = new AttributeSet();
            Tags = new AbilityTagContainer();
            Abilities = new AbilitySystem(this);
            Effects = new EffectSystem(this);

            for (int i = 0; i < initialAttributes.Count; i++)
            {
                CombatAttributeEntry e = initialAttributes[i];
                Attributes.Register(e.Type, e.BaseValue, e.Min, e.Max <= 0f ? float.MaxValue : e.Max);
            }

            for (int i = 0; i < initialSkills.Count; i++)
            {
                if (initialSkills[i] == null) continue;
                Abilities.Grant(new SkillAbility(initialSkills[i]));
            }

            Attributes.OnAttributeChanged += HandleAttributeChanged;

            Reg.RegisterUpdate(OnUpdate);
        }

        protected virtual void OnEnable()
        {
            if (EventManager.TryGetInstance(out EventManager eventManager))
            {
                eventManager.BroadcastType(new CombatUnitCreatedEvent(this));
            }
        }

        protected virtual void OnDisable()
        {
            if (EventManager.TryGetInstance(out EventManager eventManager))
            {
                eventManager.BroadcastType(new CombatUnitDestroyedEvent(this));
            }
        }

        protected override void OnDestroy()
        {
            if (Abilities != null)
            {
                Abilities.Clear();
            }

            if (Effects != null)
            {
                Effects.Clear();
            }

            base.OnDestroy();
        }

        private void OnUpdate()
        {
            Effects?.Tick();
            Abilities?.Tick();
        }

        /// <summary>
        /// 是否为敌对
        /// </summary>
        public bool IsHostile(CombatUnitController other)
        {
            if (other == null || other == this)
            {
                return false;
            }

            return other.faction != faction;
        }

        /// <summary>
        /// 通知受到伤害/治疗
        /// </summary>
        internal void NotifyDamageTaken(DamageInfo info)
        {
            if (EventManager.TryGetInstance(out EventManager eventManager))
            {
                eventManager.BroadcastType(new DamageEvent(info));
            }
        }

        /// <summary>
        /// 强制受到伤害
        /// </summary>
        public void TakeDamage(float damage, CombatUnitController source, string sourceId = BattleDef.ExternalDamageSourceId, bool isCritical = false)
        {
            if (!IsAlive)
            {
                return;
            }

            Attributes.Modify(EAttribute.Health, EAttributeModifierOp.Add, -damage);
            NotifyDamageTaken(new DamageInfo(source, this, damage, sourceId, isCritical));
        }

        private void HandleAttributeChanged(EAttribute type, float oldVal, float newVal)
        {
            if (type == EAttribute.Health && oldVal > 0f && newVal <= 0f)
            {
                Die(null);
            }
        }

        /// <summary>
        /// 死亡
        /// </summary>
        public virtual void Die(CombatUnitController killer)
        {
            Abilities?.CancelAllActive();
            Tags.Add(new AbilityTag("State.Dead"));
            if (EventManager.TryGetInstance(out EventManager em))
            {
                em.BroadcastType(new DeathEvent(this, killer));
            }
        }
    }

    /// <summary>
    /// 属性初始配置项
    /// </summary>
    [System.Serializable]
    public class CombatAttributeEntry
    {
        public EAttribute Type;
        public float BaseValue;
        public float Min = 0f;
        public float Max = 0f;
    }
}
