using Duo1JFramework.Event;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 伤害信息
    /// </summary>
    public struct DamageInfo
    {
        /// <summary>
        /// 施加者
        /// </summary>
        public CombatUnitController Source;

        /// <summary>
        /// 目标
        /// </summary>
        public CombatUnitController Target;

        /// <summary>
        /// 伤害值 (>0 为伤害, <0 为治疗)
        /// </summary>
        public float Value;

        /// <summary>
        /// 来源标识 (效果Id或技能Id)
        /// </summary>
        public string SourceId;

        /// <summary>
        /// 是否暴击
        /// </summary>
        public bool IsCritical;

        public DamageInfo(CombatUnitController source, CombatUnitController target, float value, string sourceId, bool isCritical = false)
        {
            Source = source;
            Target = target;
            Value = value;
            SourceId = sourceId;
            IsCritical = isCritical;
        }
    }

    /// <summary>
    /// 战斗单位创建事件
    /// </summary>
    public class CombatUnitCreatedEvent : BaseTypeEvent
    {
        public CombatUnitController Unit;

        public CombatUnitCreatedEvent(CombatUnitController unit)
        {
            Unit = unit;
        }
    }

    /// <summary>
    /// 战斗单位销毁事件
    /// </summary>
    public class CombatUnitDestroyedEvent : BaseTypeEvent
    {
        public CombatUnitController Unit;

        public CombatUnitDestroyedEvent(CombatUnitController unit)
        {
            Unit = unit;
        }
    }

    /// <summary>
    /// 伤害事件
    /// </summary>
    public class DamageEvent : BaseTypeEvent
    {
        public DamageInfo Info;

        public DamageEvent(DamageInfo info)
        {
            Info = info;
        }
    }

    /// <summary>
    /// 死亡事件
    /// </summary>
    public class DeathEvent : BaseTypeEvent
    {
        public CombatUnitController Unit;
        public CombatUnitController Killer;

        public DeathEvent(CombatUnitController unit, CombatUnitController killer)
        {
            Unit = unit;
            Killer = killer;
        }
    }
}