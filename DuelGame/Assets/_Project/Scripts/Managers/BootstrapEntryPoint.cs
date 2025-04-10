using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DuelGame
{
    public class BootstrapEntryPoint : IInitializable
    {
        private readonly SceneLoaderManager _sceneLoaderManager;
        
        public BootstrapEntryPoint(SceneLoaderManager sceneLoaderManager)
        {
            _sceneLoaderManager = sceneLoaderManager;
        }
        
        public void Initialize()
        {
            Debug.Log("Loading next scene...");
            _sceneLoaderManager.LoadBattleScene();
        }
    }
}

