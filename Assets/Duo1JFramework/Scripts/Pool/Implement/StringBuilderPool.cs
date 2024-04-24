using System.Text;

namespace Duo1JFramework.ObjectPool
{
    public class StringBuilderPool : BaseObjectPool<StringBuilder>
    {
        public override void OnPopObject(StringBuilder o)
        {
            o.Clear();
        }
    }
}