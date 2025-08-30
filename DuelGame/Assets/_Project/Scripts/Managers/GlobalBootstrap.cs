using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Zenject;

namespace DuelGame
{
    public class GlobalBootstrap : IInitializable, IDisposable
    {
        public bool IsAllSystemReady { get; private set; } = false;
        
        private readonly IRemoteConfigsLoader _remoteConfigsLoader;
        private readonly GlobalAssetsLoader _globalAssetsLoader;
        private readonly SkinAssetsLoader _skinAssetsLoader;
        private readonly SkinsController _skinsController;
        private readonly FireBaseInit _fireBaseInit;
        private readonly EntityFactory _entityFactory;
        private readonly UIFactory _uiFactory;
        private readonly IInAppPurchaseService _inAppPurchaseService;
        private readonly PurchasesDataController _purchasesDataController;
        private readonly IAuthService _authService;
        
        private readonly IInternetConnector _internetConnector;

        private readonly CancellationTokenSource _cts;

        public GlobalBootstrap(
            IRemoteConfigsLoader remoteConfigsLoader, 
            GlobalAssetsLoader globalAssetsLoader, 
            SkinAssetsLoader skinAssetsLoader,
            SkinsController skinsController,
            FireBaseInit fireBaseInit,
            EntityFactory entityFactory,
            UIFactory uiFactory,
            IAuthService authService,
            IInAppPurchaseService inAppPurchaseService,
            IInternetConnector internetConnector)
        {
            _remoteConfigsLoader = remoteConfigsLoader;
            _globalAssetsLoader = globalAssetsLoader;
            _skinAssetsLoader = skinAssetsLoader;
            _skinsController = skinsController;
            _fireBaseInit = fireBaseInit;
            _entityFactory = entityFactory;
            _uiFactory = uiFactory;
            _authService = authService;
            _inAppPurchaseService = inAppPurchaseService;
            
            _internetConnector = internetConnector;
            
            _cts = new CancellationTokenSource();
        }
        
        public void Initialize()
        {
            _internetConnector.OnConnected += OnInternetConnected;
            
            InitializeAsync().Forget();
        }

        public void Dispose()
        {
            _cts.Cancel();
        }
        
        private async UniTask InitializeAsync()
        {
            await _internetConnector.CheckInternetConnection();
            
            _internetConnector.MonitorInternetConnection().Forget();
            
            _fireBaseInit.Init();
            _entityFactory.Init();
            _skinsController.Init();
            _uiFactory.Init();
            _remoteConfigsLoader.Init();
            
            _skinAssetsLoader.Init();
            _globalAssetsLoader.Init();
            
            WaitUntilAllSystemsReady().Forget();
        }

        private async UniTask WaitUntilAllSystemsReady()
        {
            var timeout = UniTask.Delay(TimeSpan.FromSeconds(20),cancellationToken: _cts.Token);
            bool CheckTimeout() => 
                timeout.Status == UniTaskStatus.Succeeded || 
                timeout.Status == UniTaskStatus.Canceled; 
            
            await UniTask.WaitUntil(()=> 
                (_fireBaseInit.IsSystemReady && 
                _entityFactory.IsSystemReady && 
                _remoteConfigsLoader.IsSystemReady &&
                _globalAssetsLoader.IsSystemReady &&
                _skinAssetsLoader.IsSystemReady) || 
                CheckTimeout(), 
                cancellationToken: _cts.Token);

            if (CheckTimeout())
            {
                Debug.LogError("Global systems initialize failed");
                return;
            }
            
            Debug.Log("Global systems initialized");
            IsAllSystemReady = true;
        }
        
        private void OnInternetConnected()
        {
            OnInternetConnectedAsync().Forget();
        }
        
        private async UniTask OnInternetConnectedAsync()
        {
            await UnityServices.InitializeAsync();
            
            _authService.Init();
            _inAppPurchaseService.Init();
            
            _internetConnector.OnConnected -= OnInternetConnected;

        }
    }
}