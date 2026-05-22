using UnityEngine;

namespace Duo1JFramework.PhysicsAPI
{
    /// <summary>
    /// 碰撞监视器
    /// </summary>
    public class CollisionMonitor : BaseEditorWindow<CollisionMonitor>
    {
        private Vector2 scrollPos;

        private void OnGUI()
        {
            if (!ED.IsPlayingHelpBox())
            {
                return;
            }

            ED.Scroll(ref scrollPos, () =>
            {
                CollisionManager.Instance.DrawEditorInfo();
            });
        }
    }
}
