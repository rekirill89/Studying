using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class EntityFactory : IDisposable
    {
        private readonly GlobalAssetsLoader _globalAssetsLoader;
        
        private HeroesList _heroes; 
        private BuffsList _buffs; 
        
        public EntityFactory(GlobalAssetsLoader globalAssetsLoader)
        {
            _globalAssetsLoader = globalAssetsLoader;
            _globalAssetsLoader.OnDataLoaded += InitConfigs;
        }

        public void Dispose()
        {
            _globalAssetsLoader.OnDataLoaded -= InitConfigs;
        }

        public BaseHero SpawnRandomHero(Transform trans)
        {
            var entity = _heroes.GetRandomEntity();
            var x = Object.Instantiate(entity.HeroScript, trans);
            x.Initialize(entity.HeroStats, _buffs);
            return x;
        }

        public BaseHero SpawnHeroByEnum(Transform trans, HeroEnum heroEnum)
        {
            var entity = _heroes.GetHeroEntityByEnum(heroEnum);
            var x = Object.Instantiate(entity.HeroScript, trans);
            x.Initialize(entity.HeroStats, _buffs);
            return x;
        }        
        
        private void InitConfigs(GameConfigs configs)
        {
            _heroes = configs.HeroesList;
            _buffs = configs.BuffsList;
            
            Debug.Log($"Factory initialized");
        }
    }
}