using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// CharactorController Actor控制器
    /// </summary>
    public class CcActorController : ActorController
    {
        /// <summary>
        /// 角色控制器
        /// </summary>
        [SerializeField]
        private CharacterController cc;

        protected override void OnCollectComponent()
        {
            if (cc == null)
            {
                cc = gameObject.GetAndAssertComponent<CharacterController>();
            }
        }

        protected override void OnInitComponent()
        {
        }

        protected override void UpdateFallSpeedUp()
        {
        }
    }
}