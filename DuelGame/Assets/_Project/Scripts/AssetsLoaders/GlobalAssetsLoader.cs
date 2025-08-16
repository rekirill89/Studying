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
        public delegate void ConfigsLoaded(GameLocalConfigs localConfigs, UILocalConfigs uiConfigs);
        public event ConfigsLoaded OnDataLoaded;

        public bool IsSystemReady { get; private set; } = false; 

        private readonly AssetReference _buffsConfigRef;
        private readonly AssetReference _heroesConfigRef;
        private readonly AssetReference _panelsRef;
        private readonly AssetReference _hudCanvasRef;
        private readonly AssetReference _screenCanvasRef;
        
        private readonly DiContainer _diContainer;
        private readonly ILocalAssetLoader _localAssetLoader;
        
        private readonly CancellationTokenSource _cts;
        
        public GlobalAssetsLoader(
            DiContainer diContainer, 
            ILocalAssetLoader localAssetLoader,
            AssetReference buffsConfigRef, 
            AssetReference heroesConfigRef,
            AssetReference panelsRef,
            AssetReference hudCanvasRef,
            AssetReference screenCanvasRef)
        {
            _diContainer = diContainer;
            _localAssetLoader = localAssetLoader;
            
            _buffsConfigRef = buffsConfigRef;
            _heroesConfigRef = heroesConfigRef;
            _panelsRef = panelsRef;
            _hudCanvasRef = hudCanvasRef;
            _screenCanvasRef = screenCanvasRef;
            
            _cts = new CancellationTokenSource();
        }
        
        public void Init()
        {
            Load();
        }

        public void Dispose()
        {
            _localAssetLoader.UnloadAsset(_buffsConfigRef);
            _localAssetLoader.UnloadAsset(_heroesConfigRef);
            _localAssetLoader.UnloadAsset(_panelsRef);
            _localAssetLoader.UnloadAsset(_hudCanvasRef);
            _localAssetLoader.UnloadAsset(_screenCanvasRef);
            _cts.Cancel();
        }

        private void Load()
        {
            LoadAsync().Forget();
        }
        
        private async UniTask LoadAsync()
        {
            try
            {
                var buffsList =
                    Object.Instantiate(await _localAssetLoader.LoadAsset<BuffsList>(_buffsConfigRef, _cts.Token));
                await buffsList.Init(_localAssetLoader, _cts.Token);
                _diContainer.Bind<BuffsList>().FromInstance(buffsList).AsSingle();

                var heroesList =
                    Object.Instantiate(await _localAssetLoader.LoadAsset<HeroesList>(_heroesConfigRef, _cts.Token));
                heroesList.Init(_localAssetLoader);
                await heroesList.LoadAllHeroes(_cts.Token);
                _diContainer.Bind<HeroesList>().FromInstance(heroesList).AsSingle();
                
                var panelsObj = await _localAssetLoader.LoadAsset<GameObject>(_panelsRef, _cts.Token);
                var panels = panelsObj.GetComponent<Panels>();
                await panels.Init(_localAssetLoader, _cts.Token);
                
                var screenCanvasObj = await _localAssetLoader.LoadAsset<GameObject>(_screenCanvasRef, _cts.Token);
                var screenCanvas = screenCanvasObj.GetComponent<Canvas>();
                
                var hudCanvasObj = await _localAssetLoader.LoadAsset<GameObject>(_hudCanvasRef, _cts.Token);
                var hudCanvas = hudCanvasObj.GetComponent<Canvas>();

                OnDataLoaded?.Invoke(
                    new GameLocalConfigs(heroesList, buffsList), 
                    new UILocalConfigs(panels, screenCanvas, hudCanvas));
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Loading asset cancelled");
                throw;
            }

            IsSystemReady = true;
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

    public class UILocalConfigs
    {
        public Canvas ScreenCanvas { get; private set; }
        public Canvas HudCanvas { get; private set; }
        public Panels Panels { get; private set; }

        public UILocalConfigs(Panels panels, Canvas hudCanvas, Canvas screenCanvas)
        {
            Panels = panels;
            HudCanvas = hudCanvas;
            ScreenCanvas = screenCanvas;
        }
    }
}