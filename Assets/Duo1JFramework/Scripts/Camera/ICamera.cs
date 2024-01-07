using UnityEngine;

namespace Duo1JFramework.Camera3D
{
    public interface ICamera
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void InitCamera(params object[] param);

        /// <summary>
        /// 跟随
        /// </summary>
        void Follow(Transform t);

        /// <summary>
        /// 注视
        /// </summary>
        void LookAt(Transform t);
    }
}