using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 判定盒形状
    /// </summary>
    public enum EHitBoxShape
    {
        /// <summary>
        /// 球体
        /// </summary>
        Sphere,

        /// <summary>
        /// 盒体
        /// </summary>
        Box,

        /// <summary>
        /// 扇形 (以Owner朝向)
        /// </summary>
        Sector,
    }

    /// <summary>
    /// 判定盒片段, 检测目标并广播命中
    /// </summary>
    [Serializable]
    public class HitBoxSegment : SequenceSegment
    {
        /// <summary>
        /// 形状
        /// </summary>
        public EHitBoxShape Shape = EHitBoxShape.Sphere;

        /// <summary>
        /// 相对Owner的位置偏移 (X, Y, Z)
        /// </summary>
        public Vector3 Offset = new Vector3(0, 1, 1);

        /// <summary>
        /// 尺寸, Sphere使用x作为半径, Box使用全部, Sector使用x作为半径 y作为角度
        /// </summary>
        public Vector3 Size = new Vector3(1, 1, 1);

        /// <summary>
        /// 检测的Layer掩码
        /// </summary>
        public LayerMask HitLayer = ~0;

        /// <summary>
        /// 检测间隔 (0表示每帧)
        /// </summary>
        public float Interval = 0f;

        /// <summary>
        /// 每目标最多命中次数, <=0为不限
        /// </summary>
        public int MaxHitPerTarget = 1;

        /// <summary>
        /// 命中后应用的Effect配置Id列表
        /// </summary>
        public List<string> ApplyEffectIds = new List<string>();

        [NonSerialized]
        private float lastCheckTime;

        [NonSerialized]
        private Dictionary<CombatUnitController, int> hitCountMap;

        public override void Reset()
        {
            base.Reset();
            lastCheckTime = -999f;
            hitCountMap = null;
        }

        public override void OnEnter(SkillContext ctx)
        {
            lastCheckTime = -999f;
            hitCountMap = new Dictionary<CombatUnitController, int>();
        }

        public override void OnUpdate(SkillContext ctx, float localTime)
        {
            if (Interval > 0f && (localTime - lastCheckTime) < Interval)
            {
                return;
            }
            lastCheckTime = localTime;
            DoCheck(ctx);
        }

        private void DoCheck(SkillContext ctx)
        {
            if (ctx == null || ctx.Owner == null)
            {
                return;
            }

            Transform ownerTf = ctx.Owner.transform;
            Vector3 worldPos = ownerTf.TransformPoint(Offset);

            Collider[] hits = null;
            switch (Shape)
            {
                case EHitBoxShape.Sphere:
                    hits = Physics.OverlapSphere(worldPos, Size.x, HitLayer, QueryTriggerInteraction.Ignore);
                    break;
                case EHitBoxShape.Box:
                    hits = Physics.OverlapBox(worldPos, Size * 0.5f, ownerTf.rotation, HitLayer, QueryTriggerInteraction.Ignore);
                    break;
                case EHitBoxShape.Sector:
                    hits = Physics.OverlapSphere(worldPos, Size.x, HitLayer, QueryTriggerInteraction.Ignore);
                    break;
            }

            if (hits == null || hits.Length == 0)
            {
                return;
            }

            for (int i = 0; i < hits.Length; i++)
            {
                CombatUnitController target = hits[i].GetComponentInParent<CombatUnitController>();
                if (target == null || target == ctx.Owner || !target.IsAlive)
                {
                    continue;
                }

                if (Shape == EHitBoxShape.Sector)
                {
                    Vector3 toTarget = target.transform.position - ownerTf.position;
                    toTarget.y = 0;
                    float angle = Vector3.Angle(ownerTf.forward, toTarget);
                    if (angle > Size.y * 0.5f)
                    {
                        continue;
                    }
                }

                if (MaxHitPerTarget > 0)
                {
                    hitCountMap.TryGetValue(target, out int cnt);
                    if (cnt >= MaxHitPerTarget) continue;
                    hitCountMap[target] = cnt + 1;
                }

                ApplyHit(ctx, target);
            }
        }

        private void ApplyHit(SkillContext ctx, CombatUnitController target)
        {
            if (ApplyEffectIds == null || ApplyEffectIds.Count == 0)
            {
                return;
            }

            for (int i = 0; i < ApplyEffectIds.Count; i++)
            {
                EffectConfig effectConfig = ctx.GetEffectConfig(ApplyEffectIds[i]);
                if (effectConfig == null)
                {
                    Log.Warn($"[HitBoxSegment] 未找到Effect配置: {ApplyEffectIds[i]}");
                    continue;
                }

                target.Effects.Apply(effectConfig, ctx.Owner);
            }
        }
    }
}
