using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色逻辑基类 - Transform
    /// </summary>
    public abstract partial class BaseActor
    {
        /// <summary>
        /// 坐标
        /// </summary>
        public Vector3 Pos => AssetTf == null ? Vector3.zero : AssetTf.position;

        /// <summary>
        /// 本地坐标
        /// </summary>
        public Vector3 LocPos => AssetTf == null ? Vector3.zero : AssetTf.localPosition;

        /// <summary>
        /// 旋转四元数
        /// </summary>
        public Quaternion Rot => AssetTf == null ? Quaternion.identity : AssetTf.rotation;

        /// <summary>
        /// 本地旋转四元数
        /// </summary>
        public Quaternion LocRot => AssetTf == null ? Quaternion.identity : AssetTf.localRotation;

        /// <summary>
        /// 旋转欧拉角
        /// </summary>
        public Vector3 Angle => AssetTf == null ? Vector3.zero : AssetTf.eulerAngles;

        /// <summary>
        /// 本地旋转欧拉角
        /// </summary>
        public Vector3 LocAngle => AssetTf == null ? Vector3.zero : AssetTf.localEulerAngles;

        /// <summary>
        /// 缩放
        /// </summary>
        public Vector3 Scale => AssetTf == null ? Vector3.one : AssetTf.lossyScale;

        /// <summary>
        /// 本地缩放
        /// </summary>
        public Vector3 LocScale => AssetTf == null ? Vector3.one : AssetTf.localScale;
    }
}
