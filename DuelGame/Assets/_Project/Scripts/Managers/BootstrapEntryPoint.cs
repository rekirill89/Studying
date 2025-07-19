using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DuelGame
{
    public class BootstrapEntryPoint : IDisposable
    {
        private readonly SceneLoaderService _sceneLoaderService;
        private readonly GlobalAssetsLoader _globalAssetsLoader;
        
        public BootstrapEntryPoint(SceneLoaderService sceneLoaderService, GlobalAssetsLoader globalAssetsLoader)
        {
            _sceneLoaderService = sceneLoaderService;
            _globalAssetsLoader = globalAssetsLoader;

            _globalAssetsLoader.OnDataLoaded += Init;
        }
        
        public void Dispose()
        {
            _globalAssetsLoader.OnDataLoaded -= Init;
        }

        private void Init(GameConfigs _)
        {
            Debug.Log("Loading next scene...");
            
            var fireBaseInit = new FireBaseInit();
            fireBaseInit.Initialize();
            
            _sceneLoaderService.LoadBattleScene();
        }
    }
}