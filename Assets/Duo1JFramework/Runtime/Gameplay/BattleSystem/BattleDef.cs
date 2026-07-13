namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 战斗系统定义
    /// </summary>
    public static class BattleDef
    {
        /// <summary>
        /// 外部直接调用的伤害来源Id, 用于DamageInfo.SourceId
        /// </summary>
        public const string ExternalDamageSourceId = "external";

        /// <summary>
        /// 效果配置库菜单名
        /// </summary>
        public const string EffectConfigLibMenuName = Def.FRAME_WORK_NAME + "/BattleSystem/EffectConfigLib";

        /// <summary>
        /// 技能配置菜单名
        /// </summary>
        public const string SkillConfigMenuName = Def.FRAME_WORK_NAME + "/BattleSystem/SkillConfig";

        /// <summary>
        /// 技能配置库菜单名
        /// </summary>
        public const string SkillConfigLibMenuName = Def.FRAME_WORK_NAME + "/BattleSystem/SkillConfigLib";
    }
}
