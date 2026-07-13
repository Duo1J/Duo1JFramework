using System;
using System.Collections.Generic;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 标签容器，支持增减和查询
    /// </summary>
    [Serializable]
    public class AbilityTagContainer
    {
        private readonly HashSet<AbilityTag> tags = new HashSet<AbilityTag>(AbilityTagComparer.Default);

        /// <summary>
        /// 标签数量
        /// </summary>
        public int Count => tags.Count;

        /// <summary>
        /// 标签变更事件
        /// </summary>
        public event Action<AbilityTag, bool> OnTagChanged;

        /// <summary>
        /// 添加标签
        /// </summary>
        public bool Add(AbilityTag tag)
        {
            if (!tag.IsValid)
            {
                return false;
            }

            if (tags.Add(tag))
            {
                OnTagChanged?.Invoke(tag, true);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 添加多个标签
        /// </summary>
        public void AddRange(IEnumerable<AbilityTag> tags)
        {
            if (tags == null)
            {
                return;
            }

            foreach (AbilityTag tag in tags)
            {
                Add(tag);
            }
        }

        /// <summary>
        /// 移除标签
        /// </summary>
        public bool Remove(AbilityTag tag)
        {
            if (tags.Remove(tag))
            {
                OnTagChanged?.Invoke(tag, false);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 清空标签
        /// </summary>
        public void Clear()
        {
            if (tags.Count == 0)
            {
                return;
            }

            List<AbilityTag> tmp = new List<AbilityTag>(tags);
            tags.Clear();
            foreach (AbilityTag t in tmp)
            {
                OnTagChanged?.Invoke(t, false);
            }
        }

        /// <summary>
        /// 是否包含指定标签
        /// </summary>
        public bool HasTag(AbilityTag tag)
        {
            if (!tag.IsValid)
            {
                return false;
            }

            foreach (AbilityTag t in tags)
            {
                if (t.Matches(tag))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否包含任意一个标签
        /// </summary>
        public bool HasAny(IEnumerable<AbilityTag> checks)
        {
            if (checks == null)
            {
                return false;
            }

            foreach (AbilityTag t in checks)
            {
                if (HasTag(t)) return true;
            }

            return false;
        }

        /// <summary>
        /// 是否包含全部标签
        /// </summary>
        public bool HasAll(IEnumerable<AbilityTag> checks)
        {
            if (checks == null)
            {
                return true;
            }

            foreach (AbilityTag t in checks)
            {
                if (!HasTag(t)) return false;
            }

            return true;
        }

        /// <summary>
        /// 获取所有标签的枚举器
        /// </summary>
        public HashSet<AbilityTag>.Enumerator GetEnumerator()
        {
            return tags.GetEnumerator();
        }
    }
}
