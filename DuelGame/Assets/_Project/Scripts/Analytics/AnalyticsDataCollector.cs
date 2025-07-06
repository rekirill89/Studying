using System;
using System.Collections.Generic;

namespace DuelGame
{
    public class AnalyticsDataCollector
    {
        private readonly Dictionary<HeroEnum, ActionByHeroEnum> _heroEnumActions;

        private readonly IAnalyticService _analyticService;

        private BaseHero _player1;
        private BaseHero _player2;
        
        public AnalyticsDataCollector(IAnalyticService analyticService)
        {
            _analyticService = analyticService;
            
            _heroEnumActions = new Dictionary<HeroEnum, ActionByHeroEnum>()
            {
                {HeroEnum.Archer, new ActionByHeroEnum(_analyticService.LogArrowFired) },
                {HeroEnum.Warrior, new ActionByHeroEnum(_analyticService.LogSwordSwung) },
                {HeroEnum.Wizard, new ActionByHeroEnum(_analyticService.LogWizardFireCasted) },
            };
        }
        
        public void AttackInvoked(HeroEnum heroEnum)
        {
            _heroEnumActions[heroEnum].AttacksCount++;
            _heroEnumActions[heroEnum].Action?.Invoke();
        }

        public void LogBattleFinished()
        {
            _analyticService.LogBattleFinished(
                _heroEnumActions[HeroEnum.Archer].AttacksCount, 
                _heroEnumActions[HeroEnum.Warrior].AttacksCount, 
                _heroEnumActions[HeroEnum.Wizard].AttacksCount);
        }
    }

    public class ActionByHeroEnum
    {        
        public readonly Action Action;
        public int AttacksCount = 0;

        public ActionByHeroEnum(Action action)
        {
            Action = action;
        }
    }
}