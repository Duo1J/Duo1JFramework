using Duo1JFramework.GamerInput;
using UnityEngine;

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

            Controller.SetFallSpeedUp(true);
        }

        private void OnUpdate()
        {
            if (Controller == null) return;

            float h = InputManager.GetAxisH(true);
            float v = InputManager.GetAxisV(true);
            Controller.CircleMapping(ref h, ref v);

            Controller.SetMoveSpeedByAxis(h, v);
            Controller.RotateByAxis(h, v);

            if (InputManager.GetKeyDown(KeyCode.Space))
            {
                Controller.Jump(h, v);
            }

            UpdateAni(h, v);
        }

        private void UpdateAni(float h, float v)
        {
            if (Controller.CheckAxisZero(h, v))
            {
                Controller.AniCrossFade(Param.idleAniName);
            }
            else
            {
                Controller.AniCrossFade(Param.runAniName);
            }
        }
    }
}