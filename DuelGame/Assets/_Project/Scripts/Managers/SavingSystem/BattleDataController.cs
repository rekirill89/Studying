using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DuelGame
{
    public class BattleDataController : IDisposable, IInitializable
    {
        private readonly SaveService _saveService;
        private readonly BattleManager _battleManager;
        private readonly BattleDataCache _battleDataCache;
        private readonly SceneLoaderService _sceneLoaderService;
        
        public BattleDataController(
            SaveService saveService, 
            BattleManager battleManager, 
            BattleDataCache battleDataCache, 
            SceneLoaderService sceneLoaderService)
        {
            _saveService = saveService;
            _battleManager = battleManager;
            _battleDataCache = battleDataCache;
            _sceneLoaderService = sceneLoaderService;
        }

        public void Initialize()
        {
            _battleManager.OnBattleFinish += SaveBattleData;
        }
        
        public void Dispose()
        {
            _battleManager.OnBattleFinish -= SaveBattleData;
        }
        
        public void SaveBattleData(Players? playerWhoLost)
        {
            _saveService.Save(_battleManager.CollectBattleData(playerWhoLost));
        }

        public void LoadBattleData()
        {
            var battleData = _saveService.Load();

            if (battleData == null)
            {
                Debug.LogWarning("Nothing to load");
                return;
            }
            
            _battleDataCache.SetBattleData(battleData);
            _battleDataCache.ChangeLoadingStatus(true);
            
            _sceneLoaderService.LoadBattleScene();
        }

        /*public void LoadAutoSaveBattleData()
        {
            var battleData = _saveService.Load();
            
            _battleDataCache.SetBattleData(battleData);
            _battleDataCache.ChangeLoadingStatus(true);
            
            _sceneLoaderService.LoadBattleScene();
        }*/
        
        /*private void AutoSaveBattleData(Players? playerWhoLost)
        {
            var data = _battleManager.CollectBattleData(playerWhoLost);
            Debug.Log(data.Player1 + " " + data.Player2 + " " + data.PlayerWhoWon);
            
            _saveService.Save(data);
        }*/
    }
    
    public class BattleData
    {
        public HeroEnum Player1;
        public HeroEnum Player2;

        public Players? PlayerWhoWon = Players.None;
    }
}