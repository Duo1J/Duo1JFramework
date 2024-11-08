using System;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 贝塞尔样条线
    /// </summary>
    public class BezierSpline : BaseMono
    {
        [SerializeField]
        private BezierSplineNode[] nodeList;

        /// <summary>
        /// 节点列表
        /// </summary>
        public BezierSplineNode[] NodeList => nodeList;
    }

    /// <summary>
    /// 贝塞尔样条线节点
    /// </summary>
    [Serializable]
    public class BezierSplineNode
    {
        public Vector3 point;
        public Vector3 control;
    }
}