using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class EntityFactory : IDisposable
    {
        private readonly GlobalAssetsLoader _globalAssetsLoader;
        
        private HeroesList _heroes; 
        private BuffsList _buffs;

        private CancellationTokenSource _cts;
        
        public EntityFactory(GlobalAssetsLoader globalAssetsLoader)
        {
            _globalAssetsLoader = globalAssetsLoader;
            
            _cts = new CancellationTokenSource();
            
            _globalAssetsLoader.OnDataLoaded += InitConfigs;
        }

        public void Dispose()
        {
            _cts.Cancel();
            _globalAssetsLoader.OnDataLoaded -= InitConfigs;
        }

        public async UniTask<BaseHero> SpawnRandomHero(Transform trans)
        {
            var entity = await _heroes.GetRandomHero(_cts.Token);
            var x = Object.Instantiate(entity.HeroScript, trans);
            x.Initialize(entity.HeroStats, _buffs);
            return x;
        }

        public async UniTask<BaseHero> SpawnHeroByEnum(Transform trans, HeroEnum heroEnum)
        {
            var entity = await _heroes.GetHeroEntityByEnum(heroEnum, _cts.Token);
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