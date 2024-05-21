using Duo1JFramework.FSM;
using Duo1JFramework.GamerInput;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 可控制Rigidbody角色逻辑
    /// </summary>
    public class RbControlableActor : GenericActor<RbActorController>
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
                        InputManager.GetCircleMapAxisRaw(out float h, out float v);
                        Con.SetMoveSpeedByAxis(h, v, InWalk() ? Param.walkSpeed : Param.moveSpeed);
                        Con.RotateByAxis(h, v);

                        if (Con.CheckAxisZero(h, v))
                        {
                            Con.AniCrossFade(Param.idleAniName);
                            Con.CameraOffsetZ = 0;
                        }
                        else
                        {
                            Con.AniCrossFade(InWalk() ? Param.walkAniName : Param.runAniName);
                            Con.CameraOffsetZ = InWalk() ? -0.3f : -0.7f;
                        }
                    },
                    null),
                StateNode.Create("Jump",
                    (param) =>
                    {
                        if (!Con.Grounded)
                        {
                            Con.SwitchState("Move");
                            return;
                        }
                        jumpFrameStartCnt = Time.frameCount;
                        Con.JumpByHeight(Param.jumpHeight);
                        Con.AniCrossFade(Param.jumpAniName);
                    },
                    () =>
                    {
                        if ((Time.frameCount - jumpFrameStartCnt) >= minJumpFrameCnt && Con.Grounded)
                        {
                            Con.SwitchState("Move");
                        }
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