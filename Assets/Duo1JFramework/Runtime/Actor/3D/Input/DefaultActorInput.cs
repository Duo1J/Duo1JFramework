using Duo1JFramework.GamerInput;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 默认角色输入源
    /// </summary>
    public class DefaultActorInput : IActorInput
    {
        public Vector2 Move
        {
            get
            {
                InputManager.GetCircleMapAxisRaw(out float h, out float v);
                return new Vector2(h, v);
            }
        }

        public Vector2 Look => new Vector2(InputManager.GetAxisMX(), InputManager.GetAxisMY());

        public bool Walk => InputManager.GetKey(KeyCode.LeftControl);

        public bool JumpDown => InputManager.GetKeyDown(KeyCode.Space);
    }
}
