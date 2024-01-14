using Duo1JFramework.FSM;
using Duo1JFramework.GamerInput;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 可控制CharacterController角色
    /// </summary>
    public class CcControlableActor : GenericActor<CcActorController>
    {
        private int jumpFrameCount = 0;
        private const int MinJumpFrameCount = 30;

        /// <summary>
        /// 初始化状态机
        /// </summary>
        protected void InitFSM()
        {
            Con.InitFSM("Move",
                StateNode.Create("Move",
                    null,
                    () =>
                    {
                        InputManager.GetCircleMapAxisRaw(out float h, out float v);
                        Con.MoveByAxis(h, v);
                        Con.RotateByAxis(h, v);

                        if (Con.CheckAxisZero(h, v))
                            Con.AniCrossFade(Param.idleAniName);
                        else
                            Con.AniCrossFade(Param.runAniName);

                        UpdateCamera();
                    },
                    null),
                StateNode.Create("Jump",
                    () =>
                    {
                        if (!Con.Grounded)
                        {
                            Con.SwitchState("Move");
                            return;
                        }
                        jumpFrameCount = 0;
                        Con.JumpByHeight();
                        Con.AniCrossFade(Param.jumpAniName);
                    },
                    () =>
                    {
                        if (++jumpFrameCount >= MinJumpFrameCount && Con.Grounded)
                        {
                            Con.SwitchState("Move");
                        }

                        UpdateCamera();
                    },
                    null).SetSwitchList("Move")
            );
        }

        protected override void OnCreated()
        {
            Con.FallSpeedUp = true;
            Con.UpdateGrounded = true;

            RegisterUpdate(OnUpdate);
            InitFSM();
        }

        private void OnUpdate()
        {
            if (Con == null) return;

            if (Con.Grounded &&
                !Con.InState("Jump") &&
                InputManager.GetKeyDown(KeyCode.Space))
            {
                Con.SwitchState("Jump");
            }
        }
    }
}