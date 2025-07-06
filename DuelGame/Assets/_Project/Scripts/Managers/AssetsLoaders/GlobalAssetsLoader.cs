using System;
using System.Threading.Tasks;
using Zenject;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class GlobalAssetsLoader : IInitializable
    {
        public delegate void ConfigsLoaded(GameConfigs configs);
        public event ConfigsLoaded OnDataLoaded;

        private readonly AssetReference _buffsConfigRef;
        private readonly AssetReference _heroesConfigRef;
        
        private readonly DiContainer _diContainer;
        private readonly ILocalAssetLoader _localAssetLoader;
        
        public GlobalAssetsLoader(
            DiContainer diContainer, 
            ILocalAssetLoader localAssetLoader, 
            AssetReference buffsConfigRef, 
            AssetReference heroesConfigRef)
        {
            _diContainer = diContainer;
            _localAssetLoader = localAssetLoader;
            
            _buffsConfigRef = buffsConfigRef;
            _heroesConfigRef = heroesConfigRef;
        }
        
        public void Initialize()
        {
            var t = Load();
            
            _ = new FireBaseInit();
        }

        private async Task Load()
        {
            var buffsList = Object.Instantiate(await _localAssetLoader.LoadAsset<BuffsList>(_buffsConfigRef));
            await buffsList.Init(_localAssetLoader);
            _diContainer.Bind<BuffsList>().FromInstance(buffsList).AsSingle();
            
            var heroesList = Object.Instantiate(await  _localAssetLoader.LoadAsset<HeroesList>(_heroesConfigRef));
            await heroesList.Init(_localAssetLoader);
            _diContainer.Bind<HeroesList>().FromInstance(heroesList).AsSingle();

            OnDataLoaded?.Invoke(new GameConfigs(heroesList, buffsList));
        }
    }

    public class GameConfigs
    {
        public HeroesList HeroesList { get; private set; }
        public BuffsList BuffsList { get; private set; }

        public GameConfigs(HeroesList heroesList, BuffsList buffsList)
        {
            HeroesList = heroesList;
            BuffsList = buffsList;
        }
    }
}