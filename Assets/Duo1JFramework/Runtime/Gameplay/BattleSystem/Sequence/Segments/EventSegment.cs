using System;
using Duo1JFramework.Event;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 自定义事件片段
    /// </summary>
    [Serializable]
    public class EventSegment : SequenceSegment
    {
        /// <summary>
        /// 事件Key
        /// </summary>
        public string EventKey;

        /// <summary>
        /// 附带数据
        /// </summary>
        public string Payload;

        public override void OnEnter(SkillContext ctx)
        {
            if (string.IsNullOrEmpty(EventKey))
            {
                return;
            }

            if (EventManager.TryGetInstance(out EventManager eventManager))
            {
                eventManager.Broadcast(EventKey, new SkillEventArg
                {
                    Ctx = ctx,
                    Payload = Payload
                });
            }
        }
    }

    /// <summary>
    /// 技能事件参数
    /// </summary>
    public class SkillEventArg
    {
        public SkillContext Ctx;
        public string Payload;
    }
}
