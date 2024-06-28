using Duo1JFramework.PhysicsAPI;
using UnityEngine;

namespace Duo1JFramework
{
    public class CollisionMonitor : BaseEditorWindow<CollisionMonitor>
    {
        private Vector2 scrollPos;

        private void OnGUI()
        {
            if (!LU.IsPlayingHelpBox())
            {
                return;
            }

            LU.Scroll(ref scrollPos, () =>
            {
                CollisionManager.Instance.DrawEditorInfo();
            });
        }
    }
}