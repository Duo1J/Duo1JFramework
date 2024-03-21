using UnityEngine;

namespace Duo1JFramework.CameraAPI
{
    /// <summary>
    /// 相机可跟随
    /// </summary>
    public interface ICameraFollow
    {
        Transform CameraFollowPoint { get; }
    }
}