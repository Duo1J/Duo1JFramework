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

        private Vector3 moveVelocity;
        private Vector3 jumpVelocity;

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
                        Vector3 moveDir = Con.GetMoveDirByAxis(h, v);
                        if (InWalk())
                        {
                            moveVelocity = moveDir * Param.walkSpeed;
                        }
                        else
                        {
                            moveVelocity = moveDir * Param.moveSpeed;
                        }
                        Con.RotateByAxis(h, v);

                        if (Con.CheckAxisZero(h, v))
                            Con.AniCrossFade(Param.idleAniName);
                        else
                        {
                            if (InWalk())
                            {
                                Con.AniCrossFade(Param.walkAniName);
                            }
                            else
                            {

                                Con.AniCrossFade(Param.runAniName);
                            }
                        }

                        UpdateCamera();
                    },
                    () =>
                    {
                        moveVelocity = Vector3.zero;
                    }),
                StateNode.Create("Jump",
                    () =>
                    {
                        if (!Con.Grounded)
                        {
                            Con.SwitchState("Move");
                            return;
                        }
                        jumpFrameCount = 0;
                        InputManager.GetCircleMapAxisRaw(out float h, out float v);
                        Vector3 moveDir = Con.GetMoveDirByAxis(h, v);
                        if (InWalk())
                        {
                            jumpVelocity = moveDir * Param.walkSpeed;
                        }
                        else
                        {
                            jumpVelocity = moveDir * Param.moveSpeed;
                        }
                        jumpVelocity.y = Con.GetJumpVeloByHeight();
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
                    () =>
                    {
                        jumpVelocity = Vector3.zero;
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

        public bool InWalk()
        {
            return InputManager.GetKey(KeyCode.LeftControl);
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