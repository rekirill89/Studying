using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using IInitializable = Zenject.IInitializable;

namespace DuelGame
{
    public class BattleManager : IDisposable//, IInitializable
    {
        public delegate void BattleFinish(Players? playerWhoLost);
        public delegate void PlayerSpawned(BattleState battleState);
        
        public event BattleFinish OnBattleFinish;
        public event PlayerSpawned OnPlayersSpawned;
        public event Action OnBattleReady;

        private readonly HeroesLifecycleController _heroesLifecycleController;
        private readonly BattleStateModel _battleStateModel;
        private readonly IAnalyticService _analyticService;
        private readonly AnalyticsDataCollector _analyticsDataCollector;
        private readonly BattleSessionContext _battleSessionContext;
        private readonly BattleSceneAssetsLoader _battleSceneAssetsLoader;

        private float _attackDelayP1;
        private float _attackDelayP2;

        private HeroesCombatController _heroesCombatController;
        
        public BattleManager(
            IAnalyticService analyticService,
            AnalyticsDataCollector analyticsDataCollector,
            BattleStateModel battleStateModel, 
            HeroesLifecycleController heroesLifecycleController,
            BattleSceneAssetsLoader battleSceneAssetsLoader,
            BattleSessionContext battleSessionContext)
        {
            _analyticService = analyticService;
            _analyticsDataCollector = analyticsDataCollector;
            _heroesLifecycleController = heroesLifecycleController;
            _battleStateModel = battleStateModel;
            _battleSceneAssetsLoader = battleSceneAssetsLoader;
            _battleSessionContext = battleSessionContext;
            
            _battleSessionContext.OnSessionReady += Init;
            _battleSceneAssetsLoader.OnReadyToStart += InvokeStartBattle;

            Debug.Log("Battle Manager created");
        }

        public void Dispose()
        {
            _battleSessionContext.OnSessionReady -= Init;
            _battleSceneAssetsLoader.OnReadyToStart -= InvokeStartBattle;

            _heroesCombatController?.StopAllTasks();
            _heroesLifecycleController?.DestroyHeroes();
        }

        public void RunBattle()
        {
            if (_battleStateModel.CurrentBattleState == BattleState.NotStarted)
                _battleStateModel.SetState(BattleState.Started);

            var (player1, player2) = _heroesLifecycleController.SpawnHeroes(FinishBattle);
            
            _heroesCombatController?.StopAllTasks();
            _heroesCombatController = new HeroesCombatController(_analyticsDataCollector, player1, Players.Player1, player2, Players.Player2);
            
            _heroesCombatController.DelayPlayersAttack(_attackDelayP1, _attackDelayP2);
            
            player1.SetEnemy(player2);
            player2.SetEnemy(player1);

            _analyticService.LogBattleStarted();
            OnPlayersSpawned?.Invoke(_battleStateModel.CurrentBattleState);
        }

        public void ContinueBattle()
        {
            _battleStateModel.SetState(BattleState.Continued);
            
            _heroesLifecycleController.DestroyHeroes();
            RunBattle();
        }

        public BattleData CollectBattleData(Players? playerWhoWon = null)
        {
            return new BattleData()
            {
                Player1 = _heroesLifecycleController.Player1.HeroEnum,
                Player2 = _heroesLifecycleController.Player2.HeroEnum,
                
                PlayerWhoWon = playerWhoWon
            };
        }
                
        private void Init()
        {
            _attackDelayP1 = _battleSessionContext.AttackDelayP1;
            _attackDelayP2 = _battleSessionContext.AttackDelayP2;
        }

        private void InvokeStartBattle()
        {
            Debug.Log("Battle Ready invoked");
            OnBattleReady?.Invoke();
        }
        
        private void FinishBattle(Players playerWhoLost)
        {
            _battleStateModel.SetState(BattleState.Finished);
            
            _heroesCombatController.ChangeAttackStatusToPlayers(false);
            
            _analyticsDataCollector.LogBattleFinished();
            OnBattleFinish?.Invoke(playerWhoLost);
        }
    }
    
    public enum HeroEnum
    {
        Archer,
        Warrior,
        Wizard,
        Random,
        None
    }
}