using UnityEngine;

namespace Duo1JFramework
{
    public class GizmosSphere : BaseGizmosComponent
    {
        public Vector3 Center
        {
            get => center + transform.position;
            set => center = value;
        }

        public float Radius
        {
            get => radius;
            set => radius = value;
        }

        [SerializeField]
        private Vector3 center;

        [SerializeField]
        private float radius = 1;

        public override void DrawGizmos()
        {
            Gizmos.DrawWireSphere(Center, Radius);
        }
    }
}
