using Duo1JFramework.CameraAPI;
using System;
using UnityEngine;

namespace Duo1JFramework.Actor.Actor2D
{
    /// <summary>
    /// 2D角色逻辑基类
    /// </summary>
    [Serializable]
    public abstract class BaseActor2D : BaseRegister,
            ICameraFollow,
            ICameraLookAt
    {
        /// <summary>
        /// 相机跟随挂点
        /// </summary>
        public Transform CameraFollowPoint => null;

        /// <summary>
        /// 相机注视挂点
        /// </summary>
        public Transform CameraLookAtPoint => null;

        protected override void OnDispose()
        {
        }
    }
}
