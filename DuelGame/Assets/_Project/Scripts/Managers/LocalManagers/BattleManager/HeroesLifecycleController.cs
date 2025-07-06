using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class HeroesLifecycleController : IDisposable
    {
        public BaseHero Player1 {get; private set;}
        public BaseHero Player2 {get; private set;}
        
        private readonly PlayerSettings _player1Settings = new PlayerSettings();
        private readonly PlayerSettings _player2Settings  = new PlayerSettings();
        
        private readonly EntityFactory _entityFactory;
        private readonly BattleSessionContext _battleSessionContext;
        
        private PlayerDeath _onPlayerDeath ;

        public HeroesLifecycleController(EntityFactory entityFactory, BattleSessionContext battleSessionContext)
        {
            _entityFactory = entityFactory;
            _battleSessionContext = battleSessionContext;

            _battleSessionContext.OnSessionReady += Init;
        }
        
        public void Dispose()
        {
            _battleSessionContext.OnSessionReady -= Init;
            DestroyHeroes();
        }

        public (BaseHero, BaseHero) SpawnHeroes(PlayerDeath onPlayerDeath)
        {
            _onPlayerDeath = onPlayerDeath;
            
            Player1 = CreateHero(_player1Settings.spawnTransform, _player1Settings.heroEnum);
            Player2 = CreateHero(_player2Settings.spawnTransform, _player2Settings.heroEnum);
            
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
        }

        private void Init()
        {
            _player1Settings.spawnTransform = _battleSessionContext.FirstPlayerTrans;
            _player1Settings.heroEnum = _battleSessionContext.BattleData.Player1;
            
            _player2Settings.spawnTransform = _battleSessionContext.SecondPlayerTrans;
            _player2Settings.heroEnum = _battleSessionContext.BattleData.Player2;
        }
        
        private BaseHero CreateHero(Transform trans, HeroEnum heroEnum)
        {
            var x = heroEnum == HeroEnum.Random
                ? _entityFactory.SpawnRandomHero(trans)
                : _entityFactory.SpawnHeroByEnum(trans, heroEnum);
            
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