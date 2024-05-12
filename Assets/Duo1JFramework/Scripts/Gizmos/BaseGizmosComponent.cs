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

        public Color color = Color.white;

        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            DrawGizmos();
        }

#endif
    }
}