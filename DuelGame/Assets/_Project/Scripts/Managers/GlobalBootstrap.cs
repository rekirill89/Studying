using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class GlobalBootstrap : IInitializable, IDisposable
    {
        public bool IsAllSystemReady { get; private set; } = false;
        
        private readonly IRemoteConfigsManager _remoteConfigsManager;
        private readonly GlobalAssetsLoader _globalAssetsLoader;
        private readonly FireBaseInit _fireBaseInit;
        private readonly EntityFactory _entityFactory;

        private readonly CancellationTokenSource _cts;

        public GlobalBootstrap(
            IRemoteConfigsManager remoteConfigsManager, 
            GlobalAssetsLoader globalAssetsLoader, 
            FireBaseInit fireBaseInit,
            EntityFactory entityFactory)
        {
            _remoteConfigsManager = remoteConfigsManager;
            _globalAssetsLoader = globalAssetsLoader;
            _fireBaseInit = fireBaseInit;
            _entityFactory = entityFactory;
            
            _cts = new CancellationTokenSource();
        }
        
        public void Initialize()
        {
            _fireBaseInit.Init();
            _entityFactory.Init();
            _remoteConfigsManager.Init();
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
                _remoteConfigsManager.IsSystemReady &&
                _globalAssetsLoader.IsSystemReady) || 
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