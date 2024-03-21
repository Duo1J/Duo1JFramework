using UnityEngine;

namespace Duo1JFramework.CameraAPI
{
    /// <summary>
    /// 相机可注视
    /// </summary>
    public interface ICameraLookAt
    {
        Transform CameraLookAtPoint { get; }
    }
}