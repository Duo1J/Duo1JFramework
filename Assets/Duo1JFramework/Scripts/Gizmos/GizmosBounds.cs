using UnityEngine;

namespace Duo1JFramework
{
    public class GizmosBounds : BaseGizmosComponent
    {
        public Bounds bounds;

        public override void DrawGizmos()
        {
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}