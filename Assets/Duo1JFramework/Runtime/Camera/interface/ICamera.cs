namespace Duo1JFramework.CameraAPI
{
    /// <summary>
    /// 相机逻辑接口
    /// </summary>
    public interface ICamera
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void InitCamera(params object[] param);

        /// <summary>
        /// 销毁
        /// </summary>
        void DestroyCamera();

        /// <summary>
        /// 跟随
        /// </summary>
        void Follow(ICameraFollow t);

        /// <summary>
        /// 注视
        /// </summary>
        void LookAt(ICameraLookAt t);
    }
}
