using UnityEngine;

namespace Duo1JFramework.Camera3D
{
    /// <summary>
    /// 相机可注视
    /// </summary>
    public interface ICameraLookAt
    {
        Transform CameraLookAtPoint { get; }
    }
}