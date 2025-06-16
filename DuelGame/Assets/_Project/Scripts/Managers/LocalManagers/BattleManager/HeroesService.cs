using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class HeroesService : IDisposable
    {        
        private readonly PlayerSettings _player1Settings = new PlayerSettings();
        private readonly PlayerSettings _player2Settings = new PlayerSettings();
        
        private readonly EntityFactory _entityFactory;
        
        private PlayerDeath _onPlayerDeath ;
        private BaseHero _player1;
        private BaseHero _player2;

        public HeroesService(EntityFactory entityFactory, BattleSettingsFacade facade)
        {
            _entityFactory = entityFactory;
            
            _player1Settings.spawnTransform = facade.FirstPlayerTrans;
            _player1Settings.heroEnum = facade.BattleConfig.FirstHero;
            
            _player2Settings.spawnTransform = facade.SecondPlayerTrans;
            _player2Settings.heroEnum = facade.BattleConfig.SecondHero;
        }
        
        public void Dispose()
        {
            DestroyHeroes();
        }
        
        public (BaseHero, BaseHero) SpawnHeroes(PlayerDeath onPlayerDeath)
        {
            _onPlayerDeath = onPlayerDeath;
            
            _player1 = CreateHero(_player1Settings.spawnTransform, _player1Settings.heroEnum);
            _player2 = CreateHero(_player2Settings.spawnTransform, _player2Settings.heroEnum);
            
            return (_player1, _player2);
        }

        public void DestroyHeroes()
        {
            if (_player1 != null)
            {
                _player1.OnDeath -= StopPlayers;
                Object.Destroy(_player1.gameObject);
            }

            if (_player2 != null)
            {
                _player2.OnDeath -= StopPlayers;
                Object.Destroy(_player2.gameObject);
            }
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
            _player1.StopAllTasks();
            _player2.StopAllTasks();
            _onPlayerDeath?.Invoke(playerWhoLost);
        }
    }

    public class PlayerSettings
    {
        public Transform spawnTransform;
        public HeroEnum heroEnum;
    }
}