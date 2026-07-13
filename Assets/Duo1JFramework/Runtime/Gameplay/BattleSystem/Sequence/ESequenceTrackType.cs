namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 序列轨道类型
    /// </summary>
    public enum ESequenceTrackType
    {
        /// <summary>
        /// 动画
        /// </summary>
        Animation = 0,

        /// <summary>
        /// 判定盒
        /// </summary>
        HitBox,

        /// <summary>
        /// 效果应用
        /// </summary>
        EffectApply,

        /// <summary>
        /// 特效
        /// </summary>
        Vfx,

        /// <summary>
        /// 音效
        /// </summary>
        Sfx,

        /// <summary>
        /// 位移
        /// </summary>
        Movement,

        /// <summary>
        /// 相机震动
        /// </summary>
        CameraShake,

        /// <summary>
        /// 自定义事件
        /// </summary>
        Event,
    }
}
