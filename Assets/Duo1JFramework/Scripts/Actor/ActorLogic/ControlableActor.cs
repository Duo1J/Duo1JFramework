using Duo1JFramework.FSM;
using Duo1JFramework.GamerInput;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 可控制角色
    /// </summary>
    public class ControlableActor : CommonActor
    {
        /// <summary>
        /// 初始化状态机
        /// </summary>
        protected void InitFSM()
        {
            Controller.InitFSM("Move",
                StateNode.Create("Move",
                    null,
                    () =>
                    {
                        InputManager.GetCircleMapAxisRaw(out float h, out float v);
                        Controller.SetMoveSpeedByAxis(h, v);
                        Controller.RotateByAxis(h, v);

                        if (Controller.CheckAxisZero(h, v))
                            Controller.AniCrossFade(Param.idleAniName);
                        else
                            Controller.AniCrossFade(Param.runAniName);
                    },
                    null),
                StateNode.Create("Jump",
                    () =>
                    {
                        InputManager.GetCircleMapAxisRaw(out float h, out float v);
                        Controller.Jump(h, v);
                        Controller.AniCrossFade(Param.jumpAniName);
                    },
                    () =>
                    {
                    },
                    null).SetSwitchList("Move")
            );
        }

        protected override void OnCreated()
        {
            RegisterUpdate(OnUpdate);
            InitFSM();

            Controller.FallSpeedUp = true;
            Controller.UpdateGrounded = true;
        }

        private void OnUpdate()
        {
            if (Controller == null) return;

            if (InputManager.GetKeyDown(KeyCode.Space))
            {
                Controller.SwitchState("Jump");
            }

            if (Controller.Grounded && Controller.InState("Jump"))
            {
                Controller.SwitchState("Move");
            }
        }
    }
}