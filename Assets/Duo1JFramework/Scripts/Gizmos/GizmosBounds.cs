using System;
using UnityEngine;

namespace Duo1JFramework
{
    public class GizmosBounds : BaseGizmosComponent
    {
        public Bounds Bounds => new Bounds(bounds.center + transform.position, bounds.size);

        [SerializeField]
        private Bounds bounds;

#if UNITY_EDITOR
        public bool showCorner = false;
        public float cornerSphereSize = 0.5f;
#endif

        public override void DrawGizmos()
        {
            Bounds b = Bounds;
            Gizmos.DrawWireCube(b.center, b.size);

#if UNITY_EDITOR
            if (showCorner)
            {
                Gizmos.DrawWireSphere(b.min, cornerSphereSize);
                Gizmos.DrawWireSphere(b.max, cornerSphereSize);
            }
#endif
        }
    }
}