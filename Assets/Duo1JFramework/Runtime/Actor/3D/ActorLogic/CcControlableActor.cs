using Duo1JFramework.FSM;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 可控制CharacterController角色逻辑
    /// </summary>
    public class CcControlableActor : GenericActor<CcActorController>
    {
        protected int jumpFrameStartCnt = 0;
        protected int minJumpFrameCnt = 30;

        /// <summary>
        /// 初始化状态机
        /// </summary>
        protected virtual void InitFSM()
        {
            Con.InitFSM("Move",
                StateNode.Create("Move",
                    null,
                    () =>
                    {
                        Vector2 move = InputSource.Move;
                        Vector3 moveDir = Con.GetMoveDirByAxis(move.x, move.y);
                        float speed = InWalk() ? Param.walkSpeed : Param.moveSpeed;
                        Con.Move(moveDir, speed);
                        Con.RotateByAxis(move.x, move.y);

                        if (Con.CheckAxisZero(move.x, move.y))
                        {
                            Con.Stop();
                            Con.AnimCrossFade(Param.idleAniName);
                            Con.CameraOffsetZ = 0;
                            Con.SetFootIKGoal(1, 1, false);
                        }
                        else
                        {
                            Con.AnimCrossFade(InWalk() ? Param.walkAniName : Param.runAniName);
                            Con.CameraOffsetZ = InWalk() ? -0.3f : -0.7f;
                            Con.SetFootIKGoal(0, 0, true);
                        }
                    },
                    (param) =>
                    {
                        Con.Stop();
                        Con.SetFootIKGoal(0, 0, true);
                    }),
                StateNode.Create("Jump",
                    (param) =>
                    {
                        if (!Con.Grounded)
                        {
                            Con.SwitchState("Move");
                            return;
                        }
                        jumpFrameStartCnt = Time.frameCount;
                        Vector2 move = InputSource.Move;
                        Vector3 moveDir = Con.GetMoveDirByAxis(move.x, move.y);
                        Con.Move(moveDir, InWalk() ? Param.walkSpeed : Param.moveSpeed);
                        Con.Jump(Param.jumpHeight);
                        Con.AnimCrossFade(Param.jumpAniName);
                    },
                    () =>
                    {
                        if ((Time.frameCount - jumpFrameStartCnt) >= minJumpFrameCnt && Con.Grounded)
                        {
                            Con.SwitchState("Move");
                        }
                    },
                    (param) =>
                    {
                        Con.Stop();
                        Con.CameraOffsetY = 0f;
                    }).SetSwitchList("Move")
            );
        }

        protected override void OnCreated()
        {
            Con.FallSpeedUp = true;
            Con.UpdateGrounded = true;

            RegisterUpdate(OnUpdate);
            InitFSM();
        }

        public virtual bool InWalk()
        {
            return InputSource.Walk;
        }

        protected virtual void OnUpdate()
        {
            if (Con == null) return;

            UpdateCamera();

            if (Con.Grounded &&
                !Con.InState("Jump") &&
                InputSource.JumpDown)
            {
                Con.SwitchState("Jump");
            }
        }
    }
}
