using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using IInitializable = Zenject.IInitializable;

namespace DuelGame
{
    public class BattleManager : IDisposable, IInitializable
    {
        public delegate void BattleFinish(Players? playerWhoLost);
        public delegate void PlayerSpawned(BattleState battleState);
        
        public event BattleFinish OnBattleFinish;
        public event PlayerSpawned OnPlayersSpawned;
        public event Action OnBattleReady;
        
        public HeroesLifecycleController HeroesLifecycleController { get; }
        
        private readonly BattleStateModel _battleStateModel;
        
        private readonly float _attackDelayP1;
        private readonly float _attackDelayP2;

        private HeroesCombatController _heroesCombatController;
        
        public BattleManager(
            BattleStateModel battleStateModel, 
            HeroesLifecycleController heroesLifecycleController, 
            BattleSessionContext battleSessionContext)
        {
            HeroesLifecycleController = heroesLifecycleController;

            _attackDelayP1 = battleSessionContext.AttackDelayP1;
            _attackDelayP2 = battleSessionContext.AttackDelayP2;
            
            _battleStateModel = battleStateModel;

            Debug.Log("Battle Manager created");
        }
        
        public void Initialize()
        {
            OnBattleReady?.Invoke();
        }
        
        public void Dispose()
        {
            _heroesCombatController?.StopAllTasks();
            HeroesLifecycleController?.DestroyHeroes();
        }
        
        public void RunBattle()
        {
            if (_battleStateModel.CurrentBattleState == BattleState.NotStarted)
                _battleStateModel.SetState(BattleState.Started);

            var (player1, player2) = HeroesLifecycleController.SpawnHeroes(FinishBattle);
            
            _heroesCombatController?.StopAllTasks();
            _heroesCombatController = new HeroesCombatController(player1, Players.Player1, player2, Players.Player2);
            
            _heroesCombatController.DelayPlayersAttack(_attackDelayP1, _attackDelayP2);
            
            player1.SetEnemy(player2);
            player2.SetEnemy(player1);

            OnPlayersSpawned?.Invoke(_battleStateModel.CurrentBattleState);
        }

        public void ContinueBattle()
        {
            _battleStateModel.SetState(BattleState.Continued);
            
            HeroesLifecycleController.DestroyHeroes();
            RunBattle();
        }

        public BattleData CollectBattleData(Players? playerWhoWon = null)
        {
            return new BattleData()
            {
                Player1 = HeroesLifecycleController.Player1.heroEnum,
                Player2 = HeroesLifecycleController.Player2.heroEnum,
                
                PlayerWhoWon = playerWhoWon
            };
        }
        
        private void FinishBattle(Players playerWhoLost)
        {
            _battleStateModel.SetState(BattleState.Finished);
            
            _heroesCombatController.ChangeAttackStatusToPlayers(false);
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