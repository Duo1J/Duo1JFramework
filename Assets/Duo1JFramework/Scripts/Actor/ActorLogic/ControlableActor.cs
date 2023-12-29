using Duo1JFramework.GamerInput;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 可控制角色
    /// </summary>
    public class ControlableActor : CommonActor
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
    }
}