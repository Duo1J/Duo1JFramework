using Duo1JFramework.FSM;
using Duo1JFramework.GamerInput;
using UnityEngine;

//TODO hlj Y轴鼠标

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 可控制角色
    /// </summary>
    public class ControlableActor : CommonActor
    {
        private int jumpFrameCount = 0;
        private const int MinJumpFrame = 30;

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

                        UpdateCamera();
                    },
                    null),
                StateNode.Create("Jump",
                    () =>
                    {
                        if (!Controller.Grounded)
                        {
                            Controller.SwitchState("Move");
                            return;
                        }
                        jumpFrameCount = 0;
                        InputManager.GetCircleMapAxisRaw(out float h, out float v);
                        Controller.Jump(h, v);
                        Controller.AniCrossFade(Param.jumpAniName);
                    },
                    () =>
                    {
                        if (++jumpFrameCount >= MinJumpFrame && Controller.Grounded)
                        {
                            Controller.SwitchState("Move");
                        }

                        UpdateCamera();
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

            if (Controller.Grounded &&
                !Controller.InState("Jump") &&
                InputManager.GetKeyDown(KeyCode.Space))
            {
                Controller.SwitchState("Jump");
            }
        }

        private void UpdateCamera()
        {
            float mx = InputManager.GetAxisMX();
            float my = InputManager.GetAxisMY();
            Controller.RotateCameraPoint(mx, my);
        }
    }
}