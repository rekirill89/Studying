using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class HeroesLifecycleController : IDisposable
    {
        public BaseHero Player1 { get; private set; }
        public BaseHero Player2 {get; private set;}
        
        private readonly PlayerSettings _player1Settings = new PlayerSettings();
        private readonly PlayerSettings _player2Settings  = new PlayerSettings();
        
        private readonly EntityFactory _entityFactory;
        private readonly BattleSessionContext _battleSessionContext;
        
        private PlayerDeath _onPlayerDeath ;

        private Dictionary<Players, BaseHero> _playerHeroes;

        public HeroesLifecycleController(EntityFactory entityFactory, BattleSessionContext battleSessionContext)
        {
            _entityFactory = entityFactory;
            _battleSessionContext = battleSessionContext;

            _playerHeroes = new Dictionary<Players, BaseHero>()
            {
                { Players.Player1, Player1 },
                { Players.Player2, Player2 },
            };
            
            _battleSessionContext.OnSessionReady += Init;
        }
        
        public void Dispose()
        {
            _battleSessionContext.OnSessionReady -= Init;
            DestroyHeroes();
        }

        public async UniTask<(BaseHero, BaseHero)> SpawnHeroes(PlayerDeath onPlayerDeath, CancellationToken token, HeroEnum player1EnumIfAlive = HeroEnum.None)
        {
            _onPlayerDeath = onPlayerDeath;
            
            Player1 = await CreateHero(_player1Settings.spawnTransform, (player1EnumIfAlive == HeroEnum.None ? _player1Settings.heroEnum : player1EnumIfAlive), token);
            Player2 = await CreateHero(_player2Settings.spawnTransform, _player2Settings.heroEnum, token);
            
            return (Player1, Player2);
        }

        public void DestroyHeroes()
        {
            if (Player1 != null)
            {
                Player1.OnDeath -= StopPlayers;
                Object.Destroy(Player1.gameObject);
            }

            if (Player2 != null)
            {
                Player2.OnDeath -= StopPlayers;
                Object.Destroy(Player2.gameObject);
            }
            /*else
            {
                if (_playerHeroes[playerToDestroy] != null)
                {
                    _playerHeroes[playerToDestroy].OnDeath -= StopPlayers;
                    Object.Destroy(_playerHeroes[playerToDestroy].gameObject);
                }
            }*/
        }

        private void Init()
        {
            _player1Settings.spawnTransform = _battleSessionContext.FirstPlayerTrans;
            _player1Settings.heroEnum = _battleSessionContext.BattleData.Player1;
            
            _player2Settings.spawnTransform = _battleSessionContext.SecondPlayerTrans;
            _player2Settings.heroEnum = _battleSessionContext.BattleData.Player2;
        }
        
        private async UniTask<BaseHero> CreateHero(Transform trans, HeroEnum heroEnum, CancellationToken token)
        {
            var x = heroEnum == HeroEnum.Random
                ? await _entityFactory.SpawnRandomHero(trans)
                : await _entityFactory.SpawnHeroByEnum(trans, heroEnum);
            
            x.OnDeath += StopPlayers;
            return x;
        }

        private void StopPlayers(Players playerWhoLost)
        {
            Player1.StopAllTasks();
            Player2.StopAllTasks();
            _onPlayerDeath?.Invoke(playerWhoLost);
        }
    }

    public class PlayerSettings
    {
        public Transform spawnTransform;
        public HeroEnum heroEnum;
    }
}