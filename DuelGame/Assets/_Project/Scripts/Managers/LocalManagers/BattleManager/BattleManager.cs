using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
using Cysharp.Threading.Tasks;
using Zenject;

namespace DuelGame
{
    public class BattleManager : IDisposable
    {
        public delegate void BattleFinish(Players playerWhoWon);
        public delegate void PlayerSpawned(BattleState battleState);
        
        public event BattleFinish OnBattleFinish;
        public event PlayerSpawned OnPlayersSpawned;

        private readonly BattleStateModel _battleStateModel;
        
        private readonly HeroEnum _firstHero;
        private readonly HeroEnum _secondHero;
        
        private readonly Transform _firstPlayerTrans;
        private readonly Transform _secondPlayerTrans;

        private readonly EntityFactory _entityFactory;
        
        public BattleManager(EntityFactory entityFactory, BattleStateModel battleStateModel, BattleManagerFacade facade)
        {
            _entityFactory = entityFactory;

            _firstHero = facade.firstHero;
            _secondHero = facade.secondHero;
            
            _firstPlayerTrans = facade.firstPlayerTrans;
            _secondPlayerTrans = facade.secondPlayerTrans;
            
            _battleStateModel = battleStateModel;

            Debug.Log("Battle Manager created");
        }
        
        public void RunBattle()
        {
            if (_battleStateModel.currentBattleState == BattleState.NotStarted)
                _battleStateModel.SetState(BattleState.Started);;

            float player1Timer = 2f;
            float player2Timer = 3f;
            
            _battleStateModel.player1 = PlayerInitializeHandler(_firstPlayerTrans, Players.Player1, player1Timer, _firstHero);
            _battleStateModel.player2 = PlayerInitializeHandler(_secondPlayerTrans, Players.Player2, player2Timer, _secondHero);

            OnPlayersSpawned?.Invoke(_battleStateModel.currentBattleState);
        }

        public void ContinueBattle()
        {
            _battleStateModel.SetState(BattleState.Continued);
            DestroyPlayers();
            RunBattle();
        }
        
        private void FinishBattle(Players playerWhoLost)
        {
            _battleStateModel.SetState(BattleState.Finished);
            ChangeAttackStatusToPlayers(false);
            OnBattleFinish?.Invoke(playerWhoLost);
        }
        
        private void DestroyPlayers()
        {
            Object.Destroy(_battleStateModel.player1.gameObject);
            Object.Destroy(_battleStateModel.player2.gameObject);
        }

        private void ChangeAttackStatusToPlayers(bool isAttackable)
        {
            _battleStateModel.player1.ChangeAttackStatus(isAttackable);
            _battleStateModel.player2.ChangeAttackStatus(isAttackable);
        }

        private BaseHero PlayerInitializeHandler(Transform trans, Players player, float timerForAttack, HeroEnum heroEnum)
        {
            BaseHero hero = heroEnum == HeroEnum.Random
                ? _entityFactory.SpawnRandomHero(trans)
                : _entityFactory.SpawnHeroByEnum(trans, heroEnum);
            
            hero.SetPlayerID(player);

            hero.OnDeath += FinishBattle;

            var y = LetPlayerAttackInSeconds(hero, timerForAttack);
            return hero;
        }

        private async UniTask LetPlayerAttackInSeconds(BaseHero hero, float timeToAttack)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timeToAttack));
            hero.ChangeAttackStatus(true);
        }  
        
        public void Dispose()
        {
             _battleStateModel.player1.OnDeath -= FinishBattle;
             _battleStateModel.player2.OnDeath -= FinishBattle;
             if(_battleStateModel.player1 != null || _battleStateModel.player2 != null)
                 DestroyPlayers();
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

