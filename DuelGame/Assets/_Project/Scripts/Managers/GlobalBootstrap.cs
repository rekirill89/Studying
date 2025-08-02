using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DuelGame
{
    public class GlobalBootstrap : IInitializable, IDisposable
    {
        public bool IsAllSystemReady { get; private set; } = false;
        
        private readonly IRemoteConfigsLoader _remoteConfigsLoader;
        private readonly GlobalAssetsLoader _globalAssetsLoader;
        private readonly FireBaseInit _fireBaseInit;
        private readonly EntityFactory _entityFactory;
        private readonly UIFactory _uiFactory;
        private readonly IInAppPurchaseService _inAppPurchaseService;
        private readonly PurchasesDataController _purchasesDataController;

        private readonly CancellationTokenSource _cts;

        public GlobalBootstrap(
            IRemoteConfigsLoader remoteConfigsLoader, 
            GlobalAssetsLoader globalAssetsLoader, 
            FireBaseInit fireBaseInit,
            EntityFactory entityFactory,
            UIFactory uiFactory,
            IInAppPurchaseService inAppPurchaseService)
        {
            _remoteConfigsLoader = remoteConfigsLoader;
            _globalAssetsLoader = globalAssetsLoader;
            _fireBaseInit = fireBaseInit;
            _entityFactory = entityFactory;
            _uiFactory = uiFactory;
            _inAppPurchaseService = inAppPurchaseService;
            
            _cts = new CancellationTokenSource();
        }
        
        public void Initialize()
        {
            _fireBaseInit.Init();
            _entityFactory.Init();
            _uiFactory.Init();
            _remoteConfigsLoader.Init();
            _globalAssetsLoader.Init();
            _inAppPurchaseService.Init();
            
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
                _inAppPurchaseService.IsSystemReady) || 
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

        public void Dispose()
        {
            _cts.Cancel();
        }
    }
}