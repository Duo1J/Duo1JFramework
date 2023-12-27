using Duo1JFramework.GamerInput;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 通用角色
    /// </summary>
    public class CommonActor : BaseActor
    {
        protected override void OnCreated()
        {
            RegisterUpdate(OnUpdate);
        }

        private void OnUpdate()
        {
            if (Controller == null) return;

            float h = InputManager.HAxis(true);
            float v = InputManager.VAxis(true);
            Controller.MoveByLocalAxis(h, v, 3);
        }

        public override void OnDispose()
        {
        }
    }
}