using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace DuelGame
{
    public class BattleSceneEntryPoint : IInitializable, IDisposable
    {
        private readonly GlobalBootstrap _globalBootstrap;
        private readonly BattleManager _battleManager;
        private readonly BattleSessionContext _battleSessionContext;
        private readonly HeroesLifecycleController _heroesLifecycleController;
        private readonly BattleSceneAssetsLoader _battleSceneAssetsLoader;
        private readonly UIFactory _uiFactory;
        
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        
        public BattleSceneEntryPoint(
            GlobalBootstrap globalBootstrap,
            BattleManager battleManager, 
            BattleSessionContext battleSessionContext, 
            HeroesLifecycleController heroesLifecycleController,
            BattleSceneAssetsLoader battleSceneAssetsLoader,
            UIFactory uiFactory)
        {
            _globalBootstrap = globalBootstrap;
            _battleManager = battleManager;
            _heroesLifecycleController = heroesLifecycleController;
            _battleSessionContext = battleSessionContext;
            _battleSceneAssetsLoader = battleSceneAssetsLoader;
            _uiFactory = uiFactory;
        }

        public void Initialize()
        {
            WaitForGlobalDataReady().Forget();
        }

        private void StartInitialization()
        {
            
            _battleManager.Initialize();
            _heroesLifecycleController.Initialize();
            _battleSessionContext.Initialize();
            _uiFactory.Initialize();
            _battleSceneAssetsLoader.Initialize();
            
            CheckSystemsInit().Forget();
        }

        public void Dispose()
        {
            _cts.Cancel();
        }
        
        private async UniTask WaitForGlobalDataReady()
        {
            var timeout = UniTask.Delay(TimeSpan.FromSeconds(20), cancellationToken: _cts.Token);
            bool CheckTimeout() => 
                timeout.Status == UniTaskStatus.Succeeded || 
                timeout.Status == UniTaskStatus.Canceled;
            
            await UniTask.WaitUntil(
                () => _globalBootstrap.IsAllSystemReady || CheckTimeout(),
                cancellationToken: _cts.Token);

            if (CheckTimeout())
            {
                Debug.LogError("Global Systems initialize failed");
                return;
            }
            
            Debug.Log("Systems initialized");
            StartInitialization();
        }
        
        private async UniTask CheckSystemsInit()
        {
            var timeout = UniTask.Delay(TimeSpan.FromSeconds(20), cancellationToken: _cts.Token);
            bool CheckTimeout() => 
                timeout.Status == UniTaskStatus.Succeeded || 
                timeout.Status == UniTaskStatus.Canceled;
            
            await UniTask.WaitUntil(
                () => (_uiFactory.IsSystemReady &&  _battleSessionContext.IsSystemReady) || 
                      CheckTimeout(),
                cancellationToken: _cts.Token);
            
            if(CheckTimeout())
                Debug.LogError("Systems initialize failed");
            
            Debug.Log("Systems initialized");
            _battleManager.InvokeStartBattle();
        }
    }
}