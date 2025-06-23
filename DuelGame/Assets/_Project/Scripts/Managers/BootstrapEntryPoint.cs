using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DuelGame
{
    public class BootstrapEntryPoint : IInitializable
    {
        private readonly SceneLoaderService _sceneLoaderService;
        
        public BootstrapEntryPoint(SceneLoaderService sceneLoaderService)
        {
            _sceneLoaderService = sceneLoaderService;
        }
        
        public void Initialize()
        {
            Debug.Log("Loading next scene...");
            _sceneLoaderService.LoadBattleScene();
        }
    }
}