using System.Text;

namespace Duo1JFramework.ObjectPool
{
    /// <summary>
    /// StringBuilder¶ÔÏó³ØÊµÀý
    /// </summary>
    public class StringBuilderPool : BaseObjectPool<StringBuilder>
    {
        public override void OnPopObject(StringBuilder o)
        {
            o.Clear();
        }
    }
}