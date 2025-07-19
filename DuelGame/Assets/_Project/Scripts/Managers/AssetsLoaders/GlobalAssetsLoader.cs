using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Zenject;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class GlobalAssetsLoader : IInitializable, IDisposable
    {
        public delegate void ConfigsLoaded(GameConfigs configs);
        public event ConfigsLoaded OnDataLoaded;

        private readonly AssetReference _buffsConfigRef;
        private readonly AssetReference _heroesConfigRef;
        
        private readonly DiContainer _diContainer;
        private readonly ILocalAssetLoader _localAssetLoader;
        
        private CancellationTokenSource _cts;
        
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
            
            _cts = new CancellationTokenSource();
        }
        
        public void Initialize()
        {
            Load().Forget();
        }

        public void Dispose()
        {
            _cts.Cancel();
        }
        
        private async UniTask Load()
        {
            var buffsList = Object.Instantiate(await _localAssetLoader.LoadAsset<BuffsList>(_buffsConfigRef, _cts.Token));
            await buffsList.Init(_localAssetLoader, _cts.Token);
            _diContainer.Bind<BuffsList>().FromInstance(buffsList).AsSingle();
            
            var heroesList = Object.Instantiate(await  _localAssetLoader.LoadAsset<HeroesList>(_heroesConfigRef, _cts.Token));
            heroesList.Init(_localAssetLoader);
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