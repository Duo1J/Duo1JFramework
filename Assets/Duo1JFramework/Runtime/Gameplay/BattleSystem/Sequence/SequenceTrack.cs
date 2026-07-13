using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 序列轨道
    /// </summary>
    [Serializable]
    public class SequenceTrack
    {
        /// <summary>
        /// 轨道名
        /// </summary>
        [SerializeField]
        public string Name;

        /// <summary>
        /// 轨道类型
        /// </summary>
        [SerializeField]
        public ESequenceTrackType Type;

        /// <summary>
        /// 是否禁用
        /// </summary>
        [SerializeField]
        public bool Disabled;

        /// <summary>
        /// 片段列表
        /// </summary>
        [SerializeReference]
        public List<SequenceSegment> Segments = new List<SequenceSegment>();

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < Segments.Count; i++)
            {
                Segments[i].Reset();
            }
        }

        /// <summary>
        /// 按当前时间推进
        /// </summary>
        public void Tick(SkillContext ctx, float prevTime, float curTime)
        {
            if (Disabled)
            {
                return;
            }

            for (int i = 0; i < Segments.Count; i++)
            {
                SequenceSegment seg = Segments[i];
                bool inRange = seg.IsInstant
                    ? (prevTime < seg.StartTime && curTime >= seg.StartTime)
                    : (curTime >= seg.StartTime && curTime < seg.EndTime);

                bool wasIn = seg.Entered;
                if (!wasIn && inRange)
                {
                    seg.Entered = true;
                    SafeInvoke(() => seg.OnEnter(ctx), $"Segment.OnEnter {seg.Name}");
                    if (seg.IsInstant)
                    {
                        SafeInvoke(() => seg.OnExit(ctx), $"Segment.OnExit {seg.Name}");
                        seg.Entered = false;
                    }
                }
                else if (wasIn && inRange)
                {
                    SafeInvoke(() => seg.OnUpdate(ctx, curTime - seg.StartTime), $"Segment.OnUpdate {seg.Name}");
                }
                else if (wasIn && !inRange)
                {
                    seg.Entered = false;
                    SafeInvoke(() => seg.OnExit(ctx), $"Segment.OnExit {seg.Name}");
                }
            }
        }

        /// <summary>
        /// 强制结束所有进入中的片段
        /// </summary>
        public void ForceExit(SkillContext ctx)
        {
            for (int i = 0; i < Segments.Count; i++)
            {
                SequenceSegment seg = Segments[i];
                if (seg.Entered)
                {
                    seg.Entered = false;
                    SafeInvoke(() => seg.OnExit(ctx), $"Segment.OnExit(Force) {seg.Name}");
                }
            }
        }

        private static void SafeInvoke(Action a, string tag)
        {
            try
            {
                a?.Invoke();
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, $"[SequenceTrack] {tag} 异常");
            }
        }
    }
}
