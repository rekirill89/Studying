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
        public delegate void PlayerSpawned(BaseHero hero1, BaseHero hero2);
        
        public event BattleFinish OnBattleFinish;
        public event PlayerSpawned OnPlayersSpawned;
                
        private Transform _firstPlayerTrans;
        private Transform _secondPlayerTrans;

        private BaseHero _player1Obj;
        private BaseHero _player2Obj;

        private readonly EntityFactory _entityFactory;
        
        public BattleManager(EntityFactory entityFactory, BattleManagerFacade facade)
        {
            _entityFactory = entityFactory;

            _firstPlayerTrans = facade.firstPlayerTrans;
            _secondPlayerTrans = facade.secondPlayerTrans;

            Debug.Log("Battle Manager created");
        }
        
        public void RunBattle()
        {
            _player1Obj = PlayerInitializeHandler(_firstPlayerTrans, Players.Player1, 2f);
            _player2Obj = PlayerInitializeHandler(_secondPlayerTrans, Players.Player2, 3f);

            OnPlayersSpawned?.Invoke(_player1Obj, _player2Obj);
        }

        public void ContinueBattle()
        {
            DestroyPlayers();
            RunBattle();
        }
        
        /*public void InitializeBattleElements(BattleManagerFacade facade)
        {
            _firstPlayerTrans = facade.firstPlayerTrans;
            _secondPlayerTrans = facade.secondPlayerTrans;
        }*/

        public void Dispose()
        {
            if(_player1Obj != null || _player2Obj != null)
                DestroyPlayers();
        }
        
        
        private void FinishBattle(Players playerWhoLost)
        {
            ChangeAttackStatusToPlayers(false);
            OnBattleFinish?.Invoke(playerWhoLost);
        }
        
        private void DestroyPlayers()
        {
            Object.Destroy(_player1Obj.gameObject);
            Object.Destroy(_player2Obj.gameObject);
        }

        private void ChangeAttackStatusToPlayers(bool isAttackable)
        {
            _player1Obj.ChangeAttackStatus(isAttackable);
            _player2Obj.ChangeAttackStatus(isAttackable);
        }

        private BaseHero PlayerInitializeHandler(Transform trans, Players player, float timerForAttack)
        {
            var x = _entityFactory.SpawnRandomHero(trans);
            x.SetPlayerID(player);

            x.OnDeath += FinishBattle;

            LetPlayerAttackInSeconds(x, timerForAttack).Forget();
            return x;
        }

        private async UniTask LetPlayerAttackInSeconds(BaseHero hero, float timeToAttack)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timeToAttack));
            hero.ChangeAttackStatus(true);
        }
    }
    public enum Players
    {
        Player1,
        Player2
    }
}

