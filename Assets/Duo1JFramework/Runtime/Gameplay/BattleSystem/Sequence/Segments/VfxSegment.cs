using System;
using Duo1JFramework.Asset;
using UnityEngine;

namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 特效片段
    /// </summary>
    [Serializable]
    public class VfxSegment : SequenceSegment
    {
        /// <summary>
        /// 特效资源路径
        /// </summary>
        public string VfxPath;

        /// <summary>
        /// 相对Owner的偏移
        /// </summary>
        public Vector3 Offset;

        /// <summary>
        /// 相对Owner的旋转
        /// </summary>
        public Vector3 EulerAngles;

        /// <summary>
        /// 是否跟随Owner
        /// </summary>
        public bool AttachToOwner = true;

        /// <summary>
        /// 缩放
        /// </summary>
        public float Scale = 1f;

        [NonSerialized]
        private GameObject spawnedGo;

        [NonSerialized]
        private IAssetHandle<GameObject> handle;

        public override void OnEnter(SkillContext ctx)
        {
            if (ctx == null || ctx.Owner == null || string.IsNullOrEmpty(VfxPath))
            {
                return;
            }

            handle = AssetManager.Instance.LoadResourceSync<GameObject>(VfxPath);
            if (handle == null)
            {
                return;
            }

            spawnedGo = handle.Instantiate();
            if (spawnedGo == null)
            {
                handle.Release();
                handle = null;
                return;
            }

            Transform tf = spawnedGo.transform;
            Transform ownerTf = ctx.Owner.transform;
            if (AttachToOwner)
            {
                tf.SetParent(ownerTf, false);
                tf.localPosition = Offset;
                tf.localRotation = Quaternion.Euler(EulerAngles);
                tf.localScale = Vector3.one * Scale;
            }
            else
            {
                tf.position = ownerTf.TransformPoint(Offset);
                tf.rotation = ownerTf.rotation * Quaternion.Euler(EulerAngles);
                tf.localScale = Vector3.one * Scale;
            }
        }

        public override void OnExit(SkillContext ctx)
        {
            if (spawnedGo != null)
            {
                spawnedGo.DestroySmart();
                spawnedGo = null;
            }
            if (handle != null)
            {
                handle.Release();
                handle = null;
            }
        }
    }
}
