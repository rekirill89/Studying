using UnityEngine;
using System.Linq;
using Zenject;
using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DuelGame
{
    public class BuffsManager : IDisposable
    {        
        private CancellationTokenSource _cts;
        
        private readonly BuffsList _buffs;
        private readonly BattleManager _localBattleManager;

        private Dictionary<BuffEnum, Func<Buff>> _buffsDictionary;
        
        private BaseHero _player1;
        private BaseHero _player2;

        public BuffsManager(BuffsList buffs, BattleManager localBattleManager)
        {
            _localBattleManager = localBattleManager;
            _localBattleManager.OnPlayersSpawned += PlayersInstantiatedHandler;
            _buffs = buffs; 
            
            _cts = new CancellationTokenSource();
            _buffsDictionary = new Dictionary<BuffEnum, Func<Buff>>()
            {
                { BuffEnum.Poison, () => new PoisonBuff(_cts.Token) },
                { BuffEnum.Stun, () => new StunBuff() },
                { BuffEnum.DecreaseDamage, () => new DecreaseDamageBuff(_cts.Token) }
            };
        }
        private void PlayersInstantiatedHandler(BaseHero player1, BaseHero player2)
        {
            _player1 = player1;
            _player2 = player2;
            
            //Debug.Log($"Player1: {_player1.name}, Player2: {_player2.name}");
            
            _player1.OnBuffGot += BuffApply;
            _player2.OnBuffGot += BuffApply;
            
            _player1.OnDeath += (Players _) => _cts.Cancel();
            _player2.OnDeath += (Players _) => _cts.Cancel();
            
            Debug.Log("Buffs events registered");
        }

        private void BuffApply(BuffEnum buffEnum, BaseHero player)
        {
            Debug.Log(buffEnum);
            if (!_buffsDictionary.ContainsKey(buffEnum)) 
                return;
            
            var buffEntity = _buffs.listOfEntities.First(x => x.buff == buffEnum);
            Buff buff = _buffsDictionary[buffEnum]();

            buff.DoBuff(player);
            player.BuffApliedInvoke(buffEntity.sp.sprite, buff.BuffDuration);
        }

        public void Dispose()
        {
            _localBattleManager.OnPlayersSpawned -= PlayersInstantiatedHandler;
        }
    }
}

