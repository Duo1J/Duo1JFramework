using System;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 相机震动片段
    /// </summary>
    [Serializable]
    public class CameraShakeSegment : SequenceSegment
    {
        /// <summary>
        /// 幅度
        /// </summary>
        public float Amplitude = 0.15f;

        /// <summary>
        /// 频率
        /// </summary>
        public float Frequency = 30f;

        [NonSerialized]
        private Transform camTf;

        [NonSerialized]
        private Vector3 originLocalPos;

        [NonSerialized]
        private bool bound;

        public override void OnEnter(SkillContext ctx)
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                return;
            }

            camTf = cam.transform;
            originLocalPos = camTf.localPosition;
            bound = true;
        }

        public override void OnUpdate(SkillContext ctx, float localTime)
        {
            if (!bound || camTf == null)
            {
                return;
            }

            float t = localTime * Frequency;
            Vector3 offset = new Vector3(
                Mathf.Sin(t) * Amplitude,
                Mathf.Cos(t * 1.3f) * Amplitude * 0.5f,
                0f
            );
            camTf.localPosition = originLocalPos + offset;
        }

        public override void OnExit(SkillContext ctx)
        {
            if (bound && camTf != null)
            {
                camTf.localPosition = originLocalPos;
            }
            bound = false;
            camTf = null;
        }
    }
}
