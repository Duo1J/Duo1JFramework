using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// Gizmos可视化调整组件
    /// </summary>
    public abstract class BaseGizmosComponent : BaseMono, IGizmosDrawer
    {
        public abstract void DrawGizmos();

#if UNITY_EDITOR

        public Color gizmosColor = Color.white;

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmosColor;
            DrawGizmos();
        }

#endif
    }
}