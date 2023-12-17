using System.Text;

namespace Duo1JFramework.ObjectPool
{
    public class StringBuilderPool : BaseObjectPool<StringBuilder>
    {
        public override StringBuilder InitObject(StringBuilder o)
        {
            o.Clear();
            return o;
        }
    }
}