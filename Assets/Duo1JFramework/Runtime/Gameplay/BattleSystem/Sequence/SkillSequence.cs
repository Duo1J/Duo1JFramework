using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 技能序列, 由多个轨道和片段组成的时间轴
    /// </summary>
    [Serializable]
    public class SkillSequence
    {
        /// <summary>
        /// 总时长(s)
        /// </summary>
        [SerializeField]
        public float Duration = 1f;

        /// <summary>
        /// 是否循环
        /// </summary>
        [SerializeField]
        public bool Loop;

        /// <summary>
        /// 轨道列表
        /// </summary>
        [SerializeField]
        public List<SequenceTrack> Tracks = new List<SequenceTrack>();

        /// <summary>
        /// 已完成
        /// </summary>
        [NonSerialized]
        public bool Finished;

        /// <summary>
        /// 当前时间
        /// </summary>
        [NonSerialized]
        public float CurTime;

        /// <summary>
        /// 上次时间
        /// </summary>
        [NonSerialized]
        public float PrevTime;

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            Finished = false;
            CurTime = 0f;
            PrevTime = -1f;
            for (int i = 0; i < Tracks.Count; i++)
            {
                Tracks[i].Reset();
            }
        }

        /// <summary>
        /// 立即跳转到指定时间, 用于预览
        /// </summary>
        public void Sample(SkillContext ctx, float time)
        {
            PrevTime = CurTime;
            CurTime = Mathf.Clamp(time, 0f, Duration);
            for (int i = 0; i < Tracks.Count; i++)
            {
                Tracks[i].Tick(ctx, PrevTime, CurTime);
            }
        }

        /// <summary>
        /// 推进
        /// </summary>
        public void Tick(SkillContext ctx)
        {
            if (Finished)
            {
                return;
            }

            PrevTime = CurTime;
            CurTime += Time.deltaTime;

            if (CurTime >= Duration)
            {
                CurTime = Duration;
                for (int i = 0; i < Tracks.Count; i++)
                {
                    Tracks[i].Tick(ctx, PrevTime, CurTime);
                }

                if (Loop)
                {
                    for (int i = 0; i < Tracks.Count; i++)
                    {
                        Tracks[i].ForceExit(ctx);
                        Tracks[i].Reset();
                    }
                    PrevTime = -1f;
                    CurTime = 0f;
                }
                else
                {
                    Finished = true;
                }
                return;
            }

            for (int i = 0; i < Tracks.Count; i++)
            {
                Tracks[i].Tick(ctx, PrevTime, CurTime);
            }
        }

        /// <summary>
        /// 强制打断, 会退出所有进入中的片段
        /// </summary>
        public void Interrupt(SkillContext ctx)
        {
            for (int i = 0; i < Tracks.Count; i++)
            {
                Tracks[i].ForceExit(ctx);
            }
            Finished = true;
        }
    }
}
