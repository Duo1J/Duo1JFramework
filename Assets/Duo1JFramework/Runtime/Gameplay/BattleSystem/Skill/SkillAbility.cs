namespace Duo1JFramework.Gameplay.BattleSystem
{
    /// <summary>
    /// 技能能力
    /// </summary>
    public class SkillAbility : Ability
    {
        /// <summary>
        /// 技能配置
        /// </summary>
        public SkillConfig Config { get; private set; }

        /// <summary>
        /// 运行时上下文
        /// </summary>
        public SkillContext Context { get; private set; }

        public SkillAbility(SkillConfig skillConfig)
        {
            Config = skillConfig;
            if (skillConfig != null)
            {
                Id = skillConfig.Id;
                Name = skillConfig.DisplayName;
                InputId = skillConfig.InputId;
                Cooldown = skillConfig.Cooldown;
                CostMana = skillConfig.CostMana;

                if (skillConfig.RequiredTags != null)
                {
                    foreach (string t in skillConfig.RequiredTags)
                    {
                        ActivationRequiredTags.Add(new AbilityTag(t));
                    }
                }
                if (skillConfig.BlockedTags != null)
                {
                    foreach (string t in skillConfig.BlockedTags)
                    {
                        ActivationBlockedTags.Add(new AbilityTag(t));
                    }
                }
                if (skillConfig.OwnedTags != null)
                {
                    foreach (string t in skillConfig.OwnedTags)
                    {
                        ActivationOwnedTags.Add(new AbilityTag(t));
                    }
                }
            }
        }

        protected override void OnActivate(object param)
        {
            if (Config == null || Config.Sequence == null)
            {
                EndAbility();
                return;
            }

            Context = new SkillContext
            {
                Owner = Owner,
                Target = param as CombatUnitController,
                Ability = this,
                Config = Config
            };

            Config.Sequence.Reset();
        }

        protected override void OnTick()
        {
            if (Config == null || Config.Sequence == null)
            {
                return;
            }

            Config.Sequence.Tick(Context);
            if (Config.Sequence.Finished)
            {
                EndAbility();
            }
        }

        protected override void OnCancel()
        {
            if (Config != null && Config.Sequence != null)
            {
                Config.Sequence.Interrupt(Context);
            }
        }
    }
}
