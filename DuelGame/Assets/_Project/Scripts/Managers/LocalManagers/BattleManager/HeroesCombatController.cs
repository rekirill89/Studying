using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class HeroesCombatController : IDisposable, IInitializable
    {
        private readonly AnalyticsDataCollector _analyticsDataCollector;
        private readonly CancellationTokenSource _cts;
        
        private readonly BaseHero _player1;
        private readonly BaseHero _player2;

        public HeroesCombatController(
            AnalyticsDataCollector analyticsDataCollector, 
            BaseHero player1, Players player1ID, 
            BaseHero player2, Players player2ID)
        {
            _analyticsDataCollector = analyticsDataCollector;
            
            _player1 = player1;
            _player2 = player2;
            
            _player1.SetPlayerID(player1ID);
            _player2.SetPlayerID(player2ID);
            
            _cts = new CancellationTokenSource();
        }
        
        public void Initialize()
        {
            _player1.OnAttack += Player1AttackHandler;
            _player2.OnAttack += Player2AttackHandler;        
        }
        
        public void Dispose()
        {
            _player1.OnAttack -= Player1AttackHandler;
            _player2.OnAttack -= Player2AttackHandler;
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

        private void Player1AttackHandler()
        {
            _analyticsDataCollector.AttackInvoked(_player1.HeroEnum);
        }
        
        private void Player2AttackHandler()
        {
            _analyticsDataCollector.AttackInvoked(_player2.HeroEnum);
        }
        
        private async UniTask LetPlayerAttackInSeconds(BaseHero hero, float timeToAttack)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timeToAttack), cancellationToken: _cts.Token);
            hero.ChangeAttackStatus(true);
        }
    }   
}