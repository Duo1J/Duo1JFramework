using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 直线样条线
    /// </summary>
    public class LineSpline : BaseMono
    {
        [SerializeField]
        private Vector3[] nodeList;

        /// <summary>
        /// 节点列表
        /// </summary>
        public Vector3[] NodeList => nodeList;
    }
}