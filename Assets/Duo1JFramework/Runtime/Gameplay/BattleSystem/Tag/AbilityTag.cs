using System;
using System.Collections.Generic;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 战斗标签，支持点分层级 
    /// 如 Combat.Skill、Combat.Skill.Melee
    /// </summary>
    [Serializable]
    public struct AbilityTag : IEquatable<AbilityTag>
    {
        /// <summary>
        /// 空标签
        /// </summary>
        public static readonly AbilityTag None = new AbilityTag(null);

        /// <summary>
        /// 标签全名
        /// </summary>
        public string Name;

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid => !string.IsNullOrEmpty(Name);

        public AbilityTag(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 是否与目标标签匹配
        /// </summary>
        public bool Matches(AbilityTag other)
        {
            if (!IsValid || !other.IsValid)
            {
                return false;
            }

            if (Name == other.Name)
            {
                return true;
            }

            return Name.StartsWith(other.Name + ".");
        }

        public bool Equals(AbilityTag other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is AbilityTag other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Name == null ? 0 : Name.GetHashCode();
        }

        public override string ToString()
        {
            return IsValid ? Name : "<TagNone>";
        }

        public static bool operator ==(AbilityTag a, AbilityTag b) => a.Equals(b);
        public static bool operator !=(AbilityTag a, AbilityTag b) => !a.Equals(b);

        public static implicit operator AbilityTag(string name) => new AbilityTag(name);
    }

    /// <summary>
    /// 标签比较器
    /// </summary>
    public sealed class AbilityTagComparer : IEqualityComparer<AbilityTag>
    {
        public static readonly AbilityTagComparer Default = new AbilityTagComparer();

        public bool Equals(AbilityTag x, AbilityTag y) => x.Equals(y);

        public int GetHashCode(AbilityTag obj) => obj.GetHashCode();
    }
}
