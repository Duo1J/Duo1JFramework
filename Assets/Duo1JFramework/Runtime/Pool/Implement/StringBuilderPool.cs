using System.Text;

namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// StringBuilder对象池实例
    /// </summary>
    public class StringBuilderPool : BaseObjectPool<StringBuilder>
    {
        public override void OnPopObject(StringBuilder o)
        {
            o.Clear();
        }
    }
}
