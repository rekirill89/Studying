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
    public class GlobalAssetsLoader : IDisposable
    {
        public delegate void ConfigsLoaded(GameLocalConfigs localConfigs);
        public event ConfigsLoaded OnDataLoaded;

        public bool IsSystemReady { get; private set; } = false; 

        private readonly AssetReference _buffsConfigRef;
        private readonly AssetReference _heroesConfigRef;
        
        private readonly DiContainer _diContainer;
        private readonly ILocalAssetLoader _localAssetLoader;
        
        private readonly CancellationTokenSource _cts;
        
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
        
        public void Init()
        {
            LoadMethod();
        }

        public void Dispose()
        {
            _localAssetLoader.UnloadAsset(_buffsConfigRef);
            _localAssetLoader.UnloadAsset(_heroesConfigRef);
            _cts.Cancel();
        }

        private void LoadMethod()
        {
            Load().Forget();
        }
        
        private async UniTask Load()
        {
            var buffsList = Object.Instantiate(await _localAssetLoader.LoadAsset<BuffsList>(_buffsConfigRef, _cts.Token));
            await buffsList.Init(_localAssetLoader, _cts.Token);
            _diContainer.Bind<BuffsList>().FromInstance(buffsList).AsSingle();
            
            var heroesList = Object.Instantiate(await  _localAssetLoader.LoadAsset<HeroesList>(_heroesConfigRef, _cts.Token));
            heroesList.Init(_localAssetLoader);
            _diContainer.Bind<HeroesList>().FromInstance(heroesList).AsSingle();

            IsSystemReady = true;
            OnDataLoaded?.Invoke(new GameLocalConfigs(heroesList, buffsList));
            Debug.Log("Global assets successfully loaded");
        }
    }

    public class GameLocalConfigs
    {
        public HeroesList HeroesList { get; private set; }
        public BuffsList BuffsList { get; private set; }

        public GameLocalConfigs(HeroesList heroesList, BuffsList buffsList)
        {
            HeroesList = heroesList;
            BuffsList = buffsList;
        }
    }
}