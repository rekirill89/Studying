using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class LocalBattleManager : IBaseManager
    {
        private Transform _firstPlayerTrans;
        private Transform _secondPlayerTrans;

        private BaseHero _player1Obj;
        private BaseHero _player2Obj;

        public GameEventManager gameEventManager { get; set; }
        private EntityFactory _entityFactory;
        private CorutineManager _corutineManager;
        
        public LocalBattleManager(GameEventManager gameEventManager, CorutineManager corutineManager, EntityFactory entityFactory)
        {
            this.gameEventManager = gameEventManager;
            _corutineManager = corutineManager;
            _entityFactory = entityFactory;

            gameEventManager.OnBattleStart += RunBattle;
            gameEventManager.OnBattleContinue += ContinueBattle;
        }
        public void RunBattle()
        {
            _player1Obj = PlayerInitializeHandler(_firstPlayerTrans, Players.Player1, 2f);
            _player2Obj = PlayerInitializeHandler(_secondPlayerTrans, Players.Player2, 3f);

            gameEventManager.PlayersInstantiatedInvoke(_player1Obj, _player2Obj);
        }

        private BaseHero PlayerInitializeHandler(Transform trans, Players player, float timerForAttack)
        {
            var x = _entityFactory.SpawnRandomHero(trans);
            x.GetPlayerID(player);

            x.OnDeath += FinishBattle;

            _corutineManager.StartCoroutine(LetPlayerAttackInSeconds(x, timerForAttack));
            return x;
        }

        public void ContinueBattle()
        {
            DestroyPlayers();
            RunBattle();
        }
        public void FinishBattle(Players playerWhoLost)
        {
            ChangeAttackStatusToPlayers(false);
            gameEventManager.BattleFinishedInvoke(playerWhoLost);
        }
        public void DestroyPlayers()
        {
            Object.Destroy(_player1Obj.gameObject);
            Object.Destroy(_player2Obj.gameObject);
        }
        public void ChangeAttackStatusToPlayers(bool isAttackable)
        {
            _player1Obj.ChangeAttackStatus(isAttackable);
            _player2Obj.ChangeAttackStatus(isAttackable);
        }
        private IEnumerator LetPlayerAttackInSeconds(BaseHero hero, float timeToAttack)
        {
            yield return new WaitForSeconds(timeToAttack);
            hero.ChangeAttackStatus(true);
        }

        public void InitializeBattleElements(LocalBattleManagerFacade facade)
        {
            _firstPlayerTrans = facade.firstPlayerTrans;
            _secondPlayerTrans = facade.secondPlayerTrans;
        }
        
    }
    public enum Players
    {
        Player1,
        Player2
    }
}

