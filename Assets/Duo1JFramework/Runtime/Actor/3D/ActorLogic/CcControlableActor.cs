using Duo1JFramework.FSM;
using Duo1JFramework.GamerInput;
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

        protected Vector3 moveVelocity;
        protected Vector3 jumpVelocity;

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
                        InputManager.GetCircleMapAxisRaw(out float h, out float v);
                        Vector3 moveDir = Con.GetMoveDirByAxis(h, v);
                        moveVelocity = moveDir * (InWalk() ? Param.walkSpeed : Param.moveSpeed);
                        Con.RotateByAxis(h, v);

                        if (Con.CheckAxisZero(h, v))
                        {
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
                        moveVelocity = Vector3.zero;
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
                        InputManager.GetCircleMapAxisRaw(out float h, out float v);
                        Vector3 moveDir = Con.GetMoveDirByAxis(h, v);
                        jumpVelocity = moveDir * (InWalk() ? Param.walkSpeed : Param.moveSpeed);
                        jumpVelocity.y = Con.GetJumpVeloByHeight();
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
                        jumpVelocity = Vector3.zero;
                        Con.CameraOffsetY = 0f;
                    }).SetSwitchList("Move")
            );
        }

        protected virtual Vector3 OnUpdateVelocity(Vector3 input)
        {
            input += moveVelocity;
            input += jumpVelocity;

            return input;
        }

        protected override void OnCreated()
        {
            Con.FallSpeedUp = true;
            Con.UpdateGrounded = true;

            RegisterUpdate(OnUpdate);
            InitFSM();
            Con.RegisterOnUpdateVelocity(OnUpdateVelocity);
        }

        public virtual bool InWalk()
        {
            return InputManager.GetKey(KeyCode.LeftControl);
        }

        protected virtual void OnUpdate()
        {
            if (Con == null) return;

            UpdateCamera();

            if (Con.Grounded &&
                !Con.InState("Jump") &&
                InputManager.GetKeyDown(KeyCode.Space))
            {
                Con.SwitchState("Jump");
            }
        }
    }
}