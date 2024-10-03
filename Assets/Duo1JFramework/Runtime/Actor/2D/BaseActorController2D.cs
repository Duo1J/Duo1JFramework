using Duo1JFramework.World;
using UnityEngine;

namespace Duo1JFramework.Actor.Actor2D
{
    /// <summary>
    /// 2D角色控制器
    /// </summary>
    [RequireComponent(typeof(ActorParam2D), typeof(ActorPoint2D))]
    public abstract class BaseActorController2D : WorldItem2D
    {
    }
}
