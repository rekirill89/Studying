using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DuelGame
{
    public class HeroesCombatController
    {
        private readonly CancellationTokenSource _cts;
        
        private readonly BaseHero _player1;
        private readonly BaseHero _player2;

        public HeroesCombatController(BaseHero player1, Players player1ID, BaseHero player2, Players player2ID)
        {
            _player1 = player1;
            _player2 = player2;
            
            _player1.SetPlayerID(player1ID);
            _player2.SetPlayerID(player2ID);
            
            _cts = new CancellationTokenSource();
        }
        
        public void ChangeAttackStatusToPlayers(bool isAttackable)
        {
            _player1.ChangeAttackStatus(isAttackable);
            _player2.ChangeAttackStatus(isAttackable);
        }

        public void DelayPlayersAttack(float timerP1, float timerP2)
        {  
            LetPlayerAttackInSeconds(_player1, timerP1).Forget();
            LetPlayerAttackInSeconds(_player2, timerP2).Forget();
        }
        
        public void StopAllTasks()
        {
            _cts.Cancel();
        }
        
        private async UniTask LetPlayerAttackInSeconds(BaseHero hero, float timeToAttack)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timeToAttack), cancellationToken: _cts.Token);
            hero.ChangeAttackStatus(true);
        }
    }   
}