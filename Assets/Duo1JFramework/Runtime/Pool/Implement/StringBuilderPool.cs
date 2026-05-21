using System.Text;

namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// StringBuilder对象池实例
    /// </summary>
    public class StringBuilderPool : BaseObjectPool<StringBuilder>
    {
        /// <summary>
        /// 最大保留容量
        /// </summary>
        public int MaxRetainedCapacity { get; set; } = 4096;

        public override void OnPushObject(StringBuilder o)
        {
            o.Clear();

            if (o.Capacity > MaxRetainedCapacity)
            {
                o.Capacity = MaxRetainedCapacity;
            }
        }

        public override void OnPopObject(StringBuilder o)
        {
            o.Clear();
        }
    }
}
