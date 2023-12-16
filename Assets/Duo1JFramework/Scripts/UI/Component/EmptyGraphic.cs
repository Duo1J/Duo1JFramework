using UnityEngine.UI;

namespace Duo1JFramework
{
    /// <summary>
    /// 空图像
    /// </summary>
    public class EmptyGraphic : Image
    {
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}