using System;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 位移片段, 使Owner在片段时间内产生位移, 未使用ActorController
    /// </summary>
    [Serializable]
    public class MovementSegment : SequenceSegment
    {
        /// <summary>
        /// 相对Owner的方向 (x左右 y上下 z前后)
        /// </summary>
        public Vector3 LocalDir = new Vector3(0, 0, 1);

        /// <summary>
        /// 总距离
        /// </summary>
        public float Distance = 1f;

        /// <summary>
        /// 是否使用CharacterController.Move
        /// </summary>
        public bool UseCharacterController = true;

        [NonSerialized]
        private CharacterController cc;

        [NonSerialized]
        private Vector3 lastPos;

        public override void OnEnter(SkillContext ctx)
        {
            if (ctx == null || ctx.Owner == null)
            {
                return;
            }

            cc = ctx.Owner.GetComponent<CharacterController>();
            lastPos = ctx.Owner.transform.position;
        }

        public override void OnUpdate(SkillContext ctx, float localTime)
        {
            if (ctx == null || ctx.Owner == null || Duration <= 0f)
            {
                return;
            }

            Transform tf = ctx.Owner.transform;
            Vector3 worldDir = tf.TransformDirection(LocalDir.normalized);
            float delta = Distance * (Time.deltaTime / Duration);
            Vector3 offset = worldDir * delta;

            if (UseCharacterController && cc != null && cc.enabled)
            {
                cc.Move(offset);
            }
            else
            {
                tf.position += offset;
            }
        }
    }
}
