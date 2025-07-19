using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using IInitializable = Zenject.IInitializable;

namespace DuelGame
{
    public class BattleManager : IDisposable
    {
        public delegate void BattleFinish(Players? playerWhoLost);
        public delegate void PlayerSpawned(BattleState battleState);
        
        public event BattleFinish OnBattleFinish;
        public event PlayerSpawned OnPlayersSpawned;
        public event Action OnBattleReady;
        
        public readonly BattleStateModel BattleStateModel;
        
        private readonly HeroesLifecycleController _heroesLifecycleController;
        private readonly IAnalyticService _analyticService;
        private readonly AnalyticsDataCollector _analyticsDataCollector;
        private readonly BattleSessionContext _battleSessionContext;
        
        private readonly CancellationTokenSource _cts;

        private float _attackDelayP1;
        private float _attackDelayP2;

        private HeroesCombatController _heroesCombatController;
        
        public BattleManager(
            IAnalyticService analyticService,
            AnalyticsDataCollector analyticsDataCollector,
            BattleStateModel battleStateModel, 
            HeroesLifecycleController heroesLifecycleController,
            BattleSessionContext battleSessionContext)
        {
            _analyticService = analyticService;
            _analyticsDataCollector = analyticsDataCollector;
            _heroesLifecycleController = heroesLifecycleController;
            BattleStateModel = battleStateModel;
            _battleSessionContext = battleSessionContext;
            
            _cts = new CancellationTokenSource();
            
            _battleSessionContext.OnSessionReady += Init;

            Debug.Log("Battle Manager created");
        }

        public void Dispose()
        {
            _battleSessionContext.OnSessionReady -= Init;

            _heroesCombatController?.StopAllTasks();
            _heroesLifecycleController?.DestroyHeroes();
            Debug.Log("Battle Manager cleaned");
        }

        public async UniTask RunBattle(bool isContinue = false)
        {
            if (BattleStateModel.CurrentBattleState == BattleState.NotStarted)
                BattleStateModel.SetState(BattleState.Started);

            var (player1, player2) = await _heroesLifecycleController.SpawnHeroes(
                FinishBattle, 
                _cts.Token, (isContinue == true ? _heroesLifecycleController.Player1.HeroEnum : HeroEnum.None));
            
            _heroesCombatController?.StopAllTasks();
            _heroesCombatController = new HeroesCombatController(_analyticsDataCollector, player1, Players.Player1, player2, Players.Player2);
            
            _heroesCombatController.DelayPlayersAttack(_attackDelayP1, _attackDelayP2);
            
            player1.SetEnemy(player2);
            player2.SetEnemy(player1);

            _analyticService.LogBattleStarted();
            OnPlayersSpawned?.Invoke(BattleStateModel.CurrentBattleState);
        }

        public void ContinueBattle()
        {
            BattleStateModel.SetState(BattleState.Continued);
            
            _heroesLifecycleController.DestroyHeroes();
            RunBattle(true).Forget();
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
               
        public void InvokeStartBattle()
        {
            Debug.Log("Battle Ready invoked");
            OnBattleReady?.Invoke();
        }

        private void Init()
        {
            _attackDelayP1 = _battleSessionContext.AttackDelayP1;
            _attackDelayP2 = _battleSessionContext.AttackDelayP2;
        }

        private void FinishBattle(Players playerWhoLost)
        {
            BattleStateModel.SetState(BattleState.Finished);
            
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