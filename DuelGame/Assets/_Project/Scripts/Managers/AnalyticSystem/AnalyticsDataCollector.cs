using System;
using System.Collections.Generic;

namespace DuelGame
{
    public class AnalyticsDataCollector :IDisposable
    {
        private readonly BattleManager _battleManager;
        private readonly AnalyticService _analyticService;

        private readonly Dictionary<HeroEnum, ActionByHeroEnum> _heroEnumActions; 
                
        private Action _player1AttackHandler;
        private Action _player2AttackHandler;
        private BaseHero _player1;
        private BaseHero _player2;
        
        public AnalyticsDataCollector(BattleManager battleManager, AnalyticService analyticService)
        {
            _battleManager = battleManager;
            _analyticService = analyticService;
            
            _heroEnumActions = new Dictionary<HeroEnum, ActionByHeroEnum>()
            {
                {HeroEnum.Archer, new ActionByHeroEnum(_analyticService.LogArrowFired) },
                {HeroEnum.Warrior, new ActionByHeroEnum(_analyticService.LogSwordSwung) },
                {HeroEnum.Wizard, new ActionByHeroEnum(_analyticService.LogWizardFireCasted) },
            };

            _battleManager.OnPlayersSpawned += BattleStarted;
            _battleManager.OnBattleFinish += BattleFinished;
            _battleManager.OnPlayersSpawned += PlayerSpawned;
        }

        public void Dispose()
        {
            _battleManager.OnPlayersSpawned -= BattleStarted;
            _battleManager.OnBattleFinish -= BattleFinished;
            
            if (_player1 != null && !_player1.Equals(null))
                _player1.OnAttack -= _player1AttackHandler;
            if (_player2 != null && !_player2.Equals(null))
                _player2.OnAttack -= _player2AttackHandler;
        }

        private void PlayerSpawned(BattleState _)
        {
            _player1 = _battleManager.HeroesLifecycleController.Player1;
            _player2 = _battleManager.HeroesLifecycleController.Player2;
            
            _player1AttackHandler = () => AttackInvoked(_player1.heroEnum);
            _player2AttackHandler = () => AttackInvoked(_player2.heroEnum);
            
            _player1.OnAttack += _player1AttackHandler;
            _player2.OnAttack += _player2AttackHandler;
        }
        
        private void BattleStarted(BattleState _)
        {
            _analyticService.LogBattleStarted();
        }

        private void BattleFinished(Players? _)
        {
            _analyticService.LogBattleFinished(
                _heroEnumActions[HeroEnum.Archer].AttacksCount, 
                _heroEnumActions[HeroEnum.Warrior].AttacksCount, 
                _heroEnumActions[HeroEnum.Wizard].AttacksCount);
        }

        private void AttackInvoked(HeroEnum heroEnum)
        {
            _heroEnumActions[heroEnum].AttacksCount++;
            _heroEnumActions[heroEnum].Action?.Invoke();
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