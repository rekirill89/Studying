using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DuelGame
{
    public class MenuSceneEntryPoint : IInitializable
    {
        private readonly SceneLoaderService _sceneLoaderService;
        private readonly GlobalBootstrap _globalBootstrap;
        private readonly UIFactory _uiFactory;
        private readonly IInstantiator _instantiator;
        
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        
        public MenuSceneEntryPoint(
            SceneLoaderService sceneLoaderService, 
            GlobalBootstrap globalBootstrap,
            UIFactory uiFactory,
            IInstantiator instantiator)
        {
            _sceneLoaderService = sceneLoaderService;
            _globalBootstrap = globalBootstrap;
            _uiFactory = uiFactory;
            _instantiator = instantiator;
        }
        
        public void Initialize()
        {
            WaitForGlobalDataReady().Forget();
        }

        public void StartGame()
        {
            Debug.Log("Loading next scene...");
            _sceneLoaderService.LoadBattleScene();
        }
        
        private async UniTask WaitForGlobalDataReady()
        {
            var timeout = UniTask.Delay(TimeSpan.FromSeconds(40), cancellationToken: _cts.Token);
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
            Init();
        }
        
        private void Init()
        {
            var presenter = _instantiator.Instantiate<MenuPanelPresenter>(
                new object[] { this, _uiFactory.CreatePanelView<MenuPanelPresenter>() }
                );
            presenter.Initialize();
            presenter.ShowView();
        }
    }
}