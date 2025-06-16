using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
using Cysharp.Threading.Tasks;
using Zenject;

namespace DuelGame
{
    public class BattleManager : IDisposable, IInitializable
    {
        public delegate void BattleFinish(Players playerWhoLost);
        public delegate void PlayerSpawned(BattleState battleState);
        
        public event BattleFinish OnBattleFinish;
        public event PlayerSpawned OnPlayersSpawned;
        public event Action OnBattleReady;

        private readonly BattleStateModel _battleStateModel;
        
        private readonly float _attackDelayP1;
        private readonly float _attackDelayP2;
        
        private readonly HeroesService _heroesService;
        private BattleHeroesController _battleHeroesController;
        
        public BattleManager(BattleStateModel battleStateModel, HeroesService heroesService, BattleSettingsFacade facade)
        {
            _heroesService = heroesService;

            _attackDelayP1 = facade.BattleConfig.AttackDelayP1;
            _attackDelayP2 = facade.BattleConfig.AttackDelayP2;
            
            _battleStateModel = battleStateModel;

            Debug.Log("Battle Manager created");
        }
        
        public void Initialize()
        {
            OnBattleReady?.Invoke();
        }
        
        public void Dispose()
        {
            _battleHeroesController?.StopAllTasks();
            _heroesService?.DestroyHeroes();
        }
        
        public void RunBattle()
        {
            if (_battleStateModel.CurrentBattleState == BattleState.NotStarted)
                _battleStateModel.SetState(BattleState.Started);

            var (player1, player2) = _heroesService.SpawnHeroes(FinishBattle);
            
            _battleHeroesController?.StopAllTasks();
            _battleHeroesController = new BattleHeroesController(player1, Players.Player1, player2, Players.Player2);
            
            _battleHeroesController.DelayPlayersAttack(_attackDelayP1, _attackDelayP2);
            
            player1.SetEnemy(player2);
            player2.SetEnemy(player1);

            OnPlayersSpawned?.Invoke(_battleStateModel.CurrentBattleState);
        }

        public void ContinueBattle()
        {
            _battleStateModel.SetState(BattleState.Continued);
            
            _heroesService.DestroyHeroes();
            RunBattle();
        }
        
        private void FinishBattle(Players playerWhoLost)
        {
            _battleStateModel.SetState(BattleState.Finished);
            
            _battleHeroesController.ChangeAttackStatusToPlayers(false);
            OnBattleFinish?.Invoke(playerWhoLost);
        }
    }

    public enum HeroEnum
    {
        Archer,
        Warrior,
        Wizard,
        Random
    }
}