using Duo1JFramework.World;
using UnityEngine;

namespace Duo1JFramework.PhysicsAPI.Physics2D
{
    /// <summary>
    /// 2DÅö×²¡¢´¥·¢¿ØÖÆÆ÷
    /// </summary>
    [DisallowMultipleComponent]
    public class CollisionController2D : BaseWorldItem, ICollisionController
    {
        [SerializeField]
        private ECollisionType collisionType = ECollisionType.Trigger;

        public void SetCollisionType(ECollisionType collisionType)
        {
            this.collisionType = collisionType;
        }

        public void SetEnable(bool enable)
        {
            enabled = enable;
        }

        public void DrawEditorInfo()
        {
        }
    }
}
