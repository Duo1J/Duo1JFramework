using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI网格细分
    /// </summary>
    public class UISubdivide : BaseUIEffect
    {
        [Label("细分深度")]
        [Range(0, 5)]
        public int depth = 2;

        public override void ModifyMesh(VertexHelper vh)
        {
            List<UIVertex> oldList = new List<UIVertex>();
            List<UIVertex> newList = new List<UIVertex>();
            vh.GetUIVertexStream(oldList);

            List<UIVertex> outList = new List<UIVertex>();
            for (int i = 0; i < oldList.Count; i += 3)
            {
                outList.Clear();
                UIUtil.SubdivideTriangleDepth(oldList[i], oldList[i + 1], oldList[i + 2], depth, outList);
                newList.AddRange(outList);
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(newList);
        }
    }
}
