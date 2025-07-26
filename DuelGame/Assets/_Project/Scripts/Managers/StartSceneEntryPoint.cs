using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DuelGame
{
    public class StartSceneEntryPoint : IDisposable, IInitializable
    {
        private readonly SceneLoaderService _sceneLoaderService;
        private readonly GlobalAssetsLoader _globalAssetsLoader;
        
        public StartSceneEntryPoint(SceneLoaderService sceneLoaderService, GlobalAssetsLoader globalAssetsLoader)
        {
            _sceneLoaderService = sceneLoaderService;
            _globalAssetsLoader = globalAssetsLoader;
        }
        
        public void Initialize()
        {
            _globalAssetsLoader.OnDataLoaded += Init;
        }
        
        public void Dispose()
        {
            _globalAssetsLoader.OnDataLoaded -= Init;
        }

        private void Init(GameLocalConfigs _)
        {
            Debug.Log("Loading next scene...");
            _sceneLoaderService.LoadBattleScene();
        }
    }
}